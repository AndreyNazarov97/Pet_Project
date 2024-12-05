using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Application.Managers;
using PetProject.Accounts.Domain;
using PetProject.Core.Database;
using PetProject.Core.ObjectMappers;
using PetProject.SharedKernel.Shared;

namespace PetProject.Accounts.Application.AccountManagement.Commands.RegisterUser;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UnitResult<ErrorList>>
{
    private readonly IAccountManager _accountManager;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterUserHandler> _logger;

    public RegisterUserHandler(
        IAccountManager accountManager,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IUnitOfWork unitOfWork,
        ILogger<RegisterUserHandler> logger)
    {
        _accountManager = accountManager;
        _userManager = userManager;
        _roleManager = roleManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UnitResult<ErrorList>> Handle(RegisterUserCommand command,
        CancellationToken cancellationToken = default)
    {
        var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var role = await _roleManager.Roles
                .FirstOrDefaultAsync(r => r.Name == ParticipantAccount.Participant, cancellationToken);
            if (role is null)
                return Errors.General.NotFound().ToErrorList();

            var user = User.CreateParticipant(command.FullName.ToEntity(), command.UserName, command.Email, role);

            var result = await _userManager.CreateAsync(user, command.Password);

            if (result.Succeeded == false)
            {
                var errors = result.Errors.Select(e =>
                    Error.Failure(e.Code, e.Description)).ToList();

                return new ErrorList(errors);
            }

            var participantAccount = new ParticipantAccount(user);
            await _accountManager.CreateParticipantAccount(participantAccount, cancellationToken);

            user.ParticipantAccount = participantAccount;
            user.ParticipantAccountId = participantAccount.Id;

            await _userManager.UpdateAsync(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            transaction.Commit();

            _logger.LogInformation("User {username} was registered", user.UserName);
            return UnitResult.Success<ErrorList>();
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to register user {username}", command.UserName);
            transaction.Rollback();

            return Error.Failure("could.not.register.user", e.Message).ToErrorList();
        }
    }
}