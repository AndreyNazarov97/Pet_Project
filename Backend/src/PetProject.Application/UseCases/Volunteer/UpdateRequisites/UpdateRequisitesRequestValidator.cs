using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.UseCases.Volunteer.UpdateRequisites;

public class UpdateRequisitesRequestValidator : AbstractValidator<UpdateRequisitesRequest>
{
    public UpdateRequisitesRequestValidator()
    {
        RuleForEach(c => c.Dto.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Title, r.Description));
    }
}