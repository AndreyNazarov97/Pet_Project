using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetProject.Core.Database;
using PetProject.Core.Options;
using PetProject.VolunteerManagement.Domain.Aggregate;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;

namespace PetProject.VolunteerManagement.Infrastructure.Services;

public class DeleteExpiredVolunteersService
{
    private readonly VolunteerDbContext _context;
    private readonly EntityRetentionOptions _entityRetentionOptions;

    public DeleteExpiredVolunteersService(
        VolunteerDbContext context,
        IOptions<EntityRetentionOptions> options)
    {
        _context = context;
        _entityRetentionOptions = options.Value;
    }

    public async Task Process()
    {
        var volunteersToDelete = await _context.Volunteers
            .Where(v => v.DeletionDate < DateTime.UtcNow - TimeSpan.FromDays(_entityRetentionOptions.Days))
            .ToListAsync();

        _context.Volunteers.RemoveRange(volunteersToDelete);
        await _context.SaveChangesAsync();
    }
}