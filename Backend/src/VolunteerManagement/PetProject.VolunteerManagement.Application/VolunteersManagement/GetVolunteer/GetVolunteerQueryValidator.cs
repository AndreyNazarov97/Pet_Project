using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.GetVolunteer;

public class GetVolunteerQueryValidator : AbstractValidator<GetVolunteerQuery>
{
    public GetVolunteerQueryValidator()
    {
        RuleFor(g => g.VolunteerId)
            .NotEmpty().WithError(Errors.General.LengthIsInvalid("VolunteerId"));
    }
}