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

        if (query.SpeciesNames is { Length: > 0 })
        {
            conditions.Add("p.species_name = any(@SpeciesNames)");
            param.Add("SpeciesNames", query.SpeciesNames);
        }

        if (query.BreedNames is { Length: > 0 })
        {
            conditions.Add("p.breed_name = any(@BreedNames)");
            param.Add("BreedNames", query.BreedNames);
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
                    Patronymic = reader.IsDBNull(2) ? null : reader.GetString(2)
                };

                var requisitesJson = reader.GetString(6);
                var socialLinksJson = reader.GetString(7);

                var requisites = JsonSerializer.Deserialize<IEnumerable<RequisiteDto>>(requisitesJson)
                                 ?? [];
                var socialLinks = JsonSerializer.Deserialize<IEnumerable<SocialLinkDto>>(socialLinksJson)
                                  ?? [];

                volunteer = new VolunteerDto
                {
                    FullName = fullNameDto,
                    GeneralDescription = reader.GetString(3),
                    PhoneNumber = phoneNumber,
                    AgeExperience = reader.GetInt32(5),
                    Requisites = requisites.ToArray(),
                    SocialLinks = socialLinks.ToArray(),
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

    public async Task<PetDto[]> QueryPets(PetQueryModel query, CancellationToken cancellationToken = default)
    {
        var sqlQuery = """
                       SELECT 
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
                            p.help_status,
                            v.phone_number
                       FROM 
                           pets AS p
                       LEFT JOIN 
                           volunteers AS v ON v.id = p.volunteer_id
                       """;

        var conditions = new List<string>(["p.is_deleted = false"]);
        var parameters = new DynamicParameters();

        // Фильтрация
        if (query.PetId is not null)
        {
            conditions.Add("p.id = @PetId");
            parameters.Add("PetId", query.PetId);
        }
        
        if (query.VolunteerId is not null)
        {
            conditions.Add("p.volunteer_id = ANY(@VolunteerId)");
            parameters.Add("VolunteerId", query.VolunteerId);
        }

        if (!string.IsNullOrEmpty(query.Name))
        {
            conditions.Add("p.pet_name ILIKE @Name");
            parameters.Add("Name", $"%{query.Name}%");
        }

        if (query.MinAge.HasValue)
        {
            conditions.Add("EXTRACT(YEAR FROM AGE(CURRENT_DATE, p.birth_date)) >= @MinAge");
            parameters.Add("MinAge", query.MinAge);
        }

        if (!string.IsNullOrEmpty(query.Breed))
        {
            conditions.Add("p.breed_name ILIKE @Breed");
            parameters.Add("Breed", $"%{query.Breed}%");
        }

        if (!string.IsNullOrEmpty(query.Species))
        {
            conditions.Add("p.species_name ILIKE @Species");
            parameters.Add("Species", $"%{query.Species}%");
        }

        if (query.HelpStatus.HasValue)
        {
            conditions.Add("p.help_status = @HelpStatus");
            parameters.Add("HelpStatus", query.HelpStatus);
        }

        sqlQuery += " WHERE " + string.Join(" AND ", conditions);

        // Сортировка
        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var sortField = query.SortBy switch
            {
                "Name" => "p.pet_name",
                "MinAge" => "p.birth_date",
                "Breed" => "p.breed_name",
                "Species" => "p.species_name",
                "HelpStatus" => "p.help_status",
                "Volunteer" => "p.volunteer_id",
                _ => "p.pet_name"
            };
            sqlQuery += $" ORDER BY {sortField} {(query.SortDescending ? "DESC" : "ASC")}";
        }

        // Пагинация
        if (query.Limit > 0)
        {
            sqlQuery += " limit @Limit ";
            parameters.Add("Limit", query.Limit);
        }

        if (query.Offset > 0)
        {
            sqlQuery += " offset @Offset ";
            parameters.Add("Offset", query.Offset);
        }

        var command = new CommandDefinition(sqlQuery, parameters, cancellationToken: cancellationToken);

        await using var connection = _connectionFactory.GetConnection();

        await using var reader = await connection.ExecuteReaderAsync(command);

        var pets = new List<PetDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var addressDto = new AddressDto
            {
                Country = reader.GetString(5),
                City = reader.GetString(6),
                Street = reader.GetString(7),
                House = reader.GetString(8),
                Flat = reader.GetString(9)
            };


            var pet = new PetDto
            {
                PetName = reader.GetString(0),
                GeneralDescription = reader.GetString(1),
                HealthInformation = reader.GetString(2),
                SpeciesName = reader.GetString(3),
                BreedName = reader.GetString(4),
                Address = addressDto,
                Weight = reader.GetDouble(10),
                Height = reader.GetDouble(11),
                BirthDate = reader.GetDateTime(12),
                IsCastrated = reader.GetBoolean(13),
                IsVaccinated = reader.GetBoolean(14),
                HelpStatus = (HelpStatus)reader.GetInt32(15),
                PhoneNumber = reader.GetString(16)
            };

            pets.Add(pet);
        }
        
        return pets.ToArray();
    }
}