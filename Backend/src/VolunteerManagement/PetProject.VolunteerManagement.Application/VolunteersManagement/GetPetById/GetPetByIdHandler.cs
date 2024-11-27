using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Core.Dtos;
using PetProject.SharedKernel.Shared;
using PetProject.VolunteerManagement.Application.Models;
using PetProject.VolunteerManagement.Application.Repository;

namespace PetProject.VolunteerManagement.Application.VolunteersManagement.GetPetById;

public class GetPetByIdHandler : IRequestHandler<GetPetByIdQuery, Result<PetDto,ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;

    public GetPetByIdHandler(
        IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }
    public async Task<Result<PetDto, ErrorList>> Handle(GetPetByIdQuery request, CancellationToken cancellationToken)
    {
        var petQuery = new PetQueryModel
        {
            PetId = request.PetId,
        };
        
        var result = (await _volunteersRepository.QueryPets(petQuery, cancellationToken))
            .SingleOrDefault();
        if(result is null)
            return Errors.General.NotFound(request.PetId).ToErrorList();
        
        return result;
    }
}