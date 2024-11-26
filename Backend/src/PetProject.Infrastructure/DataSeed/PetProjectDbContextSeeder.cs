using PetProject.Domain.Shared.EntityIds;
using PetProject.Domain.Shared.ValueObjects;
using PetProject.Domain.SpeciesManagement;
using PetProject.Domain.SpeciesManagement.ValueObjects;
using PetProject.Domain.VolunteerManagement;
using PetProject.Domain.VolunteerManagement.Enums;
using PetProject.Domain.VolunteerManagement.ValueObjects;

namespace PetProject.Infrastructure.Postgres.DataSeed;

public static class PetProjectDbContextSeeder
{
    public static async Task SeedAsync(
        PetProjectDbContext context,
        CancellationToken cancellationToken = default)
    {
        if (!context.Species.Any())
        {
            await SeedSpecies(context, cancellationToken);
        }
        
        if (!context.Volunteers.Any())
        {
            await SeedVolunteers(context, cancellationToken);
        }
    }

    private static async Task SeedSpecies(PetProjectDbContext context, CancellationToken cancellationToken)
    {
        var dogSpecies = new Species(
            SpeciesId.NewId(),
            SpeciesName.Create("Dog").Value,
            new List<Breed>()
            {
                new(
                    BreedId.NewId(),
                    BreedName.Create("Bulldog").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("Poodle").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("Golden Retriever").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("Labrador").Value)
            }
        );
        
        var catSpecies = new Species(
            SpeciesId.NewId(),
            SpeciesName.Create("Cat").Value,
            new List<Breed>()
            {
                new(
                    BreedId.NewId(),
                    BreedName.Create("Persian").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("Siamese").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("Maine Coon").Value),
                new(
                    BreedId.NewId(),
                    BreedName.Create("British Shorthair").Value)
            }
        );
        
        await context.Species.AddRangeAsync([dogSpecies, catSpecies], cancellationToken);

        
    }

    private static async Task SeedVolunteers(
        PetProjectDbContext context,
        CancellationToken cancellationToken)
    {
        var firstVolunteer = new Volunteer(
            VolunteerId.NewId(),
            FullName.Create("Андрей", "Назаров", "Владиславович").Value,
            Description.Create("Самый первый волонтер").Value,
            Experience.Create(3).Value,
            PhoneNumber.Create("79511016253").Value,
            new SocialLinksList([
                SocialLink.Create("telegram", "https://t.me/andrey_nazarov").Value
            ]),
            new RequisitesList([
                Requisite.Create("Сбербанк", "8 951 101 62 53").Value
            ])
        );

        var secondVolunteer = new Volunteer(
            VolunteerId.NewId(),
            FullName.Create("Александр", "Суворов", "Васильевич").Value,
            Description.Create("Великий полководец").Value,
            Experience.Create(33).Value,
            PhoneNumber.Create("78005553535").Value,
            new SocialLinksList([
                SocialLink.Create("телеграф", "https://t.me/suvorov").Value
            ]),
            new RequisitesList([
                Requisite.Create("Банк Екатерины Великой", "Попросить Потемкина").Value
            ])
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
            firstVolunteer.RequisitesList,
            []
        );
        
        firstVolunteer.AddPet(pet);
        await context.SaveChangesAsync(cancellationToken);
    }
}