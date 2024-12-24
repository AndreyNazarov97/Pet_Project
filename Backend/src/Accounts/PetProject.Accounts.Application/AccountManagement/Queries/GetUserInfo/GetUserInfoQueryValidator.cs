using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Queries.GetUserInfo;

public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
{
    public GetUserInfoQueryValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsInvalid());
    }
}