namespace PetProject.Application.UseCases.Volunteer.UpdateMainInfo;

public record UpdateMainInfoDto(
    string FirstName, 
    string LastName, 
    string Patronymic, 
    string Description, 
    string PhoneNumber,
    int Experience);