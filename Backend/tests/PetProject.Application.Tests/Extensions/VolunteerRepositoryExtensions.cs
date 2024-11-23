using CSharpFunctionalExtensions;
using Moq;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Application.VolunteersManagement;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement;

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
    
    public static Mock<IVolunteersRepository> SetupQuery(
        this Mock<IVolunteersRepository> mock,
        VolunteerDto[] result)
    {
        mock.Setup(vr => vr.Query(
                It.IsAny<VolunteerQueryModel>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(result);
        
        return mock;
    }
}