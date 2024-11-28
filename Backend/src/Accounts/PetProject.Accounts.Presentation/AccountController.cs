using MediatR;
using Microsoft.AspNetCore.Mvc;
using PetProject.Accounts.Presentation.Requests;
using PetProject.Framework;

namespace PetProject.Accounts.Presentation;

public class AccountController : ApplicationController
{
    private readonly IMediator _mediator;

    public AccountController(
        IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        
        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}