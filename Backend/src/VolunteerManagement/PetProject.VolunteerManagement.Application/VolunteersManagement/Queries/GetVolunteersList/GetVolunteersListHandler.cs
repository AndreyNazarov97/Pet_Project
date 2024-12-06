using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Queries.GetVolunteersList;

public class GetVolunteersListHandler : IRequestHandler<GetVolunteersListQuery, Result<VolunteerDto[], ErrorList>>
{
    private readonly IReadRepository _readRepository;

    public GetVolunteersListHandler(
        IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<Result<VolunteerDto[], ErrorList>> Handle(GetVolunteersListQuery request,
        CancellationToken cancellationToken)
    {
        var volunteerQuery = new VolunteerQueryModel()
        {
            Offset = request.Offset,
            Limit = request.Limit
        };
        var volunteers = await _readRepository.QueryVolunteers(volunteerQuery, cancellationToken);

        return volunteers;
    }
}