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

        private readonly string transcationId;

        public RedisDistributedLockProvder(IConnectionMultiplexer redis)
        {
            this.redis = redis;
            this.transcationId = Guid.NewGuid().ToString();
        }

        public string TransactionId => transcationId;

        public async Task<DistributedLock> Acquire(string key)
        {
            var db = redis.GetDatabase();
            var now = DateTime.Now;
            while (true)
            {
                var vaildKeyExist = await db.StringSetAsync(key,
                    TransactionId, TimeSpan.FromSeconds(30),
                    When.NotExists, CommandFlags.None);

                if (vaildKeyExist)
                {
                    return new DistributedLock(key, this);
                }

                if(await db.StringGetAsync(key) == transcationId)
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
            if(await db.StringGetAsync(distributedLock.LockKey) == TransactionId)
            {
                await db.KeyDeleteAsync(distributedLock.LockKey);
            }
        }
    }
}
