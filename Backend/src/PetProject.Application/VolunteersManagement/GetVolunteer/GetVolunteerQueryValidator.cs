using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.GetVolunteer;

public class GetVolunteerQueryValidator : AbstractValidator<GetVolunteerQuery>
{
    public GetVolunteerQueryValidator()
    {
        RuleFor(g => g.VolunteerId)
            .NotEmpty().WithError(Errors.General.LengthIsInvalid("VolunteerId"));
    }
}