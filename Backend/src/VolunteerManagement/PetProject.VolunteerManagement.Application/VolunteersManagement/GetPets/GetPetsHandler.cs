using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.GetPets;

public class GetPetsHandler : IRequestHandler<GetPetsQuery, Result<PetDto[], ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IReadRepository _readRepository;

    public GetPetsHandler(
        IVolunteersRepository volunteersRepository,
        IReadRepository readRepository)
    {
        _volunteersRepository = volunteersRepository;
        _readRepository = readRepository;
    }

    public async Task<Result<PetDto[], ErrorList>> Handle(GetPetsQuery request, CancellationToken cancellationToken)
    {
        var petQuery = new PetQueryModel
        {
            VolunteerId = request.VolunteerId,
            Name = request.Name,
            MinAge = request.MinAge,
            Breed = request.Breed,
            Species = request.Species,
            HelpStatus = (int?)request.HelpStatus,
            SortBy = request.SortBy,
            SortDescending = request.SortDescending,
            Limit = request.Limit,
            Offset = request.Offset
        };

        var result = await _readRepository.QueryPets(petQuery, cancellationToken);

        return result;
    }
}