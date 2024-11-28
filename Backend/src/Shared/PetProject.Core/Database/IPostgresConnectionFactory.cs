using Npgsql;

namespace PetProject.Core.Database;

public interface IPostgresConnectionFactory
{
    NpgsqlConnection GetConnection();
}