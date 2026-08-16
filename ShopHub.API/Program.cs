using Microsoft.EntityFrameworkCore;
using ShopHub.Application.Interfaces;
using ShopHub.Application.Interfaces.ICatalog;
using ShopHub.Application.Mapping;
using ShopHub.Application.Services.CatalogServices;
using ShopHub.Infrastructure.Data;
using ShopHub.Infrastructure.Data.SeedData;
using ShopHub.Infrastructure.Repositories;


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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
