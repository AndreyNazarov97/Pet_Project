using CSharpFunctionalExtensions;
using PetProject.Domain.Shared;

namespace PetProject.Domain.VolunteerManagement.ValueObjects;

public record SocialLink
{
    public string Title { get; }
    public string Url { get; }

    private SocialLink(string title, string url)
    {
        Title = title;
        Url = url;
    }

    public static Result<SocialLink, Error> Create(string title, string url)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > Constants.MAX_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Name");

        if (string.IsNullOrWhiteSpace(url) || url.Length > Constants.EXTRA_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Path");

        return new SocialLink(title, url);
    }
}