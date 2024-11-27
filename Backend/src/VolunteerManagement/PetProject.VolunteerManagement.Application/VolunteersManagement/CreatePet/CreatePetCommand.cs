using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.VolunteerManagement.Domain.Enums;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.CreatePet;

public record CreatePetCommand : IRequest<Result<Guid, ErrorList>>
{
    public required Guid VolunteerId { get; init; }
    public required string Name { get; init; }
    public required string GeneralDescription { get; init; }
    public required string HealthInformation { get; init; }
    public required string SpeciesName { get; init; }
    public required string BreedName { get; init; }
    public required AddressDto Address { get; init; }
    public required double Weight { get; init; }
    public required double Height { get; init; }
    public required DateOnly BirthDate { get; init; }
    public required bool IsCastrated { get; init; }
    public required bool IsVaccinated { get; init; }
    public required HelpStatus HelpStatus { get; init; }
}


