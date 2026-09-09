using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopHub.Application.Interfaces;
using ShopHub.Application.Interfaces.IAccount;
using ShopHub.Application.Interfaces.IBasket;
using ShopHub.Application.Interfaces.ICatalog;
using ShopHub.Application.Mapping;
using ShopHub.Application.Services.BasketService;
using ShopHub.Application.Services.CatalogServices;
using ShopHub.Domain.Entities.Identity;
using ShopHub.Infrastructure.Data;
using ShopHub.Infrastructure.Data.SeedData;
using ShopHub.Infrastructure.IdentityData;
using ShopHub.Infrastructure.InfraService.Account;
using ShopHub.Infrastructure.Repositories;
using ShopHub.Infrastructure.Repositories.BasketRepo;
using StackExchange.Redis;
using System.Text;


namespace ShopHub.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            #region Confgure Services

            var builder = WebApplication.CreateBuilder(args);
            
            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddScoped(typeof(IGenricRepo<>), typeof(GenricRepository<>));

            
            builder.Services.AddDbContext<AppDbContext>(
                options => options.UseSqlServer
                (builder.Configuration.GetConnectionString("DefaultConnection")
                )); // -- Registered as a Scoped

            builder.Services.AddAutoMapper(typeof(MappingProfile)); // Registered as a Singleton (IMapper)

            // -- Registering Services
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductBrandService, ProductBrandService>();
            builder.Services.AddScoped<IProductTypeService, ProductTypeService>();

            // -- Identity Database 
            builder.Services.AddDbContext<Identitydbcontext>(
                options => options.UseSqlServer
                (builder.Configuration.GetConnectionString("IdentityUserConnection")
                )); // -- Registered as a Scoped

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<Identitydbcontext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 12;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                
            });

            builder.Services.AddScoped<ITokenService, TokenService>();

            builder.Services.
                AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                AddJwtBearer(options =>
                {
                    var key = builder.Configuration["Jwt:Key"];
                    var issuer = builder.Configuration["Jwt:Issuer"];
                    var audience = builder.Configuration["Jwt:Audience"];

                    if (string.IsNullOrWhiteSpace(key))
                        throw new InvalidOperationException("JWT Key is not configured.");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(key)),

                        ValidateIssuer = true,
                        ValidIssuer = issuer,

                        ValidateAudience = true,
                        ValidAudience = audience,

                        ValidateLifetime = true,

                        ClockSkew = TimeSpan.Zero
                    };
                });
            builder.Services.AddScoped<IRefreshTokenServices , RefreshTokenService>();

            // 1. Redis connection (the basket store)
            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connectionString!);
            });

            // 2. Basket repository (Redis implementation)
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();

            // 3. Basket service (business logic layer)
            builder.Services.AddScoped<IBasketServices, Basketservices>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            #endregion

            var app = builder.Build();

            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AppDbContext>();
                await DataSeeder.Seeder(context);
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
