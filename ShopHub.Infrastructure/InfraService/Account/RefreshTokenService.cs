using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopHub.Application.DTOs.IDentityDTOs;
using ShopHub.Application.Interfaces.IAccount;
using ShopHub.Domain.Entities.Identity;
using ShopHub.Infrastructure.IdentityData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Infrastructure.InfraService.Account
{
    public class RefreshTokenService : IRefreshTokenServices
    {
        private readonly Identitydbcontext _identityContext;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly Identitydbcontext _appdbcontext;

        public RefreshTokenService(
            Identitydbcontext identityContext,
            ITokenService tokenService,
            UserManager<AppUser> userManager,
            Identitydbcontext appdbcontext) 
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _appdbcontext = appdbcontext;
            _identityContext = identityContext;
            
        }

        public async Task<LoginResponseDto?> RefreshAsync(string refreshToken)
        {
            // 1. Hash the refresh token received from the client
            var hashedToken = _tokenService.HashToken(refreshToken);

            // 2. Find the stored refresh token
            //    and load its related user
            var storedToken =await  _identityContext.RefreshTokens.Include(x => x.User).SingleOrDefaultAsync(x => x.Token == hashedToken);

            // 3. Make sure the token exists and is active
            if (storedToken is null || !storedToken.IsActive)
                return null;

            // 4. Generate a new access token
            var accessToken = await _tokenService.CreateTokenAsync(storedToken.User);

            // 5. Generate a new refresh token
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            // 6. Hash the new refresh token before storing it
            var hashedNewRefreshToken =
                _tokenService.HashToken(newRefreshToken);

            // 7. Revoke the old refresh token
            storedToken.RevokedOn = DateTime.UtcNow;

            // 8. Create the new refresh token
            var refreshTokenEntity = new RefreshToken
            {
                Token = hashedNewRefreshToken,
                UserId = storedToken.UserId,
                ExpiresOn = storedToken.ExpiresOn,
                CreatedOn = DateTime.UtcNow
            };

            // 9. Store the new refresh token
            _identityContext.RefreshTokens.Add(refreshTokenEntity);

            // 10. Save everything
            await _identityContext.SaveChangesAsync();

            // 11. Return the new tokens
            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken,
                User = new UserDto
                {
                    Email = storedToken.User.Email!,
                    DisplayName = storedToken.User.DisplayName
                }

            };
        }

        public async Task<bool> RevokeRefreshTokenAsync(string userId)
        {
            var user = await _userManager.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return false;

            foreach (var refreshToken in user.RefreshTokens)
            {
                if (refreshToken.IsActive)
                {
                    refreshToken.RevokedOn = DateTime.UtcNow;
                }
            }

            await _appdbcontext.SaveChangesAsync();

            return true;
        }
    }
    
}
