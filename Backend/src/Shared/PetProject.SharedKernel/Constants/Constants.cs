﻿namespace PetProject.SharedKernel.Constants;

public class Constants
{
    public const int MIN_TEXT_LENGTH = 50;
    public const int MIDDLE_TEXT_LENGTH = 100;
    public const int MAX_TEXT_LENGTH = 500;
    public const int EXTRA_TEXT_LENGTH = 2000;
    
    public static string[] PhotoExtensions = [".jpg", ".png", ".jpeg"];
    
    public enum Context
    {
        VolunteerManagement,
        SpeciesManagement,
        Accounts,
        VolunteerRequests,
        Discussions
    }
}