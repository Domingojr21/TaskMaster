

namespace TaskMaster.Core.Application.Dtos.Account
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Revoked { get; set; }
        public string? ReplacedByToken { get; set; }

        // Property to check if the token is expired
        public bool IsExpired => Expires.HasValue && DateTime.UtcNow >= Expires.Value;

        // Property to check if the token is active
        public bool IsActive => !IsExpired && Revoked == null;
    }

}
