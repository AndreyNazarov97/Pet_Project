﻿using CSharpFunctionalExtensions;

namespace PetProject.SharedKernel.Shared.ValueObjects;

public record SocialLink
{
    public string Title { get; }
    public string Url { get; }

    private SocialLink(string title, string url)
    {
        Title = title;
        Url = url;
    }

    public static Result<SocialLink, Error> Create(string name, string url)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length > Constants.Constants.MAX_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Name");

        if (string.IsNullOrWhiteSpace(url) || url.Length > Constants.Constants.EXTRA_TEXT_LENGTH)
            return Errors.General.ValueIsInvalid($"Path");

        return new SocialLink(name, url);
    }
}