using DomainLayer.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Data;
using Persistence.Repositories;
using Service;
using Service.MappingProfiles;
using ServiceAbstraction;
using System.Reflection;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDataSeed, DataSeed>();
            builder.Services.AddAutoMapper(typeof(ServiceAssemblyReference).Assembly);
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();

            #endregion

            var app = builder.Build();

            using var scoope = app.Services.CreateScope();
            var objectOfDataSeed = scoope.ServiceProvider.GetRequiredService<IDataSeed>();
            await objectOfDataSeed.DataSeedAsync();

            #region Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            #endregion            

            app.Run();
        }
    }
}
