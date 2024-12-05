using Microsoft.AspNetCore.Identity;

namespace PetProject.Accounts.Domain;

public class Role : IdentityRole<long>
{
    public List<RolePermission> RolePermissions { get; set; }
}