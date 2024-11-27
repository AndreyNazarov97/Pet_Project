using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.VolunteerManagement.Application.Models;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.GetVolunteersList;

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