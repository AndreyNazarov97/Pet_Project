﻿namespace PetProject.Application.Dto;

public record UpdateVolunteerDto(
    FullNameDto FullName,
    string Description,
    int AgeExperience,
    string PhoneNumber);