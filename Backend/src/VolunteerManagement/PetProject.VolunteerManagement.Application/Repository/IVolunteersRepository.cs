﻿using CSharpFunctionalExtensions;
using PetProject.Core.Database.Models;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.VolunteerManagement.Domain.Aggregate;

namespace PetProject.VolunteerManagement.Application.Repository;

public interface IVolunteersRepository
{
    public Task<Guid> Add(Volunteer volunteer,
        CancellationToken cancellationToken = default);

    public Task<Result<Guid, Error>> Delete(Volunteer volunteer,
        CancellationToken cancellationToken = default);

    public Task<Result<Volunteer, Error>> GetById(VolunteerId id,
        CancellationToken cancellationToken = default);
}