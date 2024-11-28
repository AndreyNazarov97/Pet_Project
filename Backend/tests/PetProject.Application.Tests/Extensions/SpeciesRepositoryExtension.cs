using Moq;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.SpeciesManagement.Application.Repository;

namespace PetProject.Application.Tests.Extensions;

public static class SpeciesRepositoryExtension
{
    public static void SetupQuerySpecies(
        this Mock<IReadRepository> mock,
        SpeciesDto[] speciesDto)
    {
        mock.Setup(sr => sr.QuerySpecies(
                It.IsAny<SpeciesQueryModel>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(speciesDto);
    }
}