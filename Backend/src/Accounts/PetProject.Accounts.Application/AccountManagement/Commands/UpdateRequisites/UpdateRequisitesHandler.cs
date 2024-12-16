using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Application.AccountManagement.Commands.UpdateRequisites;

public class UpdateRequisitesHandler : IRequestHandler<UpdateRequisitesCommand, Result<long, ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly IAccountManager _accountManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateRequisitesHandler> _logger;

    public UpdateRequisitesHandler(
        UserManager<User> userManager,
        IAccountManager accountManager,
        [FromKeyedServices(Constants.Context.Accounts)]IUnitOfWork unitOfWork,
        ILogger<UpdateRequisitesHandler> logger)
    {
        _userManager = userManager;
        _accountManager = accountManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<long, ErrorList>> Handle(UpdateRequisitesCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);

        if (user is null)
            return Errors.General.NotFound(command.UserId).ToErrorList();

        var volunteerAccount = user.VolunteerAccount;

        if (volunteerAccount is null)
            return Error.NotFound(
                "could.not.find.volunteer.account", 
                "Volunteer account not found")
                .ToErrorList();

        var requisites = command.Requisites
            .Select(x => Requisite.Create(x.Title, x.Description).Value)
            .ToList();
        
        volunteerAccount.AddRequisites(requisites);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.Log(LogLevel.Information, "VolunteerAccount {UserId} was updated requisites", command.UserId);
        return volunteerAccount.Id;
    }
}