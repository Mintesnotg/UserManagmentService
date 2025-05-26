using Infrastructure.Contracts;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class RedisCacheService(IDistributedCache redisCache) : IRedisCache
    {
        public async Task<T?> GetDataAsync<T>(string key, CancellationToken cancellation)
        {

            try
            {
                string? data = await redisCache.GetStringAsync(key, cancellation);

                if (string.IsNullOrEmpty(data))
                {
                    return default;
                }
                return JsonSerializer.Deserialize<T>(data);
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        public async Task RemoveDataAsync (string key, CancellationToken cancellation)
        {


            await redisCache.RemoveAsync(key, cancellation);

            //throw new NotImplementedException();
        }

        public async Task SetDataAsync<T>(string key, T value, CancellationToken cancellation)
        {



            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) // Set expiration time as needed
            };

            await redisCache.SetStringAsync(key, JsonSerializer.Serialize(value), options, cancellation);

            //throw new NotImplementedException();
        }
    }
}
