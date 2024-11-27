using FluentValidation;
using PetProject.Core.Validation;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;

namespace PetProject.Core.Dtos.Validators;

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