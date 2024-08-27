using PetProject.Application.UseCases.Volunteer.UpdateRequisites;
using PetProject.Application.UseCases.Volunteer.UpdateSocialNetworks;
using PetProject.Domain.Shared;

namespace PetProject.Application.UseCases.Volunteer.UpdateMainInfo;

public interface IUpdateMainInfoUseCase
{
    Task<Result> UpdateMainInfo(UpdateMainInfoRequest request, CancellationToken cancellationToken);
}