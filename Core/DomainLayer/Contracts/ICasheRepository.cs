namespace DomainLayer.Contracts
{
    public interface ICasheRepository
    {
        Task<string?> GetAsync(string CasheKey);

        Task SetAsync(string CasheKey, string CasheValue, TimeSpan TimeToLive);
    }
}
