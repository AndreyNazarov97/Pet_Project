using System.Collections;
using System.Text.Json.Serialization;

namespace PetProject.Domain.Shared.ValueObjects;

public class ValueObjectList<T> : IReadOnlyList<T>
{
    public IReadOnlyList<T> Values { get; } = null!;
    public T this[int index] => Values[index];

    [JsonIgnore]
    public int Count => Values.Count;
    
    public string TypeName => typeof(T).Name;
    
    private ValueObjectList(){}
    
    [JsonConstructor]

    public ValueObjectList(IEnumerable<T> values)
    {
        Values = new List<T>(values).AsReadOnly();
    }
    
    public IEnumerator<T> GetEnumerator()
        => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Values.GetEnumerator();
    
    public static implicit operator ValueObjectList<T>(List<T> list)
        => new(list);

    public static implicit operator List<T>(ValueObjectList<T> list)
        => list.Values.ToList();
}