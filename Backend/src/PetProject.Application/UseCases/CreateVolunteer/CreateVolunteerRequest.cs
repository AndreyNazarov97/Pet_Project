using PetProject.Domain.Dto;
using PetProject.Domain.Entities.ValueObjects;

namespace PetProject.Application.UseCases.CreateVolunteer;

public class CreateVolunteerRequest
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string Patronymic { get; set; }
    
    public string Description { get; set; }
    
    public int Experience { get; set; }
    
    public PhoneNumber PhoneNumber { get; set; }
    
    public List<SocialNetworkDto> SocialNetworks { get; set; }
    
    public List<RequisiteDto> Requisites { get; set; }
}