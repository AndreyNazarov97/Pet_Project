using Microsoft.AspNetCore.Mvc;
using PetProject.API.Extensions;
using PetProject.Application.UseCases.Volunteer.CreateVolunteer;
using PetProject.Application.UseCases.Volunteer.UpdateMainInfo;
using PetProject.Application.UseCases.Volunteer.UpdateRequisites;
using PetProject.Application.UseCases.Volunteer.UpdateSocialNetworks;
using PetProject.Domain.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.API.Controllers;
[Controller]
[Route("volunteers")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<VolunteerId>> CreateVolunteer(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] ICreateVolunteerUseCase useCase,
        CancellationToken cancellationToken)
    {
        var volunteerId = await useCase.Create(request, cancellationToken);

        return volunteerId.ToResponse();
    }

    [HttpPut("{volunteerId:guid}/main-info")]
    public async Task<ActionResult> UpdateVolunteerMainInfo(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateMainInfoDto dto,
        [FromServices] IUpdateMainInfoUseCase useCase,
        CancellationToken cancellationToken)
    {
        var request = new UpdateMainInfoRequest(volunteerId, dto);
        
        await useCase.UpdateMainInfo(request, cancellationToken);

        return Result.Success().ToResponse();
    }

    [HttpPut("{volunteerId:guid}/requisites")]
    public async Task<ActionResult> UpdateVolunteerRequisites(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateRequisitesDto dto,
        [FromServices] IUpdateRequisitesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var request = new UpdateRequisitesRequest(volunteerId, dto);
        
        await useCase.UpdateRequisites(request, cancellationToken);
    
        return Result.Success().ToResponse();
    }
    
    [HttpPut("{volunteerId:guid}/social-networks")]
    public async Task<ActionResult> UpdateVolunteerSocialNetworks(
        [FromRoute] Guid volunteerId,
        [FromBody] UpdateSocialNetworksDto dto,
        [FromServices] IUpdateSocialNetworksUseCase useCase,
        CancellationToken cancellationToken)
    {
        var request = new UpdateSocialNetworksRequest(volunteerId, dto);
        
        await useCase.UpdateSocialNetworks(request, cancellationToken);
    
        return Result.Success().ToResponse();
    }
}