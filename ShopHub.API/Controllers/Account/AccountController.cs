using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Application.DTOs.IDentityDTOs;
using ShopHub.Application.Interfaces.IAccount;
using ShopHub.Domain.Entities.Identity;
using ShopHub.Infrastructure.InfraService.Account;

namespace ShopHub.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager , SignInManager<AppUser> signInManager , ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var checkEmail = await _userManager.FindByEmailAsync(registerDto.Email);

            if (checkEmail is not null)
                return BadRequest(new 
                { 
                    message = "This Email is Exist" 
                });

            var result = await _userManager.CreateAsync(user , registerDto.Password);

            if (!result.Succeeded)
            {
                if (result.Errors.Any(p => p.Code.StartsWith("Password")))
                    return BadRequest(new 
                    { 
                        message = "Weak Password" 
                    });

                return BadRequest(new 
                { 
                    message = "Unable to create the account." 
                });
            }

            

            return new UserDto 
            { 
                DisplayName = user.DisplayName, 
                Email = user.Email,
                Token = ""
            };

        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
                return Unauthorized(new 
                {
                    message = "Invalid email or password" 
                });

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });

            var token = await _tokenService.CreateTokenAsync(user);

            return new UserDto
            {
                DisplayName = loginDto.Email,
                Email = loginDto.Email,
                Token = token
            };
        }
    }
}
