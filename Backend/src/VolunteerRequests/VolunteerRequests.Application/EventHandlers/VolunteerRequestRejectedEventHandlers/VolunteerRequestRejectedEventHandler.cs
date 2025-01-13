using MediatR;
using PetProject.SharedKernel.Exceptions;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Domain.Aggregate;
using VolunteerRequests.Domain.Events;

namespace VolunteerRequests.Application.EventHandlers.VolunteerRequestRejectedEventHandlers;

public class VolunteerRequestRejectedEventHandler : INotificationHandler<VolunteerRequestRejectedEvent>
{
    private readonly IUserRestrictionsRepository _userRestrictionsRepository;

    public VolunteerRequestRejectedEventHandler(IUserRestrictionsRepository userRestrictionsRepository)
    {
        _userRestrictionsRepository = userRestrictionsRepository;
    }
    
    public async Task Handle(VolunteerRequestRejectedEvent notification, CancellationToken cancellationToken)
    {
        var userRestrictionResult = UserRestriction.Create(notification.UserId);
        if (userRestrictionResult.IsFailure)
            throw new CanNotCreateRecordException(userRestrictionResult.Error);

        await _userRestrictionsRepository.Add(userRestrictionResult.Value, cancellationToken);
    }
}