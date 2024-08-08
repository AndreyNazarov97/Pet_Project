﻿namespace PetProject.Domain.Entities;

public class SocialNetwork
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Link { get; private set; }
    private SocialNetwork()
    {
        
    }
}