using Microsoft.EntityFrameworkCore;
using PetProject.Application.UseCases.CreateVolunteer;
using PetProject.Domain.Entities;
using PetProject.Infrastructure.Postgres.Abstractions;

namespace PetProject.Infrastructure.Postgres.Storages;

public class CreateVolunteerStorage : ICreateVolunteerStorage
{
    private readonly IGuidFactory _guidFactory;
    private readonly PetProjectDbContext _dbContext;

    public CreateVolunteerStorage(
        IGuidFactory guidFactory,
        PetProjectDbContext dbContext)
    {
        _guidFactory = guidFactory;
        _dbContext = dbContext;
    }

    public async Task<Volunteer> CreateVolunteer(CreateVolunteerCommand command, CancellationToken cancellationToken)
    {
        var id = _guidFactory.Create();

        var volunteer = new Volunteer
        (
            id,
            command.FullName,
            command.Description,
            command.Experience,
            command.PetsAdopted,
            command.PetsFoundHomeQuantity,
            command.PetsInTreatment,
            command.PhoneNumber,
            command.SocialNetworks,
            command.Requisites
        );

        await _dbContext.Set<Volunteer>().AddAsync(volunteer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await _dbContext.Set<Volunteer>()
            .Where(v => v.Id == id)
            .FirstAsync(cancellationToken);
    }
}