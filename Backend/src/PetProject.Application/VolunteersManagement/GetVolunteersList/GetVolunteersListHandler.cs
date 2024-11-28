using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.GetVolunteersList;

public class GetVolunteersListHandler : IRequestHandler<GetVolunteersListQuery, Result<VolunteerDto[], ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;

    public GetVolunteersListHandler(
        IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<VolunteerDto[], ErrorList>> Handle(GetVolunteersListQuery request,
        CancellationToken cancellationToken)
    {
        var queryModel = new VolunteerQueryModel()
        {
            Offset = request.Offset,
            Limit = request.Limit
        };
        var volunteers = await _volunteersRepository.Query(queryModel, cancellationToken);

        return volunteers;
    }
}