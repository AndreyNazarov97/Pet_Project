using Microsoft.Extensions.Configuration;
using Npgsql;
using PetProject.Core.Database;

namespace PetProject.VolunteerManagement.Infrastructure.Common;

public class PostgresConnectionFactory : IPostgresConnectionFactory
{
    private readonly string _connectionString;

    public PostgresConnectionFactory(
        string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}