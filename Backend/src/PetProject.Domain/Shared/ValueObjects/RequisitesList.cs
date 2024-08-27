namespace PetProject.Domain.Shared.ValueObjects;

public record RequisitesList 
{
    private RequisitesList(){}

    public RequisitesList(IEnumerable<Requisite> requisites)
    {
        Requisites = requisites.ToList();
    }

    public IReadOnlyList<Requisite> Requisites { get; }
}