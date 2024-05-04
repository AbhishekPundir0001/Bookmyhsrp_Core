
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BookMyHsrp.Libraries
{

    public class FetchDataAndCache
    {
        // private readonly CacheService _cacheService;
        private readonly IDistributedCache _redisCache;
        public FetchDataAndCache(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
            // _cacheService = cacheService;
        }

        //Set String in DistributedCache
        public async Task SetStringInCache(string key, string value, int cacheMinutes = 20)
        {
            await _redisCache.SetStringAsync(key, value, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheMinutes)
            });
        }

        //Get String from DistributedCache
        public async Task<string> GetStringFromCache(string key)
        {
            return await _redisCache.GetStringAsync(key);
        }

        //Set Object in DistributedCache
        public async Task SetObjectInCache<T>(string key, T value, int cacheMinutes = 20)
        {
            string jsonString = JsonConvert.SerializeObject(value);
            await _redisCache.SetStringAsync(key, jsonString, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(cacheMinutes)
            });
        }

        //Get Object from DistributedCache
        public async Task<T> GetObjectFromCache<T>(string key)
        {
            var cacheExists = await _redisCache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cacheExists))
            {
                return JsonConvert.DeserializeObject<T>(cacheExists);
            }
            return default(T);
        }

        //Remove Object from DistributedCache
        public async Task RemoveObjectFromCache(string key)
        {
            await _redisCache.RemoveAsync(key);
        }

        //Get Report from Cache or Service
        public async Task<dynamic> GetReportFromCacheOrService<TRequest, TResponse>(
            TRequest requestDto,
            Func<TRequest, Task<TResponse>> serviceMethod,
            string cacheKey = "",
            int cacheMinutes = 20,
            bool useCache = false)
        {
            if (useCache)
            {
                var cacheExists = await GetStringFromCache(cacheKey);
                if (!string.IsNullOrEmpty(cacheExists))
                {
                    var resultData = TryDeserializeJson(cacheExists);
                    return resultData;
                }
            }
            return await GetFreshDataAndUpdateCache(requestDto, serviceMethod, cacheKey, cacheMinutes, useCache);
        }


        public static List<Dictionary<string, object>> TryDeserializeJson(string json)
        {
            try
            {
                var jsonArray = JArray.Parse(json);
                return jsonArray.Select(item =>
                    item.ToObject<Dictionary<string, object>>()).ToList();
            }
            catch (JsonException ex)
            {
                // Log the exception if necessary.
                // Returning an empty list upon failure.
                return new List<Dictionary<string, object>>();
            }
        }


        private async Task<TResponse> GetFreshDataAndUpdateCache<TRequest, TResponse>(
            TRequest requestDto,
            Func<TRequest, Task<TResponse>> serviceMethod,
            string cacheKey,
            int cacheMinutes, bool useCache = false)
        {
            var resultGot = await serviceMethod(requestDto);
            if (useCache)
            {
                if (resultGot != null)
                {
                    string jsonString = JsonConvert.SerializeObject(resultGot);
                    await SetStringInCache(cacheKey, jsonString, cacheMinutes);
                }
            }
            return resultGot;
        }
    }
}