using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.Domains
{
    public class RedisDistributedLockProvder : IDistributedLockProvder
    {
        private readonly IConnectionMultiplexer redis;

        public RedisDistributedLockProvder(IConnectionMultiplexer redis)
        {
            this.redis = redis;
        }

        public string TransactionId => throw new NotImplementedException();

        public async Task<DistributedLock> Aquire(string key)
        {
            var db = redis.GetDatabase();
            var now = DateTime.Now;
            while (true)
            {
                var vaildKeyExist = await db.StringSetAsync(key,
                    30, TimeSpan.FromSeconds(30),
                    When.NotExists, CommandFlags.None);

                if (vaildKeyExist)
                {
                    return new DistributedLock(key, this);
                }

                if (now.AddSeconds(3) < DateTime.Now)
                {
                    break;
                }

                await Task.Delay(500);
            }

            throw new DomainException(500, $"Aquire key->{key} is time out");
        }

        public async Task Release(DistributedLock distributedLock)
        {
            var db = redis.GetDatabase();
            await db.KeyDeleteAsync(distributedLock.LockKey);
        }
    }
}
