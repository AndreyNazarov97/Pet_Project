using PetProject.Domain.Entities.ValueObjects;
using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities;

public class PetDetails : Entity<Guid>
{
    private readonly List<Requisite> _requisites = [];

    private PetDetails(){}
    private PetDetails(
        List<Requisite> requisites)
    {
        _requisites = requisites;
    }
    
    
    public IReadOnlyCollection<Requisite> Requisites => _requisites;

    
    public void AddRequisites(List<Requisite> requisites) => _requisites.AddRange(requisites);
    
    
    public static Result<PetDetails> Create(
        List<Requisite> requisites)
    {
        var details = new PetDetails(
            requisites);
        return Result<PetDetails>.Success(details);
    }
}