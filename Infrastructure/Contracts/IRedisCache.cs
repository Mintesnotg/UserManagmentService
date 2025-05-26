using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Contracts
{
    public interface IRedisCache
    {

        Task<T?> GetDataAsync<T>(string key, CancellationToken cancellation);
        Task SetDataAsync<T>(string key, T value, CancellationToken cancellation);
        Task RemoveDataAsync(string key, CancellationToken cancellation);
    }
}
