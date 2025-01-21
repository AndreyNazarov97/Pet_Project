using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Core.Dtos;
using PetProject.VolunteerManagement.Application.VolunteersManagement.Commands.CreateVolunteer;
using VolunteerRequests.Contracts.Events;

namespace PetProject.VolunteerManagement.Infrastructure.Consumers;

public class VolunteerRequestApprovedConsumer : IConsumer<VolunteerRequestApprovedMessage>
{
    private readonly IMediator _mediator;
    private readonly ILogger<VolunteerRequestApprovedConsumer> _logger;

    public VolunteerRequestApprovedConsumer(IMediator mediator,
        ILogger<VolunteerRequestApprovedConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<VolunteerRequestApprovedMessage> context)
    {
        var eventData = context.Message;
        var fullNameDto = new FullNameDto
        {
            Name = eventData.FirstName,
            Surname = eventData.Surname,
            Patronymic = eventData.Patronymic
        };

        var command = new CreateVolunteerCommand
        {
            FullName = fullNameDto,
            Description = eventData.Description,
            AgeExperience = eventData.Experience,
            PhoneNumber = eventData.PhoneNumber
        };
        
       var result =  await _mediator.Send(command);
       
        if (result.IsFailure)
        {
            _logger.LogError("{Message}", result.Error.Errors);
        }
    }
}