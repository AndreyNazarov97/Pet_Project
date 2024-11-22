using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.GetListVolunteers;

public class GetListVolunteersQueryValidator : AbstractValidator<GetListVolunteersQuery>
{
    public GetListVolunteersQueryValidator()
    {
        RuleFor(g => g.Limit)
            .GreaterThan(0).WithError(Errors.General.ValueIsInvalid("Limit"));
        
        RuleFor(g => g.Offset)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Offset"));
    }
}