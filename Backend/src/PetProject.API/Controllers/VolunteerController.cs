using Microsoft.AspNetCore.Mvc;
using PetProject.API.Models;
using PetProject.Application.UseCases.CreateVolunteer;

namespace PetProject.API.Controllers;
[ApiController]
[Route("volunteers")]
public class VolunteerController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVolunteer(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] ICreateVolunteerUseCase useCase,
        CancellationToken cancellationToken)
    {
        var command = new CreateVolunteerCommand(
            request.FullName,
            request.Description,
            request.Experience,
            request.PetsAdopted,
            request.PetsFoundHomeQuantity,
            request.PetsInTreatment,
            request.PhoneNumber,
            request.Requisites,
            request.SocialNetworks
            );

        var volunteer = await useCase.Create(command, cancellationToken);
        return Ok(volunteer);
    }
}