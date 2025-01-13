using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Domain.Aggregate;
using PetProject.VolunteerManagement.Domain.Entities;
using PetProject.VolunteerManagement.Domain.Enums;
using PetProject.VolunteerManagement.Domain.ValueObjects;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;

namespace PetProject.VolunteerManagement.Infrastructure.DataSeed;

public class VolunteersSeedService
{
    private readonly VolunteerDbContext _context;

    public VolunteersSeedService(VolunteerDbContext context)
    {
        _context = context;
    }
    
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (!_context.Volunteers.Any())
        {
            await SeedVolunteers(_context, cancellationToken);
        }
    }
    
    private async Task SeedVolunteers(
        VolunteerDbContext context,
        CancellationToken cancellationToken)
    {
        var firstVolunteer = new Volunteer(
            VolunteerId.NewId(),
            FullName.Create("Андрей", "Назаров", "Владиславович").Value,
            Description.Create("Самый первый волонтер").Value,
            Experience.Create(3).Value,
            PhoneNumber.Create("79511016253").Value
        );
        
        var secondVolunteer = new Volunteer(
            VolunteerId.NewId(),
            FullName.Create("Александр", "Суворов", "Васильевич").Value,
            Description.Create("Великий полководец").Value,
            Experience.Create(33).Value,
            PhoneNumber.Create("78005553535").Value
        );
        

        await context.Volunteers.AddRangeAsync([firstVolunteer, secondVolunteer], cancellationToken);
        
        
        var pet = new Pet(
            PetId.NewId(),
            PetName.Create("Bobby").Value,
            Description.Create("A very good dog").Value,
            Description.Create("A very healthy dog").Value,
            new AnimalType(SpeciesName.Create("Dog").Value, BreedName.Create("Bulldog").Value),
            Address.Create("Russia", "Moscow", "Lenina", "1", "1").Value,
            PetPhysicalAttributes.Create(10.5d, 55d).Value,
            firstVolunteer.PhoneNumber,
            new DateTime(2007, 09, 03).ToUniversalTime(),
            true,
            true,
            HelpStatus.LookingForHome,
            []
        );
        
        firstVolunteer.AddPet(pet);
        await context.SaveChangesAsync(cancellationToken);
    }
}