using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using PetProject.Core.Database;
using PetProject.SharedKernel.Shared;
using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Domain.ValueObjects;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.UpdateSocialLinks;

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
        var volunteerId = VolunteerId.Create(command.VolunteerId);

        var volunteer = await _repository.GetById(volunteerId, cancellationToken);

        if (volunteer.IsFailure)
            return volunteer.Error.ToErrorList();

        var socialLinks = command.SocialLinks
            .Select(x => SocialNetwork.Create(x.Title, x.Url).Value)
            .ToList();

        //TODO: переместить хендлер в аккаунты
        //volunteer.Value.UpdateSocialLinks(socialLinks);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.Log(LogLevel.Information, "Volunteer {volunteerId} was updated social links", volunteerId);

        return volunteer.Value.Id.Id;
    }
}