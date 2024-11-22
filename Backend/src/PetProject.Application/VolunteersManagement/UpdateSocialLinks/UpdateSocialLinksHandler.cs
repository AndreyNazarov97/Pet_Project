﻿using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Application.Abstractions;
using PetProject.Application.Models;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Application.VolunteersManagement.UpdateSocialLinks;

public class UpdateSocialLinksHandler : IRequestHandler<UpdateSocialLinksCommand, Result<Guid, ErrorList>>
{
    private readonly IVolunteersRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateSocialLinksHandler> _logger;

    public UpdateSocialLinksHandler(
        IVolunteersRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<UpdateSocialLinksHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid, ErrorList>> Handle(UpdateSocialLinksCommand command,
        CancellationToken cancellationToken = default)
    {
        var volunteerQuery = new VolunteerQueryModel()
        {
            VolunteerIds = [command.VolunteerId]
        };

        var volunteer = (await _repository.Query(volunteerQuery, cancellationToken)).SingleOrDefault();
        if (volunteer == null)
            return Errors.General.NotFound(command.VolunteerId).ToErrorList();

        var volunteerEntity = volunteer.ToEntity();

        var socialLinks = command.SocialLinks
            .Select(x => SocialLink.Create(x.Title, x.Url).Value);
        var socialLinksList = new SocialLinksList(socialLinks);

        volunteerEntity.UpdateSocialLinks(socialLinksList);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated social links", command.VolunteerId);

        return volunteerEntity.Id.Id;
    }
}