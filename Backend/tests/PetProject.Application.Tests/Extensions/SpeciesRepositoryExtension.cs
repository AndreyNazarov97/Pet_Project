using CSharpFunctionalExtensions;
using Moq;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Application.SpeciesManagement;
using PetProject.Application.VolunteersManagement;

namespace PetProject.Application.Tests.Extensions;

public static class SpeciesRepositoryExtension
{
    public static void SetupQuery(
        this Mock<ISpeciesRepository> mock,
        SpeciesDto[] speciesDto)
    {
        mock.Setup(sr => sr.Query(
                It.IsAny<SpeciesQueryModel>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(speciesDto);
    }
}