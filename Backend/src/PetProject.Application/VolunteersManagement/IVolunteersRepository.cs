using CSharpFunctionalExtensions;
using PetProject.Application.Dto;
using PetProject.Application.Models;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement;

namespace PetProject.Application.VolunteersManagement;

public interface IVolunteersRepository
{
    public Task<Guid> Add(Volunteer volunteer, 
        CancellationToken cancellationToken = default);
    
    public Task<Result<Guid, Error>> Delete(Volunteer volunteer, 
        CancellationToken cancellationToken = default);
    
    public Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber requestNumber,
        CancellationToken cancellationToken = default);

    public Task<Result<Volunteer, Error>> GetById(VolunteerId id, 
        CancellationToken cancellationToken = default);
    
    Task<Result<VolunteerDto[], Error>> GetList(
        int offset, int limit, CancellationToken cancellationToken = default);
    
    Task<Result<VolunteerDto[], Error>> Query(
        VolunteerQueryModel query, CancellationToken cancellationToken = default);
}