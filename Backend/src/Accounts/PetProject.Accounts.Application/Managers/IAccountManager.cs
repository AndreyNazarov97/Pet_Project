using CSharpFunctionalExtensions;
using PetProject.Accounts.Domain;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.Managers;

public interface IAccountManager
{
    Task<UnitResult<ErrorList>>  CreateAdminAccount(AdminAccount adminAccount);
    
    Task<Result<AdminAccount, Error>> GetAdminAccount(long id, CancellationToken cancellationToken = default);

    Task<UnitResult<ErrorList>> CreateParticipantAccount(
        ParticipantAccount participantAccount, CancellationToken cancellationToken = default);
    
    Task<Result<ParticipantAccount, Error>> GetParticipantAccount(
        long id, CancellationToken cancellationToken = default);

    Task<UnitResult<ErrorList>> CreateVolunteerAccount(
        VolunteerAccount volunteerAccount, CancellationToken cancellationToken = default);
    
    Task<Result<VolunteerAccount, Error>> GetVolunteerAccount(
        long id, CancellationToken cancellationToken = default);
}