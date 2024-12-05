﻿using CSharpFunctionalExtensions;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Infrastructure.IdentityManagers;

public class AccountManager : IAccountManager
{
    private readonly AccountsDbContext _context;

    public AccountManager(AccountsDbContext context)
    {
        _context = context;
    }

    public async Task<UnitResult<ErrorList>> CreateAdminAccount(AdminAccount adminAccount)
    {
        try
        {
            await _context.AdminAccounts.AddAsync(adminAccount);
            await _context.SaveChangesAsync();
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception e)
        {
            return Error.Failure("could.not.create.account", e.Message).ToErrorList();
        }
    }

    public async Task<UnitResult<ErrorList>> CreateParticipantAccount(ParticipantAccount participantAccount,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.ParticipantAccounts.AddAsync(participantAccount, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception e)
        {
            return Error.Failure("could.not.create.account", e.Message).ToErrorList();
        }
    }

    public async Task<UnitResult<ErrorList>> CreateVolunteerAccount(VolunteerAccount volunteerAccount,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await _context.VolunteerAccounts.AddAsync(volunteerAccount, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception e)
        {
            return Error.Failure("could.not.create.account", e.Message).ToErrorList();
        }
    }
}