using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.GetPetById;

public class GetPetByIdQueryValidator : AbstractValidator<GetPetByIdQuery>
{
    public GetPetByIdQueryValidator()
    {
        RuleFor(g => g.PetId)
            .NotEmpty().WithError(Errors.General.LengthIsInvalid("PetId"));
    }
}