using FluentValidation;
using PetProject.Core.Dtos.Validators;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace VolunteerRequests.Application.RequestsManagement.Commands.UpdateVolunteerRequest;

public class UpdateVolunteerRequestCommandValidator : AbstractValidator<UpdateVolunteerRequestCommand>
{
    public UpdateVolunteerRequestCommandValidator()
    {
        RuleFor(x => x.VolunteerRequestId)
            .NotEmpty()
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("Admin id must be greater than 0");
        
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