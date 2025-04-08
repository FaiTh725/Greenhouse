using CSharpFunctionalExtensions;
using Greenhouse.Application.Common.Intrefaces;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Greenhouse.Infastructure.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache cache;
        private readonly IServer redisServer;

        private JsonSerializerOptions serializeOptions = new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = null,
            WriteIndented = true,   
            AllowTrailingCommas = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };


        public CacheService(
            IDistributedCache cache,
            IConnectionMultiplexer connection)
        {
            this.cache = cache;
            var endPoint = connection.GetEndPoints();
            redisServer = connection.GetServer(endPoint[0]); 
        }

        public async Task<Result<T>> GetData<T>(string key)
        {
            var jsonData = await cache.GetStringAsync(key);

            if(jsonData is null)
            {
                return Result.Failure<T>("Data doesnt exist");
            }

            var data = JsonSerializer.Deserialize<T>(jsonData, serializeOptions);

            if(data is null)
            {
                return Result.Failure<T>("Error deserialize data");
            }

            return data;
        }

        public async Task RemoveByPatern(string pattern)
        {
            var keys = redisServer.KeysAsync(pattern: pattern);

            if(keys is null)
            {
                return;
            }

            await foreach (var key in keys)
            {
                await cache.RemoveAsync(key!);
            }
        }

        public async Task RemoveData(string key)
        {
            await cache.RemoveAsync(key);
        }

        public async Task SetData<T>(string key, T value, int expirationSecond)
        {
            var jsonData = JsonSerializer.Serialize(value, serializeOptions);

            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(expirationSecond)
            };

            await cache.SetStringAsync(key, jsonData, cacheOptions);
        }
    }
}
