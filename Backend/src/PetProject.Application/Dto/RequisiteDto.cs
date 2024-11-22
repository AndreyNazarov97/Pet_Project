using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Dto;

public record RequisiteDto(string Title, string Description)
{
    public Requisite ToEntity()
    {
        var requisite = Requisite.Create(Title, Description).Value;
        
        return requisite;
    }
}