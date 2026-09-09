using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopHub.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Infrastructure.IdentityData
{
    public class Identitydbcontext : IdentityDbContext<AppUser>
    {
        public Identitydbcontext(DbContextOptions<Identitydbcontext> options) : base(options)
        {
            
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
    }
}
