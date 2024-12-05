using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Core.ObjectMappers;

public static class DtoMapper
{
    public static FullName ToEntity(this FullNameDto dto) => FullName.Create(dto.Name, dto.Surname, dto.Patronymic).Value;
}