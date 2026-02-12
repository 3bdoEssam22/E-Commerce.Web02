using DomainLayer.Contracts;
using E_Commerce.Web.CustomMiddlewares;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text.Json;

namespace E_Commerce.Web.Extensions
{
    public static class WebApplicationRegistration
    {
        public static async Task SeedDataBase(this WebApplication app)
        {
            using var scoope = app.Services.CreateScope();
            var objectOfDataSeed = scoope.ServiceProvider.GetRequiredService<IDataSeed>();
            await objectOfDataSeed.DataSeedAsync();
            await objectOfDataSeed.IdentityDataSeedAsync();

        }

        public static IApplicationBuilder UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandlerMiddleware>();
            return app;
        }

        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    options.ConfigObject = new ConfigObject()
                    {
                        DisplayRequestDuration = true
                    };
                    options.DocumentTitle = "E-Commerce Api";
                    options.JsonSerializerOptions = new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    };
                    options.DocExpansion(DocExpansion.None);
                    options.EnableFilter();
                }
                );
            return app;
        }

    }
}
