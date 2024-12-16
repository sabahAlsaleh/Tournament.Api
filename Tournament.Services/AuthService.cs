using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.DTOs;
using Tournament.Core.Entities;

namespace Tournament.Services
{
    public class AuthService : IAuthService
    {
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        //private readonly JwtConfiguration jwtConfig;
        //private readonly IConfiguration config;
        private ApplicationUser? user;

        public AuthService(IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager/* IOptions<JwtConfiguration> jwtConfig, IConfiguration config*/)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
          //  this.jwtConfig = jwtConfig.Value;
            // this.config = config;
        }
        public async Task<IdentityResult> RegisterUser(UserRegistrationDto registrationDto)
        {
            if (registrationDto is null)
            {
                throw new ArgumentNullException(nameof(registrationDto));
            }

            var roleExists = await roleManager.RoleExistsAsync(registrationDto.Role);
            if (!roleExists)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Role does not exist" });
            }

            var user = mapper.Map<ApplicationUser>(registrationDto);

            var result = await userManager.CreateAsync(user, registrationDto.Password!);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, registrationDto.Role);
            }

            return result;
        }

        public async Task<bool> ValidateUserAsync(UserAuthDto userAuthDto)
        {
            if (userAuthDto is null)
            {
                throw new ArgumentNullException(nameof(userAuthDto));
            }
            var user = await userManager.FindByNameAsync(userAuthDto.Email);
            return user != null && await userManager.CheckPasswordAsync(user, userAuthDto.PassWord);

        }
    }
}
