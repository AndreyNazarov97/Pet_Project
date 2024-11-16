using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Dto.Validators;

public class RequisiteDtoValidator : AbstractValidator<RequisiteDto>
{
    public RequisiteDtoValidator()
    {
        RuleFor(c => new {c.Description, c.Name})
            .MustBeValueObject(s => Requisite.Create(s.Name, s.Description));
    }
}