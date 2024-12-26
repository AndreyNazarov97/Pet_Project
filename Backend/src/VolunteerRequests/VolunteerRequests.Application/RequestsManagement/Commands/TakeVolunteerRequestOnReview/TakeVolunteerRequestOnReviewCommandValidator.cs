using FluentValidation;

namespace VolunteerRequests.Application.RequestsManagement.Commands.TakeVolunteerRequestOnReview;

public class TakeVolunteerRequestOnReviewCommandValidator : AbstractValidator<TakeVolunteerRequestOnReviewCommand>
{
    public TakeVolunteerRequestOnReviewCommandValidator()
    {
        RuleFor(x => x.VolunteerRequestId)
            .NotEmpty()
            .WithMessage("Volunteer request id is required");
        
        RuleFor(x => x.AdminId)
            .GreaterThan(0)
            .WithMessage("Admin id must be greater than 0");
    }
}