using System;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace BookMyHsrp.Redis;

public class ConnectionHelper : IDisposable
{
    private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

    public ConnectionHelper(IOptions<RedisConnectionString> options) 
    {
        _lazyConnection = new Lazy<ConnectionMultiplexer>(() => CreateConnection(options));
    } 

    private ConnectionMultiplexer CreateConnection(IOptions<RedisConnectionString> options)
    {
        var connectionString = options.Value.ConnectionString;
        return ConnectionMultiplexer.Connect(connectionString);
    }

    public ConnectionMultiplexer Connection => _lazyConnection.Value;

    public void Dispose()
    {
        if (_lazyConnection.IsValueCreated)
        {
            _lazyConnection.Value.Dispose();
        }
    }
}