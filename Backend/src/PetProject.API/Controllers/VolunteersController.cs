using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PetProject.API.Extensions;
using PetProject.API.Response;
using PetProject.Application.UseCases.CreateVolunteer;
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
}