using CSharpFunctionalExtensions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using PetProject.Application.Dto;
using PetProject.Application.VolunteersManagement;
using PetProject.Domain.Shared;
using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Infrastructure.Postgres.Abstractions;

namespace PetProject.Infrastructure.Postgres.Repositories;

public class VolunteersRepository : IVolunteersRepository
{
    private readonly PetProjectDbContext _context;
    private readonly IPostgresConnectionFactory _connectionFactory;

    public VolunteersRepository(
        PetProjectDbContext context,
        IPostgresConnectionFactory connectionFactory)
    {
        _context = context;
        _connectionFactory = connectionFactory;
    }

    public async Task<Guid> Add(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        await _context.Volunteers.AddAsync(volunteer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return volunteer.Id;
    }

    public async Task<Result<Guid, Error>> Delete(Volunteer volunteer, CancellationToken cancellationToken = default)
    {
        var existedVolunteer = await _context.Volunteers
            .FirstOrDefaultAsync(v => v.Id == volunteer.Id, cancellationToken);

        if (existedVolunteer == null)
            return Errors.General.NotFound();

        _context.Volunteers.Remove(existedVolunteer);
        return volunteer.Id.Id;
    }

    public async Task<Result<Volunteer, Error>> GetByPhoneNumber(PhoneNumber requestNumber,
        CancellationToken cancellationToken = default)
    {
        var volunteer =
            await _context.Volunteers.FirstOrDefaultAsync(v => v.PhoneNumber == requestNumber, cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound();

        return volunteer;
    }

    public async Task<Result<Volunteer, Error>> GetById(VolunteerId id, CancellationToken cancellationToken = default)
    {
        var volunteer = await _context.Volunteers
            .Where(v => v.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        if (volunteer == null)
            return Errors.General.NotFound(id.Id);

        return volunteer;
    }

    public async Task<Result<VolunteerDto[], Error>> GetList(int offset, int limit, CancellationToken cancellationToken = default)
    {
        const string sql = """
                            SELECT "Id", "FullName", "GeneralDescription", "PhoneNumber", "AgeExperience"
                            FROM volunteers
                            ORDER BY "FullName"
                            LIMIT @limit
                            OFFSET @offset
                           """;

        var param = new DynamicParameters();
        param.Add("offset", offset);
        param.Add("limit", limit);
        
        var command = new CommandDefinition(sql, param, cancellationToken: cancellationToken);
        
        await using var connection = _connectionFactory.GetConnection();
        var result = (await connection.QueryAsync<VolunteerDto>(command)).ToArray();
    
        if(result.Length == 0)
            return Errors.General.NotFound();
        
        return result;
    }

    public async Task<Result<List<Volunteer>, Error>> GetAll(CancellationToken cancellationToken = default)
    {
        var volunteers = await _context.Volunteers.ToListAsync(cancellationToken);
        return volunteers;
    }
}