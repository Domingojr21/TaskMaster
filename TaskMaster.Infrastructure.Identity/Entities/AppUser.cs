using Microsoft.AspNetCore.Identity;

namespace TaskMaster.Infrastructure.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
