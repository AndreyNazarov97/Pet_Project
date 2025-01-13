using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetProject.Accounts.Application.AccountManagement.Commands.UpdateRequisites;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using Random = PetProject.SharedTestData.Creators.Random;

namespace PetProject.IntegrationTests.AccountsManagement;

public class UpdateRequisitesHandlerTest : AccountsManagementTestsBase
{
    private readonly IRequestHandler<UpdateRequisitesCommand, Result<long, ErrorList>> _sut;

    public UpdateRequisitesHandlerTest(AccountsTestsWebFactory factory) : base(factory)
    {
        _sut = _scope.ServiceProvider
            .GetRequiredService<IRequestHandler<UpdateRequisitesCommand, Result<long, ErrorList>>>();
    }

    private static UpdateRequisitesCommand Command => new()
     {
         UserId = Random.Long,
         Requisites = new List<RequisiteDto>
         {
             new(){Title ="Title 1", Description = "Description 1"},
             new(){Title ="Title 2", Description = "Description 2"}
         }
     };
    
     [Fact]
     public async Task Handle_ShouldReturnError_WhenVolunteerNotFound()
     {
         // Arrange
         var command = Command ;
    
         // Act
         var result = await _sut.Handle(command, CancellationToken.None);
    
         // Assert
         result.IsFailure.Should().BeTrue();
         result.Error.Errors.Should().ContainSingle(e => e.Code == "record.not.found");
     }
    
     [Fact]
     public async Task Handle_ShouldUpdateRequisites_WhenVolunteerExists()
     {
         // Arrange
         var volunteerAccount = await SeedVolunteerAccount(CancellationToken.None);
         var command = Command with {UserId = volunteerAccount.User.Id};
    
         // Act
         var result = await _sut.Handle(command, CancellationToken.None);
    
         // Assert
         result.IsSuccess.Should().BeTrue();
         
         var volunteerAccountFromDb = await _accountsDbContext.VolunteerAccounts
             .FirstOrDefaultAsync(x => x.Id == volunteerAccount.Id, CancellationToken.None);
         
         volunteerAccountFromDb.Should().NotBeNull();
         volunteerAccountFromDb!.Requisites.Should().HaveCount(2);
         
         var requisite1 = volunteerAccountFromDb.Requisites.First(x => x.Title == command.Requisites.First().Title);
         requisite1.Title.Should().Be(command.Requisites.First().Title);
         requisite1.Description.Should().Be(command.Requisites.First().Description);
         
         var requisite2 = volunteerAccountFromDb.Requisites.First(x => x.Title == command.Requisites.Last().Title);
         requisite2.Title.Should().Be(command.Requisites.Last().Title);
         requisite2.Description.Should().Be(command.Requisites.Last().Description);
     }
}