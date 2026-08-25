using ShopHub.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Interfaces.IAccount
{
    public interface ITokenService
    {
        Task<string> CreateTokenAsync(AppUser user);
        string GenerateRefreshToken();
        string HashToken(string token);
    }
}
