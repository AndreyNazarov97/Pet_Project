using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Extensions;
using PetProject.Application.Dto;
using PetProject.Application.Volunteers.CreateVolunteer;
using PetProject.Application.Volunteers.UpdateRequisites;
using PetProject.Application.Volunteers.UpdateSocialLinks;
using PetProject.Application.Volunteers.UpdateVolunteer;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.API.Controllers;
public class VolunteersController : ApplicationController
{
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