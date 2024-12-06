using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Domain;
using PetProject.Core.Database;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Application.AccountManagement.Commands.UpdateSocialNetworks;

public class UpdateSocialNetworksHandler : IRequestHandler<UpdateSocialNetworksCommand, Result<long, ErrorList>>
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateSocialNetworksHandler> _logger;

    public UpdateSocialNetworksHandler(
        UserManager<User> userManager,
        IUnitOfWork unitOfWork,
        ILogger<UpdateSocialNetworksHandler> logger)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<long, ErrorList>> Handle(UpdateSocialNetworksCommand command,
        CancellationToken cancellationToken = default)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == command.UserId, cancellationToken);

        if (user is null)
            return Errors.General.NotFound(command.UserId).ToErrorList();

        var socialNetworks = command.SocialLinks
            .Select(x => SocialNetwork.Create(x.Title, x.Url).Value);
        
        user.AddSocialNetworks(socialNetworks);

        await _userManager.UpdateAsync(user); // unitOfWork is not working
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.Log(LogLevel.Information, "User {user.Id} was updated social links", user.Id);

        return user.Id;
    }
}