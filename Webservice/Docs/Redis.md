
### Install locally
1. [Download Memurai ad install on windows](https://www.memurai.com/get-memurai?version=windows-valkey)
2. [Download Redis insights](https://redis.io/downloads/#Redis_Insight)
3. Redis is using `127.0.0.1:6379`
4. Open powershell and run `redis-cli` to test installation completed
5. Open redis insights and add new database. `redis://default@127.0.0.1:6379`

<hr/>

### Redis portal

[Portal](https://cloud.redis.io/#/databases/13862414/subscription/3054029/view-bdb/configuration)

<hr/>

## Redis Cli
```
redis-cli -u redis://default:XHnKreW3MCqKxlsHOayZOEGFihyPFz1f@redis-16147.c282.east-us-mz.azure.cloud.redislabs.com:16147
```

## C#
````
using StackExchange.Redis;

public class ConnectBasicExample
{

    public void run()
    {
        var muxer = ConnectionMultiplexer.Connect(
            new ConfigurationOptions{
                EndPoints= { {"redis-16147.c282.east-us-mz.azure.cloud.redislabs.com", 16147} },
                User="default",
                Password=""
            }
        );
        var db = muxer.GetDatabase();
        
        db.StringSet("foo", "bar");
        RedisValue result = db.StringGet("foo");
        Console.WriteLine(result); // >>> bar
        
    }
}

```