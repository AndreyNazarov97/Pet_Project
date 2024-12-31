using Bogus;
using Bogus.DataSets;
using PetProject.VolunteerManagement.Domain.Enums;

namespace PetProject.SharedTestData.Creators;

public static class Random
{
    private static readonly Faker Faker = new();

    public static Address Address => Faker.Address;
    public static int Experience => Faker.Random.Int(1, 10);
    public static string Name => Faker.Name.FirstName();
    public static string Words => Faker.Lorem.Sentence(3);
    public static string PhoneNumber => Faker.Phone.PhoneNumber("7##########");
    public static DateTimeOffset DateTimeOffset => Faker.Date.PastOffset(10, DateTimeOffset.UtcNow);
    public static double Double => Faker.Random.Double(1d, 100d);
    public static long Long => Faker.Random.Long(1, 10_000);
    public static bool Bool => Faker.Random.Bool();
    public static string Email => Faker.Internet.Email();
    public static string UserName => Faker.Internet.UserName();
    public static HelpStatus HelpStatus => Faker.Random.Enum<HelpStatus>();

    public static T Element<T>(IReadOnlyList<T> list) => Faker.PickRandom(list.ToArray());
}
