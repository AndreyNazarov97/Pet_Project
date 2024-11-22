using CSharpFunctionalExtensions;
using MediatR;
using PetProject.Application.Dto;
using PetProject.Domain.Shared;

namespace PetProject.Application.VolunteersManagement.GetListVolunteers;

public class GetListVolunteersHandler : IRequestHandler<GetListVolunteersQuery, Result<VolunteerDto[], ErrorList>>
{
    private readonly IVolunteersRepository _volunteersRepository;

    public GetListVolunteersHandler(
        IVolunteersRepository volunteersRepository)
    {
        _volunteersRepository = volunteersRepository;
    }

    public async Task<Result<VolunteerDto[], ErrorList>> Handle(GetListVolunteersQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _volunteersRepository.GetList(request.Offset, request.Limit, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToErrorList();

        return result.Value;
    }
}