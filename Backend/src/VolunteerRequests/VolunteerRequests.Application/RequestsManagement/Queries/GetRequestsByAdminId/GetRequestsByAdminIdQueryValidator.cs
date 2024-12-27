using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace VolunteerRequests.Application.RequestsManagement.Queries.GetRequestsByAdminId;

public class GetRequestsByAdminIdQueryValidator : AbstractValidator<GetRequestsByAdminIdQuery>
{
    public GetRequestsByAdminIdQueryValidator()
    {
        var allowedSortFields = new[] { "RequestStatus", "UserId", "CreatedAt", "PhoneNumber" };
        
        RuleFor(x => x.AdminId)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid());
        
        RuleFor(x => x.SortBy)
            .Must(sortBy => string.IsNullOrEmpty(sortBy) || allowedSortFields.Contains(sortBy))
            .WithError(Errors.General.ValueIsInvalid("SortBy"));
        
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Limit"));
        
        RuleFor(x => x.Offset)
            .GreaterThanOrEqualTo(0).WithError(Errors.General.ValueIsInvalid("Offset"));
    }
}