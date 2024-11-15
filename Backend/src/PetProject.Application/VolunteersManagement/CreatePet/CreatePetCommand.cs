using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.VolunteerManagement.Enums;

namespace PetProject.Application.VolunteersManagement.CreatePet;

public record CreatePetCommand : IRequest<Result<Guid, Error>>
{
    public required Guid VolunteerId { get; init; }
    public required string Name { get; init; }
    public required string GeneralDescription { get; init; }
    public required string HealthInformation { get; init; }
    public required string Species { get; init; }
    public required string Breed { get; init; }
    public required AddressDto Address { get; init; }
    public required double Weight { get; init; }
    public required double Height { get; init; }
    public required DateOnly BirthDate { get; init; }
    public required bool IsCastrated { get; init; }
    public required bool IsVaccinated { get; init; }
    public required HelpStatus HelpStatus { get; init; }
}


