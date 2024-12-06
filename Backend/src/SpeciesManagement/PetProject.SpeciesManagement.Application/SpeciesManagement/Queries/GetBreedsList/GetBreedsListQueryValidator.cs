using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace PetProject.SpeciesManagement.Application.SpeciesManagement.Queries.GetBreedsList;

public class GetBreedsListQueryValidator : AbstractValidator<GetBreedsListQuery>
{
    public GetBreedsListQueryValidator()
    {
        RuleFor(g => g.Limit)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Limit"));

        RuleFor(g => g.Offset)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Offset"));
    }
}