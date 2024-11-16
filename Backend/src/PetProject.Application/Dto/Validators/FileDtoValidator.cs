using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.Dto.Validators;

public class FileDtoValidator : AbstractValidator<FileDto>
{
    public FileDtoValidator()
    {
        RuleFor(p => p.FileName)
            .Must(ext => Constants.Extensions.Contains(Path.GetExtension(ext)))
            .NotEmpty().WithError(Errors.General.ValueIsInvalid("FileName"));

        RuleFor(p => p.Content)
            .Must(s => s.Length is > 0 and <= 15 * 1024 * 1024)
            .WithError(Errors.General.ValueIsInvalid("Content"));
    }
}