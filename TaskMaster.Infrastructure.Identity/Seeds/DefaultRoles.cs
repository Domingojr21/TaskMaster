using Microsoft.AspNetCore.Identity;
using TaskMaster.Core.Application.Enums;
using TaskMaster.Infrastructure.Identity.Entities;

namespace TaskMaster.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.Client.ToString()));
        }

    }
}
