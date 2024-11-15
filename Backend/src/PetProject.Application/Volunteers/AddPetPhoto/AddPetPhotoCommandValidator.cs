using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.Volunteers.AddPetPhoto;

public class AddPetPhotoCommandValidator : AbstractValidator<AddPetPhotoCommand>
{
    public AddPetPhotoCommandValidator()
    {
        RuleFor(p => p.PetId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("PetId"));
        
        RuleFor(p => p.VolunteerId)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("VolunteerId"));
        
        RuleForEach(p => p.Photos)
            .ChildRules(p =>
            {
                p.RuleFor(p => p.FileName)
                    .Must(ext => Constants.Extensions.Contains(Path.GetExtension(ext)))
                    .NotEmpty().WithError(Errors.General.ValueIsInvalid("FileName"));

                p.RuleFor(p => p.Content)
                    .Must(s => s.Length is > 0 and <= 15 * 1024 * 1024).WithError(Errors.General.ValueIsInvalid("Content"));
            });

    }
}