using DomainLayer.Contracts;
using StackExchange.Redis;

namespace Persistence.Repositories
{
    public class CasheRepository(IConnectionMultiplexer connection) : ICasheRepository
    {
        private readonly IDatabase _database = connection.GetDatabase();

        public async Task<string?> GetAsync(string CasheKey)
        {
            var cachedValue = await _database.StringGetAsync(CasheKey);
            return cachedValue.IsNullOrEmpty ? null : cachedValue.ToString();
        }

        public async Task SetAsync(string CasheKey, string CasheValue, TimeSpan TimeToLive) => await _database.StringSetAsync(CasheKey, CasheValue, TimeToLive);
    }
}
