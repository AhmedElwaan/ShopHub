using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopHub.Application.Interfaces.IAccount;
using ShopHub.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Infrastructure.InfraService.Account
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configration;

        public TokenService(UserManager<AppUser> userManager , IConfiguration configration)
        {
            _userManager = userManager;
            _configration = configration;
        }
        public async Task<string> CreateTokenAsync(AppUser user)
        {
            // Claims = the "payload" data
            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.DisplayName!)

            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // The secret key 
            var key = _configration["Token:SecretKey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            
            // use HMAC-SHA256 with this key
            var credentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);

            // Build the token
            var token = new JwtSecurityToken(
                issuer: _configration["Token:Issuer"],
                audience: _configration["Token:Audience"],
                claims: userClaims,
                expires : DateTime.UtcNow.AddMinutes(double.Parse(_configration["Token:AccessTokenExpirationMinutes"])),
                signingCredentials: credentials
                
                );

            // Serialize into the final "header.payload.signature" string 
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public string HashToken(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }
    }
}
