using Npgsql;

namespace PetProject.Infrastructure.Postgres.Abstractions;

public interface IPostgresConnectionFactory
{
    NpgsqlConnection GetConnection();
}