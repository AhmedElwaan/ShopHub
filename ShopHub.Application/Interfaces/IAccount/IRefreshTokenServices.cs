using ShopHub.Application.DTOs.IDentityDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Application.Interfaces.IAccount
{
    public interface IRefreshTokenServices
    {
        Task<LoginResponseDto?> RefreshAsync(string refresherToken);
    }
}
