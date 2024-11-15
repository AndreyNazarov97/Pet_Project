using FluentValidation;
using PetProject.Application.Dto;
using PetProject.Application.Validation;
using PetProject.Domain.Shared;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.Volunteers.UpdateRequisites;

public class UpdateRequisitesRequestValidator : AbstractValidator<UpdateRequisitesCommand>
{
    public UpdateRequisitesRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithError(Errors.General.LengthIsInvalid("Id"));

    }
}

public class UpdateRequisitesDtoValidator : AbstractValidator<UpdateRequisitesDto>
{
    public UpdateRequisitesDtoValidator()
    {
        RuleForEach(c => c.Requisites)
            .MustBeValueObject(s => Requisite.Create(s.Name, s.Description));
    }
}