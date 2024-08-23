using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.UseCases.UpdateVolunteer;

public class UpdateRequisitesRequestValidator : AbstractValidator<UpdateRequisitesRequest>
{
    public UpdateRequisitesRequestValidator()
    {
        RuleForEach(c => c.Requisites)
            .MustBeValueObject(r => Requisite.Create(r.Title, r.Description));
        
    }
}