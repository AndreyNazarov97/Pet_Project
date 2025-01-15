using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Accounts.Application.AccountManagement.Commands.CreateVolunteerAccount;
using VolunteerRequests.Contracts.Events;

namespace PetProject.Accounts.Infrastructure.Consumers;

public class VolunteerRequestApprovedConsumer : IConsumer<VolunteerRequestApprovedMessage>
{
    private readonly IMediator _mediator;
    private readonly ILogger<VolunteerRequestApprovedConsumer> _logger;

    public VolunteerRequestApprovedConsumer(
        IMediator mediator,
        ILogger<VolunteerRequestApprovedConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<VolunteerRequestApprovedMessage> context)
    {
        var eventData = context.Message;

        var command = new CreateVolunteerAccountCommand
        {
            UserId = eventData.UserId,
            Experience = eventData.Experience
        };
        
        var result = await _mediator.Send(command);
        
        if (result.IsFailure)
        {
            _logger.LogError("{Message}", result.Error.Errors);
        }
    }
}