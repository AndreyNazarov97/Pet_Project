using Microsoft.Extensions.Configuration;
using Npgsql;
using PetProject.Infrastructure.Postgres.Abstractions;

namespace PetProject.Infrastructure.Postgres.Common;

public class PostgresConnectionFactory : IPostgresConnectionFactory
{
    private readonly IConfiguration _configuration;

    public PostgresConnectionFactory(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public NpgsqlConnection GetConnection()
    {
        var connectionString = _configuration.GetConnectionString("Postgres");
        return new NpgsqlConnection(connectionString);
    }
}