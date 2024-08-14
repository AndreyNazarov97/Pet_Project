using FluentValidation;

namespace PetProject.Application.UseCases.CreateVolunteer;

public class CreateVolunteerRequestValidator : AbstractValidator<CreateVolunteerRequest>
{
    public CreateVolunteerRequestValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Patronymic).MaximumLength(50);
        RuleFor(x => x.Experience).GreaterThan(-1);
        RuleFor(x => x.SocialNetworks).NotEmpty();
        RuleFor(x => x.Requisites).NotEmpty();
        
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^7\d{10}$");
    }
}