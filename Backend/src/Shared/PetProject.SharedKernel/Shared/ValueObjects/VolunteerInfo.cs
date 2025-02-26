﻿namespace PetProject.SharedKernel.Shared.ValueObjects;

public record VolunteerInfo 
{
    public FullName FullName { get; }
    public PhoneNumber PhoneNumber { get; }
    public Experience WorkExperience { get; }
    public Description GeneralDescription { get; }
    public IReadOnlyList<SocialNetwork> SocialNetworks { get; }
    
    private VolunteerInfo(){}
    
    public VolunteerInfo(
        FullName fullName,
        PhoneNumber phoneNumber, 
        Experience workExperience, 
        Description generalDescription,
        IEnumerable<SocialNetwork> socialNetworks)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        WorkExperience = workExperience;
        GeneralDescription = generalDescription;
        SocialNetworks = socialNetworks.ToList();
    }
}