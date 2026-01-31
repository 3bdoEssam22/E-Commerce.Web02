
using DomainLayer.Contracts;
using DomainLayer.Models.BasketModule;
using StackExchange.Redis;
using System.Text.Json;

namespace Persistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();

        public async Task<CustomerBasket?> CreateOrUpdateBasket(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);
            var isCreated = await _database.StringSetAsync(basket.Id, jsonBasket, timeToLive ?? TimeSpan.FromDays(30));
            if (isCreated)
                return await GetBasketByIdAsync(basket.Id);
            else
                return null;
        }

        public async Task<bool> DeleteBasketAsync(string Key) => await _database.KeyDeleteAsync(Key);

        public async Task<CustomerBasket?> GetBasketByIdAsync(string id)
        {
            var basket = await _database.StringGetAsync(id);
            if (basket.IsNullOrEmpty)
                return null;
            else
                return JsonSerializer.Deserialize<CustomerBasket>(basket!);

        }
    }
}
