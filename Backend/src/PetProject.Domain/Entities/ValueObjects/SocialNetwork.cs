﻿using PetProject.Domain.Shared;

namespace PetProject.Domain.Entities.ValueObjects;

public class SocialNetwork : ValueObject
{
    public string Title { get; }
    public string Link { get; }
    
    private SocialNetwork(){}

    private SocialNetwork(string title, string link)
    {
        Title = title;
        Link = link;
    }

    public static Result<SocialNetwork> Create(string title, string link)
    {
        if(string.IsNullOrWhiteSpace(title) || title.Length > Constants.MAX_SHORT_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(title));
        if (string.IsNullOrWhiteSpace(link) || link.Length > Constants.MAX_LONG_TEXT_LENGTH)
            return Errors.General.ValueIsRequired(nameof(link));
        
        var socialNetwork = new SocialNetwork(title, link);
        return Result<SocialNetwork>.Success(socialNetwork);
    }
    
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Title;
        yield return Link;
    }
}