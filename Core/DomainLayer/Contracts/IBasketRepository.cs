
using DomainLayer.Models.BasketModule;

namespace DomainLayer.Contracts
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> CreateOrUpdateBasket(CustomerBasket basket, TimeSpan? timeToLive = null);
        public Task<CustomerBasket?> GetBasketByIdAsync(string id);
        public Task<bool> DeleteBasketAsync(string Key);
    }
}
