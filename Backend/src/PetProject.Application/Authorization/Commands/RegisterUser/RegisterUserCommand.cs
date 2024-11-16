﻿using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Domain.Shared;

namespace PetProject.Application.Authorization.Commands.RegisterUser;

public record RegisterUserCommand() : IRequest<UnitResult<ErrorList>>
{
    public required string Email { get; init; }
    public required string Username { get; init; }
    public required string Password { get; init; }
}