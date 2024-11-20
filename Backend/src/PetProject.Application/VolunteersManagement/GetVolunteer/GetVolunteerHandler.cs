﻿using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;

namespace PetProject.Application.VolunteersManagement.GetVolunteer;

public class GetVolunteerHandler : IRequestHandler<GetVolunteerQuery, Result<VolunteerDto, ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;

    public GetVolunteerHandler(
        IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<VolunteerDto, ErrorList>> Handle(GetVolunteerQuery query,
        CancellationToken cancellationToken = default)
    {
        var volunteerId = VolunteerId.Create(query.VolunteerId);

        var result = await _volunteersRepository.GetById(volunteerId, cancellationToken);
        if (result.IsFailure)
        {
            return result.Error.ToErrorList();
        }

        var fullNameDto = new FullNameDto(
            result.Value.FullName.Name, 
            result.Value.FullName.Surname, 
            result.Value.FullName.Patronymic);
        
        var volunteerDto = new VolunteerDto
        {
            FullName = fullNameDto,
            GeneralDescription = result.Value.GeneralDescription.Value,
            PhoneNumber = result.Value.PhoneNumber.Value,
            AgeExperience = result.Value.Experience.Years
        };

        return volunteerDto;
    }
}