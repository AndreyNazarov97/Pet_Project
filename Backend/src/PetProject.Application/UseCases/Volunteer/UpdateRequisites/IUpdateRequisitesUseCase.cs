using PetProject.Domain.Shared;

namespace PetProject.Application.UseCases.Volunteer.UpdateRequisites;

public interface IUpdateRequisitesUseCase
{
    Task<Result> UpdateRequisites(UpdateRequisitesRequest request, CancellationToken cancellationToken);
}