using MediatR;
using VolunteerRequests.Domain.Events;

namespace VolunteerRequests.Application.EventHandlers.VolunteerRequestRejectedEventHandlers;

public class VolunteerRequestRejectedEventHandler : INotificationHandler<VolunteerRequestRejectedEvent>
{
    public Task Handle(VolunteerRequestRejectedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}