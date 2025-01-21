using MassTransit;
using MediatR;
using VolunteerRequests.Application.Repositories;
using VolunteerRequests.Domain.Events;

namespace VolunteerRequests.Application.EventHandlers.VolunteerRequestApprovedEventHandlers;

public class SendIntegrationEvent : INotificationHandler<VolunteerRequestApprovedEvent>
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IVolunteerRequestsRepository _volunteerRequestsRepository;

    public SendIntegrationEvent(IPublishEndpoint publishEndpoint,
        IVolunteerRequestsRepository volunteerRequestsRepository)
    {
        _publishEndpoint = publishEndpoint;
        _volunteerRequestsRepository = volunteerRequestsRepository;
    }

    public async Task Handle(VolunteerRequestApprovedEvent domainEvent, CancellationToken cancellationToken)
    {
        var requestResult =
            await _volunteerRequestsRepository.GetById(domainEvent.VolunteerRequestId, cancellationToken);
        if (requestResult.IsFailure)
            return;

        var volunteerInfo = requestResult.Value.VolunteerInfo;

        var integrationEvent = new Contracts.Events.VolunteerRequestApprovedMessage
        {
            UserId = domainEvent.UserId,
            FirstName = volunteerInfo.FullName.Name,
            Surname = volunteerInfo.FullName.Surname,
            Patronymic = volunteerInfo.FullName.Patronymic,
            Description = volunteerInfo.GeneralDescription.Value,
            PhoneNumber = volunteerInfo.PhoneNumber.Value,
            Experience = volunteerInfo.WorkExperience.Years
        };

        await _publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}