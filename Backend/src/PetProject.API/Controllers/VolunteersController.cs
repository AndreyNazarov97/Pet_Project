using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Extensions;
using PetProject.API.Response;
using PetProject.Application.UseCases.CreateVolunteer;
using PetProject.Application.UseCases.UpdateVolunteer;
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

    [HttpPut("{volunteerGuid:guid}/update-main-info")]
    public async Task<ActionResult> UpdateVolunteerMainInfo(
        [FromRoute] Guid volunteerGuid,
        [FromBody] UpdateMainInfoRequest request,
        [FromServices] IUpdateVolunteerUseCase useCase,
        CancellationToken cancellationToken)
    {
        var volunteerId = VolunteerId.FromGuid(volunteerGuid);
        await useCase.UpdateMainInfo(volunteerId, request, cancellationToken);

        return Result.Success().ToResponse();
    }
}