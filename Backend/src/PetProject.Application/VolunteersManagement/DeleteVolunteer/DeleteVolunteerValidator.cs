using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.DeleteVolunteer;

public class DeleteVolunteerValidator : AbstractValidator<DeleteVolunteerRequest>
{
    public DeleteVolunteerValidator()
    {
        RuleFor(d => d.Id)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired("Id"));
    }
}