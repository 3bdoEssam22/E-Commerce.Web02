
namespace ServiceAbstraction
{
    public interface ICasheService
    {
        Task<string?> GetAsync(string CasheKey);
        Task SetAsync(string CasheKey, object CasheValue, TimeSpan TimeToLive);
    }
}
