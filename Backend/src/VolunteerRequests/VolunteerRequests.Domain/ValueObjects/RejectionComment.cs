using CSharpFunctionalExtensions;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;

namespace VolunteerRequests.Domain.ValueObjects;

public record RejectionComment
{
    public string Value { get; }

    private RejectionComment(string value)
    {
        Value = value;
    }

    public static RejectionComment Empty => new("");

    public static Result<RejectionComment, Error> Create(string comment)
    {
        if (string.IsNullOrEmpty(comment) || comment.Length > Constants.EXTRA_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid(nameof(RejectionComment));

        return new RejectionComment(comment);
    }
}