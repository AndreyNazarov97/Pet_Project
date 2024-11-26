using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Dto.Validators;

public class PetPhotoDtoValidator : AbstractValidator<PetPhotoDto>
{
    public PetPhotoDtoValidator()
    {
        RuleFor(p => p.Path)
            .MustBeValueObject(FilePath.Create);
    }
}