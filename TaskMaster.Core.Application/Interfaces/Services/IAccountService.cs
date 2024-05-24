using TaskMaster.Core.Application.Dtos.Account;

namespace TaskMaster.Infraestructure.Identity.Services
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request, bool IsForApi);
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request, string origin, string Role);
        Task SignOutAsync();
        Task<bool> VerifyUser(string UserName);
    }
}