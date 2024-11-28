using CSharpFunctionalExtensions;
using Moq;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.VolunteerManagement.Application.Repository;
using PetProject.VolunteerManagement.Domain.Aggregate;

namespace PetProject.Application.Tests.Extensions;

public static class VolunteerRepositoryExtensions
{
    public static void SetupGetById(
        this Mock<IVolunteersRepository> mock,
        VolunteerId volunteerId,
        Result<Volunteer, Error> result)
    {
        mock.Setup(vr => vr.GetById(
                volunteerId,
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(result);
    }
    
    public static Mock<IReadRepository> SetupQueryVolunteer(
        this Mock<IReadRepository> mock,
        VolunteerDto[] result)
    {
        mock.Setup(vr => vr.QueryVolunteers(
                It.IsAny<VolunteerQueryModel>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(result);
        
        return mock;
    }
}