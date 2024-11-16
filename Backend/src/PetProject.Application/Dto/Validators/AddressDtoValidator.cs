using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Dto.Validators;

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