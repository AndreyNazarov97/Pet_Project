using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Database.Models;
using PetProject.Core.Database.Repository;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.Queries.GetPetById;

public class GetPetByIdHandler : IRequestHandler<GetPetByIdQuery, Result<PetDto,ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;
    private readonly IReadRepository _readRepository;

    public GetPetByIdHandler(
        IVolunteersRepository volunteersRepository,
        IReadRepository readRepository)
    {
        _volunteersRepository = volunteersRepository;
        _readRepository = readRepository;
    }
    public async Task<Result<PetDto, ErrorList>> Handle(GetPetByIdQuery request, CancellationToken cancellationToken)
    {
        var petQuery = new PetQueryModel
        {
            PetId = request.PetId,
        };
        
        var result = (await _readRepository.QueryPets(petQuery, cancellationToken))
            .SingleOrDefault();
        if(result is null)
            return Errors.General.NotFound(request.PetId).ToErrorList();
        
        return result;
    }
}