using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace PetProject.Discussions.Application.DiscussionsManagement.Queries.GetByRelationId;

public class GetByRelationIdQueryValidator : AbstractValidator<GetByRelationIdQuery>
{
    public GetByRelationIdQueryValidator()
    {
        RuleFor(x => x.RelationId)
            .NotNull()
            .WithError(Errors.General.Null());
    }
}