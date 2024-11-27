using PetProject.SharedKernel.Shared.EntityIds;
using PetProject.SharedKernel.Shared.ValueObjects;
using PetProject.VolunteerManagement.Domain.Aggregate;
using PetProject.VolunteerManagement.Domain.Entities;
using PetProject.VolunteerManagement.Domain.Enums;
using PetProject.VolunteerManagement.Domain.ValueObjects;
using PetProject.VolunteerManagement.Infrastructure.DbContexts;

namespace PetProject.VolunteerManagement.Infrastructure.DataSeed;

public static class VolunteerDbContextSeeder
{
    public static async Task SeedAsync(
        PetProjectDbContext context,
        CancellationToken cancellationToken = default)
    {
        if (!context.Volunteers.Any())
        {
            await SeedVolunteers(context, cancellationToken);
        }
    }
    
    private static async Task SeedVolunteers(
        PetProjectDbContext context,
        CancellationToken cancellationToken)
    {
        var socialLink = SocialLink.Create("telegram", "https://t.me/andrey_nazarov").Value;
        var requisite = Requisite.Create("Сбербанк", "8 951 101 62 53").Value;
        
        var firstVolunteer = new Volunteer(
            VolunteerId.NewId(),
            FullName.Create("Андрей", "Назаров", "Владиславович").Value,
            Description.Create("Самый первый волонтер").Value,
            Experience.Create(3).Value,
            PhoneNumber.Create("79511016253").Value,
            [socialLink],
            [requisite]
        );

        var secondSocialLink = SocialLink.Create("telegram", "https://t.me/suvorov").Value;
        var secondRequisite = Requisite.Create("Банк Екатерины Великой", "Попросить Потемкина").Value;
        
        var secondVolunteer = new Volunteer(
            VolunteerId.NewId(),
            FullName.Create("Александр", "Суворов", "Васильевич").Value,
            Description.Create("Великий полководец").Value,
            Experience.Create(33).Value,
            PhoneNumber.Create("78005553535").Value,
            [secondSocialLink],
            [secondRequisite]
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
            new DateOnly(2007, 09, 03),
            true,
            true,
            HelpStatus.LookingForHome,
            [requisite],
            []
        );
        
        firstVolunteer.AddPet(pet);
        await context.SaveChangesAsync(cancellationToken);
    }
}