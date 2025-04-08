using Authorize.Application.Common.Intefaces;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Authorize.Infastructure.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache cache;

        private JsonSerializerOptions serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = null,
            WriteIndented = true,
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public CacheService(
            IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<Result<T>> GetData<T>(string key)
        {
            var jsonData = await cache.GetStringAsync(key);

            if(jsonData is null)
            {
                return Result.Failure<T>("Data doesnt exist");
            }

            var data = JsonSerializer.Deserialize<T>(jsonData, serializerOptions);

            if (data is null)
            {
                return Result.Failure<T>("Data is incorrect");
            }

            return Result.Success(data);
        }

        public async Task RemoveData(string key)
        {
            await cache.RemoveAsync(key);
        }

        public async Task SetData<T>(string key, T value, int expiratingTime)
        {
            var jsonData = JsonSerializer.Serialize(value, serializerOptions);

            var cacheoptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expiratingTime)
            };

            await cache.SetStringAsync(key, jsonData, cacheoptions);
        }
    }
}
