using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BookMyHsrp.Redis
{
    public class CacheService
    {
        private readonly IDatabase _db;

        public CacheService(IDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<T> GetDataAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key).ConfigureAwait(false);
            return string.IsNullOrEmpty(value) ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<bool> SetDataAsync<T>(string key, T value, DateTimeOffset expirationTime)
        {
            TimeSpan ttl = expirationTime.Subtract(DateTimeOffset.Now);
            return await _db.StringSetAsync(key, JsonConvert.SerializeObject(value), ttl).ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key).ConfigureAwait(false);
        }

        public async Task<bool> RemoveDataAsync(string key)
        {
            return await _db.KeyExistsAsync(key).ConfigureAwait(false) &&
                   await _db.KeyDeleteAsync(key).ConfigureAwait(false);
        }


        public async Task HashDeleteAsync(string key, string hashField)
        {
            await _db.HashDeleteAsync(key, hashField).ConfigureAwait(false);
        }

        public async Task<bool> HashExistsAsync(string key, string hashField)
        {
            return await _db.HashExistsAsync(key, hashField).ConfigureAwait(false);
        }

        public async Task<T> HashGetAsync<T>(string key, string hashField)
        {
            var value = await _db.HashGetAsync(key, hashField).ConfigureAwait(false);
            return string.IsNullOrEmpty(value) ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task<bool> HashSetAsync<T>(string key, string hashField, T value)
        {
            return await _db.HashSetAsync(key, hashField, JsonConvert.SerializeObject(value)).ConfigureAwait(false);
        }

        public async Task<long> HashLengthAsync(string key)
        {
            return await _db.HashLengthAsync(key).ConfigureAwait(false);
        }

        public async Task<RedisValue[]> HashKeysAsync(string key)
        {
            return await _db.HashKeysAsync(key).ConfigureAwait(false);
        }

        public async Task<RedisValue[]> HashValuesAsync(string key)
        {
            return await _db.HashValuesAsync(key).ConfigureAwait(false);
        }

        public async Task<HashEntry[]> HashGetAllAsync(string key)
        {
            return await _db.HashGetAllAsync(key).ConfigureAwait(false);
        }

        //Scan Keys with pattern using keys method and return result as T
        public async Task<T> ScanKeysAsync<T>(string pattern)
        {
            var result = await _db.ScriptEvaluateAsync(
                LuaScript.Prepare(
                    "return redis.call('KEYS', @pattern)"),
                new { pattern = pattern });
            return JsonConvert.DeserializeObject<T>(result.ToString());
        }

        //Scan Keys with pattern using keys method and return value
        public async Task<RedisValue[]> ScanKeysAsync(string pattern)
        {
            var result = await _db.ScriptEvaluateAsync(
                LuaScript.Prepare(
                    "return redis.call('KEYS', @pattern)"),
                new { pattern = pattern });
            return (RedisValue[])result;
        }
    }
}