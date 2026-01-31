using Shared.DataTtansferObjects.BasketModuleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IBasketService
    {
        public Task<BasketDto> GetBasketAsync(string key);
        public Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basket);
        public Task<bool> DeleteBasketAsync(string key);
    }
}
