using System.Data;
using System.Text.Json;
using Dapper;

namespace PetProject.Core.Database.Dapper;

public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
{
    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.Value = JsonSerializer.Serialize(value, JsonSerializerOptions.Default);
    }

    public override T? Parse(object value)
    {
        return JsonSerializer.Deserialize<T>(value as string ?? string.Empty, JsonSerializerOptions.Default);
    }
}