using Infrastructure.Dtos;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helper
{
    public class CacheService
    {


        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache=cache;
        }

        public AuthenticatedResponse GetCahcedValue(string key) {


            if (_cache.TryGetValue(key, out AuthenticatedResponse cachedvalue))
            {
                return cachedvalue;
            }
            return null;
        }

    }
}
