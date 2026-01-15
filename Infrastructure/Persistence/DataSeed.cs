using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System.Text.Json;

namespace Persistence
{
    public class DataSeed(StoreDbContext _dbContext) : IDataSeed
    {
        void IDataSeed.DataSeed()
        {
            try
            {
                if (!_dbContext.Database.GetPendingMigrations().Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    var productBrandData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\DataSeed\brands.json");
                    var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(productBrandData);
                    if (productBrands is not null && productBrands.Any())
                    {
                        _dbContext.AddRange(productBrands);
                    }
                }
                if (!_dbContext.ProductTypes.Any())
                {
                    var productTypesData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\DataSeed\Types.json");
                    var productTypes = JsonSerializer.Deserialize<List<ProductType>>(productTypesData);
                    if (productTypes is not null && productTypes.Any())
                    {
                        _dbContext.AddRange(productTypes);
                    }
                }
                if (!_dbContext.Products.Any())
                {
                    var productData = File.ReadAllText(@"..\Infrastructure\Persistence\Data\DataSeed\Products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(productData);
                    if (products is not null && products.Any())
                    {
                        _dbContext.AddRange(products);
                    }
                }
                _dbContext.SaveChanges();

            }
            catch (Exception ex)
            {

                //TODO
            }
        }
    }
}
