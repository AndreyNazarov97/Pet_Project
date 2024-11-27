using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Core.Dtos.Validators;

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(c => new { c.Country, c.City, c.Street, c.House, c.Flat })
            .MustBeValueObject(x => Address.Create(
                x.Country,
                x.City, 
                x.Street,
                x.House,
                x.Flat));
    }
}