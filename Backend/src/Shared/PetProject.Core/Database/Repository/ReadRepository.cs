using System.Text.Json;
using Dapper;
using PetProject.Core.Database.Models;
using PetProject.Core.Dtos;

namespace PetProject.Core.Database.Repository;

public class ReadRepository : IReadRepository
{
    private readonly IPostgresConnectionFactory _connectionFactory;

    public ReadRepository(IPostgresConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<SpeciesDto[]> QuerySpecies(SpeciesQueryModel query, CancellationToken cancellationToken = default)
    {
        var sqlQuery = """
                       select 
                           s.id,
                           s.species_name,
                           b.id,
                           b.breed_name
                       from 
                           species.species as s 
                       left join 
                           species.breeds as b on b.species_id = s.id AND b.is_deleted = false
                       """;

        var conditions = new List<string>(["s.is_deleted = false"]);
        var param = new DynamicParameters();

        if (string.IsNullOrEmpty(query.SpeciesName) == false)
        {
            conditions.Add("s.species_name = @SpeciesName");
            param.Add("SpeciesName", query.SpeciesName);
        }

        if (string.IsNullOrEmpty(query.BreedName) == false)
        {
            conditions.Add("b.breed_name = @BreedName");
            param.Add("BreedName", query.BreedName);
        }

        if (query.SpeciesIds is { Length: > 0 })
        {
            conditions.Add("s.id = any(@SpeciesIds)");
            param.Add("SpeciesIds", query.SpeciesIds);
        }

        if (query.BreedIds is { Length: > 0 })
        {
            conditions.Add("b.id = any(@BreedIds)");
            param.Add("BreedIds", query.BreedIds);
        }

        sqlQuery += " where " + string.Join(" and ", conditions);
        
        // Сортировка
        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var sortField = query.SortBy switch
            {
                "SpeciesName" => "s.species_name",
                "BreedName" => "b.breed_name",
                _ => "s.species_name"
            };
            sqlQuery += $" ORDER BY {sortField} {(query.SortDescending ? "DESC" : "ASC")}";
        }

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

        var speciesList = new List<SpeciesDto>();
        while (await reader.ReadAsync(cancellationToken))
        {
            var speciesId = reader.GetGuid(0);
            var speciesName = reader.GetString(1);


            var species = speciesList.FirstOrDefault(s => s.Name == speciesName);
            if (species == null)
            {
                species = new SpeciesDto { Id = speciesId, Name = speciesName, Breeds = [] };

                speciesList.Add(species);
            }

            if (reader.IsDBNull(2))
                continue;
            var breedId = reader.GetGuid(2);
            var breedName = reader.GetString(3);

            var breedDto = new BreedDto { Id = breedId, Name = breedName };

            species.Breeds.Add(breedDto);
        }

        return speciesList.ToArray();
    }

    public async Task<VolunteerDto[]> QueryVolunteers(VolunteerQueryModel query, CancellationToken cancellationToken = default)
    {
        var sqlQuery = """
                        SELECT 
                            v.name, 
                            v.surname, 
                            v.patronymic, 
                            v.general_description, 
                            v.phone_number, 
                            v.age_experience,
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
                            volunteers.volunteers v
                        left join 
                            volunteers.pets p on v.id = p.volunteer_id AND p.is_deleted = false
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
        
        // Сортировка
        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var sortField = query.SortBy switch
            {
                "Name" => "v.surname",
                "Age" => "v.age_experience",
                "PhoneNumber" => "v.phone_number",
                _ => "v.surname"
            };
            sqlQuery += $" ORDER BY {sortField} {(query.SortDescending ? "DESC" : "ASC")}";
        }

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

                volunteer = new VolunteerDto
                {
                    FullName = fullNameDto,
                    GeneralDescription = reader.GetString(3),
                    PhoneNumber = phoneNumber,
                    AgeExperience = reader.GetInt32(5)
                };
                volunteers.Add(volunteer);
            }

            var country = reader.IsDBNull(11) ? null : reader.GetString(11);
            if (country is null) continue;
            var addressDto = new AddressDto
            {
                Country = reader.GetString(11),
                City = reader.GetString(12),
                Street = reader.GetString(13),
                House = reader.GetString(14),
                Flat = reader.GetString(15)
            };


            var pet = new PetDto
            {
                PetName = reader.GetString(6),
                GeneralDescription = reader.GetString(7),
                HealthInformation = reader.GetString(8),
                SpeciesName = reader.GetString(9),
                BreedName = reader.GetString(10),
                Address = addressDto,
                Weight = reader.GetDouble(16),
                Height = reader.GetDouble(17),
                PhoneNumber = phoneNumber,
                BirthDate = reader.GetDateTime(18),
                IsCastrated = reader.GetBoolean(19),
                IsVaccinated = reader.GetBoolean(20),
                HelpStatus = reader.GetInt32(21)
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
                           volunteers.pets AS p
                       LEFT JOIN 
                           volunteers.volunteers AS v ON v.id = p.volunteer_id
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
                HelpStatus = reader.GetString(15),
                PhoneNumber = reader.GetString(16)
            };

            pets.Add(pet);
        }

        return pets.ToArray();
    }
}