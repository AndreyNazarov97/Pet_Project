using Dapper;
using Microsoft.Extensions.Options;
using PetProject.Core.Database;
using PetProject.Core.Options;
using PetProject.VolunteerManagement.Domain.Aggregate;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;

namespace PetProject.VolunteerManagement.Infrastructure.Services;

public class DeleteExpiredPetsService
{
    private readonly IPostgresConnectionFactory _connectionFactory;
    private readonly EntityRetentionOptions _entityRetentionOptions;

    public DeleteExpiredPetsService(
        IPostgresConnectionFactory connectionFactory,
        IOptions<EntityRetentionOptions> options)
    {
        _connectionFactory = connectionFactory;
        _entityRetentionOptions = options.Value;
    }

    public async Task DeleteExpiredPets()
    {
        var sqlQuery = """
                       DELETE
                       FROM volunteers.pets p
                       WHERE deletion_date < now() - make_interval(days => @RetentionDays);
                       """;
        
        var param = new DynamicParameters();
        
        param.Add("RetentionDays", _entityRetentionOptions.Days);

        var connection = _connectionFactory.GetConnection();

        await connection.ExecuteAsync(sqlQuery, param);
            
    }
}