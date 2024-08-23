namespace PetProject.Application.UseCases.UpdateVolunteer;

public record UpdateMainInfoRequest(
    string FirstName, 
    string LastName, 
    string Patronymic, 
    string Description, 
    string PhoneNumber,
    int Experience);