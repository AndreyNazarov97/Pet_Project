using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;
using PetProject.Core.Database;
using PetProject.SharedKernel.Constants;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Application.AccountManagement.Commands.CreateVolunteerAccount;

public class CreateVolunteerAccountHandler : IRequestHandler<CreateVolunteerAccountCommand, Result<long, ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly IAccountManager _accountManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateVolunteerAccountHandler> _logger;

    public CreateVolunteerAccountHandler(
        UserManager<User> userManager,
        IAccountManager accountManager,
        [FromKeyedServices(Constants.Context.Accounts)]
        IUnitOfWork unitOfWork,
        ILogger<CreateVolunteerAccountHandler> logger)
    {
        _userManager = userManager;
        _accountManager = accountManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }


    public async Task<Result<long, ErrorList>> Handle(CreateVolunteerAccountCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId.ToString());
        if (user is null)
            return Errors.User.NotFound(command.UserId).ToErrorList();
        
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var volunteerAccount = new VolunteerAccount(Experience.Create(command.Experience).Value, user);
            var result = await _accountManager.CreateVolunteerAccount(volunteerAccount, cancellationToken);
            if (result.IsFailure)
                return result.Error;
        
            user.VolunteerAccount = volunteerAccount;
            user.VolunteerAccountId = volunteerAccount.Id;
            //await _userManager.UpdateAsync(user);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();
            
            _logger.Log(LogLevel.Information, "VolunteerAccount for user with id {UserId} was created", command.UserId);
            return volunteerAccount.Id;
        }
        catch (Exception e)
        {
            _logger.LogError("failed to create volunteer account for user {UserId}", command.UserId);
            transaction.Rollback();

            return Error.Failure("could.not.create.account", e.Message).ToErrorList();
        }
        
    }
}