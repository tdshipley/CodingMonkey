namespace CodingMonkey.Models.Repositories
{
    using Microsoft.Extensions.Caching.Memory;

    public interface IRepository
    {
        IMemoryCache MemoryCache { get; set; }
        CodingMonkeyContext CodingMonkeyContext { get; set; }
    }
}
