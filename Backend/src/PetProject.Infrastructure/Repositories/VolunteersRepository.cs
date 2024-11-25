using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Application.VolunteersManagement;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.Enums;
using PetProject.Infrastructure.Postgres.Abstractions;

namespace PetProject.Infrastructure.Postgres.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly PetProjectDbContext _context;
    private readonly IPostgresConnectionFactory _connectionFactory;

    public VolunteersRepository(
        PetProjectDbContext context,
        IPostgresConnectionFactory connectionFactory)
    {
        _context = context;
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.Volunteers.AddAsync(volunteer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Guid, Error>> Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        var existedVolunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);

        if (existedVolunteer == null)
            return Errors.General.NotFound();

        _context.Volunteers.Remove(existedVolunteer);
        await _context.SaveChangesAsync(cancellationToken);
        return volunteer.Id.Id;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default)
    {
        var volunteer = await _context.Volunteers
            .Where(v => v.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound(id.Id);

        return volunteer;
    }
    public async Task<VolunteerDto[]> Query(VolunteerQueryModel query,
        CancellationToken cancellationToken = default)
    {
        var sqlQuery = """
                        SELECT 
                            v.name, 
                            v.surname, 
                            v.patronymic, 
                            v.general_description, 
                            v.phone_number, 
                            v.age_experience,
                            v.requisites,
                            v.social_links,
                            p.pet_name,
                            p.general_description,
                            p.health_information,
                            p.species_name,
                            p.breed_name,
                            p.country,
                            p.city,
                            p.street,
                            p.house,
                            p.flat,
                            p.weight,
                            p.height,
                            p.birth_date,
                            p.is_castrated,
                            p.is_vaccinated,
                            p.help_status
                        FROM 
                            volunteers v
                        left join 
                            pets p on v.id = p.volunteer_id AND p.is_deleted = false
                       """;

        var conditions = new List<string>(["v.is_deleted = false"]);
        var param = new DynamicParameters();

        if (query.VolunteerIds is { Length: > 0 })
        {
            conditions.Add("v.id = any(@VolunteerIds)");
            param.Add("VolunteerIds", query.VolunteerIds);
        }

        if (query.PetIds is { Length: > 0 })
        {
            conditions.Add("p.id = any(@PetIds)");
            param.Add("PetIds", query.PetIds);
        }
        
        if (query.SpeciesIds is { Length: > 0 })
        {
            conditions.Add("p.species_id = any(@SpeciesIds)");
            param.Add("SpeciesIds", query.SpeciesIds);
        }
        
        if (query.BreedIds is { Length: > 0 })
        {
            conditions.Add("p.breed_id = any(@BreedIds)");
            param.Add("BreedIds", query.BreedIds);
        }
        
        if (string.IsNullOrEmpty(query.PhoneNumber) == false)
        {
            conditions.Add("v.phone_number = @PhoneNumber");
            param.Add("PhoneNumber", query.PhoneNumber);
        }

        sqlQuery += " where " + string.Join(" and ", conditions);

        if (query.Limit > 0)
        {
            sqlQuery += " limit @Limit ";
            param.Add("Limit", query.Limit);
        }

        if (query.Offset > 0)
        {
            sqlQuery += " offset @Offset ";
            param.Add("Offset", query.Offset);
        }

        var command = new CommandDefinition(sqlQuery, param, cancellationToken: cancellationToken);

        await using var connection = _connectionFactory.GetConnection();

        await using var reader = await connection.ExecuteReaderAsync(command);

        var volunteers = new HashSet<VolunteerDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var phoneNumber = reader.GetString(4);
            var volunteer = volunteers.FirstOrDefault(v => v.PhoneNumber == phoneNumber);

            if (volunteer == null)
            {
                var fullNameDto = new FullNameDto
                {
                    Name = reader.GetString(0),
                    Surname = reader.GetString(1),
                    Patronymic = reader.GetString(2)
                };
                    
                var requisitesJson = reader.GetString(6);
                var socialLinksJson = reader.GetString(7);

                var requisites = JsonSerializer.Deserialize<RequisitesListDto>(requisitesJson)
                                 ?? new RequisitesListDto() { Requisites = [] };
                var socialLinks = JsonSerializer.Deserialize<SocialLinksListDto>(socialLinksJson)
                                  ?? new SocialLinksListDto() { SocialLinks = [] };

                volunteer = new VolunteerDto
                {
                    FullName = fullNameDto,
                    GeneralDescription = reader.GetString(3),
                    PhoneNumber = phoneNumber,
                    AgeExperience = reader.GetInt32(5),
                    Requisites = requisites.Requisites.ToArray(),
                    SocialLinks = socialLinks.SocialLinks.ToArray(),
                };
                volunteers.Add(volunteer);
            }

            var country = reader.IsDBNull(13) ? null : reader.GetString(13);
            if (country is null) continue;
            var addressDto = new AddressDto
            {
                Country = reader.GetString(13),
                City = reader.GetString(14),
                Street = reader.GetString(15),
                House = reader.GetString(16),
                Flat = reader.GetString(17)
            };


            var pet = new PetDto
            {
                PetName = reader.GetString(8),
                GeneralDescription = reader.GetString(9),
                HealthInformation = reader.GetString(10),
                SpeciesName = reader.GetString(11),
                BreedName = reader.GetString(12),
                Address = addressDto,
                Weight = reader.GetDouble(18),
                Height = reader.GetDouble(19),
                PhoneNumber = phoneNumber,
                BirthDate = reader.GetDateTime(20),
                IsCastrated = reader.GetBoolean(21),
                IsVaccinated = reader.GetBoolean(22),
                HelpStatus = (HelpStatus)reader.GetInt32(23)
            };

            volunteer.AddPet(pet);
        }

        return volunteers.ToArray();
    }
}
