using FluentValidation;
using PetProject.Core.Dtos.Validators;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace VolunteerRequests.Application.RequestsManagement.Commands.CreateVolunteerRequest;

public class CreateVolunteerRequestCommandValidator : AbstractValidator<CreateVolunteerRequestCommand>
{
    public CreateVolunteerRequestCommandValidator()
    {
        RuleFor(c => c.FullName)
            .SetValidator(new FullNameDtoValidator());
        
        RuleFor(c => c.Description)
            .MustBeValueObject(Description.Create);

        RuleFor(c => c.AgeExperience)
            .MustBeValueObject(Experience.Create);

        RuleFor(c => c.PhoneNumber)
            .MustBeValueObject(PhoneNumber.Create);
        
        RuleForEach(x => x.SocialNetworksDto)
            .SetValidator(new SocialNetworkDtoValidator());
    }
}