using Microsoft.AspNetCore.Mvc;
using PetProject.API.Controllers.Account.Requests;
using PetProject.API.Response;
using PetProject.Application.Authorization.Commands.LoginUser;
using PetProject.Application.Authorization.Commands.RegisterUser;

namespace PetProject.API.Controllers.Account;

public class AccountController : ApplicationController
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        [FromServices] RegisterUserHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
        {
            var responseErrors = result.Error.Select(e => new ResponseError(e.Code, e.Message, null))
                .ToList();
            var envelope = Envelope.Error(responseErrors);

            return new ObjectResult(envelope)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] LoginHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
        {
            var responseErrors = result.Error.Select(e => new ResponseError(e.Code, e.Message, null))
                .ToList();
            var envelope = Envelope.Error(responseErrors);

            return new ObjectResult(envelope)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }

        return Ok(result.Value);
    }
}