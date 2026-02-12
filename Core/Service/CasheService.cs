using DomainLayer.Contracts;
using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    internal class CasheService(ICasheRepository _casheRepository) : ICasheService
    {
        public Task<string?> GetAsync(string CasheKey) => _casheRepository.GetAsync(CasheKey);

        public async Task SetAsync(string CasheKey, object CasheValue, TimeSpan TimeToLive)
        {
            var value = JsonSerializer.Serialize(CasheValue);
            await _casheRepository.SetAsync(CasheKey, value, TimeToLive);
        }
    }
}
