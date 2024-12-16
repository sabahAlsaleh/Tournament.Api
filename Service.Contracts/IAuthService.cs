using Microsoft.AspNetCore.Identity;
using Tournament.Core.DTOs;

namespace Service.Contracts
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterUser(UserRegistrationDto userRegistrationDto);
        Task<bool> ValidateUserAsync(UserAuthDto userAuthDto);
    }
}