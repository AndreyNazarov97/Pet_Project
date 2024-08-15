using PetProject.Domain.Enums;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities.ValueObjects;

public class PetDetails : ValueObject
{
    private readonly List<Requisite> _requisites = [];

    private PetDetails(){}
    private PetDetails(
        List<Requisite> requisites)
    {
        _requisites = requisites;
    }


    public string BreedName { get;  }
    public string Color { get; }
    public string HealthInfo { get; }
    public double Weight { get;  }
    public double Height { get;  }
    public DateTimeOffset BirthDate { get; }
    public IReadOnlyCollection<Requisite> Requisites => _requisites;

    
    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);
    
    
    public static Result<PetDetails> Create(
        List<Requisite> requisites)
    {
        var details = new PetDetails(
            requisites);
        return Result<PetDetails>.Success(details);
    }
    
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        
        
        foreach (var requisite in _requisites)
        {
            yield return requisite;
        }
    }
}