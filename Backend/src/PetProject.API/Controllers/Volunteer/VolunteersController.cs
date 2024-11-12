using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Extensions;
using PetProject.Application.Dto;
using PetProject.Application.VolunteersManagement.CreateVolunteer;
using PetProject.Application.VolunteersManagement.GetVolunteer;
using PetProject.Application.VolunteersManagement.UpdateRequisites;
using PetProject.Application.VolunteersManagement.UpdateSocialLinks;
using PetProject.Application.VolunteersManagement.UpdateVolunteer;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Infrastructure.Authorization;

namespace PetProject.API.Controllers.Volunteer;

public class VolunteersController : ApplicationController
{
    [Permission("volunteer.read")]
    [HttpGet("{volunteerId:guid}")]
    public async Task<ActionResult<VolunteerDto>> GetVolunteer(
        [FromRoute] Guid volunteerId,
        [FromServices] GetVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var request = new GetVolunteerRequest(volunteerId);

        var result = await handler.Handle(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission("volunteer.create")]
    [HttpPost]
    public async Task<ActionResult<VolunteerId>> CreateVolunteer(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] CreateVolunteerHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Execute(request, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/main-info")]
    public async Task<ActionResult> UpdateVolunteerMainInfo(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateVolunteerDto dto,
        [FromServices] UpdateVolunteerHandler handler,
        [FromServices] IValidator<UpdateVolunteerRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new UpdateVolunteerRequest(volunteerId, dto);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await handler.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{volunteerId:guid}/requisites")]
    public async Task<ActionResult> UpdateVolunteerRequisites(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateRequisitesDto dto,
        [FromServices] UpdateRequisitesHandler handler,
        [FromServices] IValidator<UpdateRequisitesRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new UpdateRequisitesRequest(volunteerId, dto);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await handler.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPut("{volunteerId:guid}/social-networks")]
    public async Task<ActionResult> UpdateVolunteerSocialNetworks(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateSocialLinksDto dto,
        [FromServices] UpdateSocialLinksHandler handler,
        [FromServices] IValidator<UpdateSocialLinksRequest> validator,
        CancellationToken cancellationToken)
    {
        var request = new UpdateSocialLinksRequest(volunteerId, dto);
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToValidationErrorResponse();
        
        var result = await handler.Execute(request, cancellationToken);
        
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}