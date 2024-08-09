using Microsoft.AspNetCore.Mvc;
using PetProject.Application.Models;
using PetProject.Application.UseCases.CreateVolunteer;

namespace PetProject.API.Controllers;
[Controller]
[Route("volunteers")]
public class VolunteersController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVolunteer(
        [FromBody] CreateVolunteerRequest request,
        [FromServices] ICreateVolunteerUseCase useCase,
        CancellationToken cancellationToken)
    {
        var volunteer = await useCase.Create(request, cancellationToken);
        return Ok(volunteer);
    }
}