

namespace TaskMaster.Core.Application.Dtos.User
{
    public class UserDTO
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public bool IsActive { get; set; }
        public string Phone { get; set; } = null!;
        public List<string>? Roles { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
