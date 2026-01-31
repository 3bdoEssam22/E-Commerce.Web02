using DomainLayer.Contracts;
using DomainLayer.Models.IdentityModule;
using DomainLayer.Models.ProductModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
using System.Text.Json;

namespace Persistence
{
    public class DataSeed(StoreDbContext _dbContext,
        RoleManager<IdentityRole> _roleManager,
        UserManager<ApplicationUser> _userManager,
        StoreIdentityDbContext _identityDbContext) : IDataSeed
    {
        public async Task DataSeedAsync()
        {
            try
            {
                var PendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();
                if (!PendingMigrations.Any())
                {
                    await _dbContext.Database.MigrateAsync();
                }

                if (!_dbContext.ProductTypes.Any())
                {
                    var productTypesData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\types.json");
                    var productTypes = await JsonSerializer.DeserializeAsync<List<ProductType>>(productTypesData);
                    if (productTypes is not null && productTypes.Any())
                    {
                        await _dbContext.AddRangeAsync(productTypes);
                    }
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    var productBrandData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    var productBrands = await JsonSerializer.DeserializeAsync<List<ProductBrand>>(productBrandData);
                    if (productBrands is not null && productBrands.Any())
                    {
                        await _dbContext.AddRangeAsync(productBrands);
                    }
                }

                if (!_dbContext.Products.Any())
                {
                    var productData = File.OpenRead(@"..\Infrastructure\Persistence\Data\DataSeed\products.json");
                    var products = await JsonSerializer.DeserializeAsync<List<Product>>(productData);
                    if (products is not null && products.Any())
                    {
                        await _dbContext.AddRangeAsync(products);
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                //TODO
            }
        }

        public async Task IdentityDataSeedAsync()
        {
            try
            {
                if (!_roleManager.Roles.Any())
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }

                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser()
                    {
                        Email = "Abdulrahman@gmail.com",
                        DisplayName = "Abdulrahman Essam",
                        PhoneNumber = "0123456789",
                        UserName = "3bdox.x"
                    };
                    var user02 = new ApplicationUser()
                    {
                        Email = "Abdullah@gmail.com",
                        DisplayName = "Abdullah Essam",
                        PhoneNumber = "9876543210",
                        UserName = "bodyx.x"
                    };
                    await _userManager.CreateAsync(user01, "P@ssw0rd");
                    await _userManager.CreateAsync(user02, "P@ssw0rd");

                    await _userManager.AddToRoleAsync(user01, "SuperAdmin");
                    await _userManager.AddToRoleAsync(user02, "Admin");

                }

                await _identityDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

        }
    }
}
