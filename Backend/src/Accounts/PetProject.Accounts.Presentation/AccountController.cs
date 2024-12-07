﻿using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetProject.Accounts.Application.Mappers;
using PetProject.Accounts.Contracts.Requests;
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
        
        var refreshToken = result.Value.RefreshToken; 
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, 
            Secure = false, 
            Expires = DateTime.UtcNow.AddDays(30)
        };
        HttpContext.Response.Cookies.Append("RefreshToken", refreshToken.ToString(), cookieOptions);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await _mediator.Send(command, cancellationToken);
        
        var refreshToken = result.Value.RefreshToken; 
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, 
            Secure = false, 
            Expires = DateTime.UtcNow.AddDays(30)
        };
        HttpContext.Response.Cookies.Append("RefreshToken", refreshToken.ToString(), cookieOptions);


        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{userId:long}/social-networks")]
    public async Task<ActionResult> UpdateSocialNetworks(
        [FromRoute] long userId,
        [FromBody] UpdateSocialNetworksRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(userId);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{userId:long}/requisites")]
    public async Task<ActionResult> UpdateRequisites(
        [FromRoute] long userId,
        [FromBody] UpdateRequisitesRequest request,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(userId);

        var result = await _mediator.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}