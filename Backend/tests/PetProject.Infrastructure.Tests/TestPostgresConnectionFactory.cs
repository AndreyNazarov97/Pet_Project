using Npgsql;
using PetProject.Infrastructure.Postgres.Abstractions;
using Testcontainers.PostgreSql;

namespace PetProject.Infrastructure.Tests;

public class TestPostgresConnectionFactory : IPostgresConnectionFactory
{
    private readonly PostgreSqlContainer _postgresSqlContainer;

    public TestPostgresConnectionFactory(PostgreSqlContainer postgresSqlContainer)
    {
        _postgresSqlContainer = postgresSqlContainer;
    }
    
    public NpgsqlConnection GetConnection()
    {
        var connectionString = _postgresSqlContainer.GetConnectionString();
        return new NpgsqlConnection(connectionString);
    }
}