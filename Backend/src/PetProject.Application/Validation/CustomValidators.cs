using FluentValidation;
using PetProject.Domain.Shared;

namespace PetProject.Application.Validation;

public static class CustomValidators
{
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject>> factory)
    {
        return ruleBuilder
            .Custom((value, context) =>
            {
                Result<TValueObject> result = factory(value);
                if (result.IsSuccess)
                    return;

                context.AddFailure(result.Error.Serialize());
            });
    }
}