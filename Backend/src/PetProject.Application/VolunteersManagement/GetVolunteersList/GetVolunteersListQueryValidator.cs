using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.GetVolunteersList;

public class GetVolunteersListQueryValidator : AbstractValidator<GetVolunteersListQuery>
{
    public GetVolunteersListQueryValidator()
    {
        RuleFor(g => g.Limit)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Limit"));
        
        RuleFor(g => g.Offset)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Offset"));
    }
}