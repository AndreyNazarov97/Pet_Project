using PetProject.Domain.Shared.ValueObjects;

namespace PetProject.Application.Dto;

public record FullNameDto(string Name, string Surname, string? Patronymic)
{
    public FullName ToEntity()
    {
        return FullName.Create(Name, Surname, Patronymic).Value;
    }
}