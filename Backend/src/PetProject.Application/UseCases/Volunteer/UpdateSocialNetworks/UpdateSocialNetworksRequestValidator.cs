using FluentValidation;
using PetProject.Application.Validation;
using PetProject.Domain.PetManagement.Entities.ValueObjects;

namespace PetProject.Application.UseCases.Volunteer.UpdateSocialNetworks;

public class UpdateSocialNetworksRequestValidator : AbstractValidator<UpdateSocialNetworksRequest>
{
    public UpdateSocialNetworksRequestValidator()
    {
        RuleForEach(c => c.Dto.SocialNetworks)
            .MustBeValueObject(s => SocialNetwork.Create(s.Title, s.Link));
    }
}