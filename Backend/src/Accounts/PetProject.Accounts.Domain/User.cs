using Microsoft.AspNetCore.Identity;
using PetProject.SharedKernel.Shared.ValueObjects;

namespace PetProject.Accounts.Domain;

public class User : IdentityUser<long>
{
    public List<SocialNetwork> SocialNetworks { get; set; } = [];
}

public static class Permissions
{
    public static class Volunteer
    {
        public const string Create = "volunteers.create";
        public const string Read = "volunteers.read";
        public const string Update = "volunteers.update";
        public const string Delete = "volunteers.delete";
    }
}