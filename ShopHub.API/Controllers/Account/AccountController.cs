using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ShopHub.Application.DTOs.IDentityDTOs;
using ShopHub.Application.Interfaces.IAccount;
using ShopHub.Domain.Entities.Identity;
using ShopHub.Infrastructure.IdentityData;
using ShopHub.Infrastructure.InfraService.Account;
using System.Security.Claims;

namespace ShopHub.API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly Identitydbcontext _identityContext;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenServices _refreshTokenService;

        public AccountController(
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            ITokenService tokenService , 
            Identitydbcontext identityContext,
            IConfiguration configuration,
            IRefreshTokenServices refreshTokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _identityContext = identityContext;
            _configuration = configuration;
            _refreshTokenService = refreshTokenService;
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

            var result = await _userManager.CreateAsync(user, registerDto.Password);

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
                Email = user.Email
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginDto loginDto)
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

            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Token = _tokenService.HashToken(refreshToken),
                ExpiresOn = DateTime.UtcNow.AddDays(double.Parse(_configuration["Token:RefreshTokenExpirationDays"])),
                UserId = user.Id,
                
            };

            _identityContext.RefreshTokens.Add(refreshTokenEntity);

            await _identityContext.SaveChangesAsync();



            return new LoginResponseDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                User = new UserDto
                {
                    DisplayName = user.DisplayName!,
                    Email = user.Email
                }

            };
        }

        [Authorize]
        [HttpGet("me")]
        public  async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (user is null)
                return NotFound(new 
                {
                    message = "user not found"
                });
            
            return new UserDto
            {
                DisplayName = user.Email.Split("@")[0],
                Email = user.Email
            };
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<LoginResponseDto>> RefreshToken(string refresherToken)
        {
            var refreshToken = await _refreshTokenService.RefreshAsync(refresherToken);

            if(refreshToken is null)
                return Unauthorized(new 
                {
                    message = "Unauthorized" 
                });

            return Ok(refreshToken);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new 
                { 
                    message = "Unauthorized" 
                });

            var result = await _refreshTokenService.RevokeRefreshTokenAsync(userId);

            if (!result)
                return NotFound(new
                {
                    message = "User not found."
                });


            return Ok(new 
            { 
                message = "Logged out successfully." 
            });
        }
    }
}
