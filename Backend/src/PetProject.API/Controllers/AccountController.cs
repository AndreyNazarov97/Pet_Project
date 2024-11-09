using Microsoft.AspNetCore.Mvc;
using PetProject.Application.AccountManagement.Commands;

namespace PetProject.API.Controllers;

public class AccountController: ApplicationController 
{
    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(request.Email, request.Username, request.Password);
        var result = await handler.Handle(command, cancellationToken);

        return Ok("ok");
    }
}

public record RegisterUserRequest(string Email, string Username ,string Password);