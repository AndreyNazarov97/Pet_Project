using System.Text.Json;
using CSharpFunctionalExtensions;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PetProject.Accounts.Application.Interfaces;
using PetProject.Core.Database;
using PetProject.Core.Dtos;
using PetProject.Core.Dtos.Accounts;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Queries.GetUserInfo;

public class GetUserInfoHandler : IRequestHandler<GetUserInfoQuery, Result<UserDto, ErrorList>>
{
    private readonly IReadDbContext _readDbContext;

    public GetUserInfoHandler(
        IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }


    public async Task<Result<UserDto, ErrorList>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
    {
        var user = await GetUserById(request.UserId, cancellationToken);
        if (user is null)
            return Errors.General.NotFound(request.UserId).ToErrorList();

        return user;
    }
    
    private async Task<UserDto?> GetUserById(long userId, CancellationToken cancellationToken = default) =>
        await _readDbContext.Users
            .Include(u => u.AdminAccount)
            .Include(u => u.VolunteerAccount)
            .Include(u => u.ParticipantAccount)
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
}