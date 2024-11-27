﻿using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.VolunteerManagement.Enums;

namespace PetProject.Application.VolunteersManagement.GetPets;

public record GetPetsQuery : IRequest<Result<PetDto[], ErrorList>>
{
    public Guid? VolunteerId{ get; init; } 
    public string? Name { get; init; }
    public int? MinAge { get; init; }
    public string? Breed { get; init; }
    public string? Species { get; init; }
    public HelpStatus? HelpStatus { get; init; }
    public string? SortBy { get; init; } 
    public bool SortDescending { get; init; } 
    public int Limit { get; init; } 
    public int Offset { get; init; }
}