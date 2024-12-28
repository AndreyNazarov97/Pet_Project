using PetProject.Core.Dtos.Accounts;

namespace PetProject.Accounts.Application.Interfaces;

public interface IReadDbContext
{
    IQueryable<UserDto> Users { get; }

    IQueryable<RoleDto> Roles { get; }

    IQueryable<AdminAccountDto> AdminAccounts { get; }

    IQueryable<ParticipantAccountDto> ParticipantAccounts { get; }

    IQueryable<VolunteerAccountDto> VolunteerAccounts { get; }
}