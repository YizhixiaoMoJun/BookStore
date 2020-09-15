using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shirley.Book.Service.Domains
{
    public interface IDistributedLockProvder
    {
        string TransactionId { get; }
        Task<DistributedLock> Acquire(string key);
        Task Release(DistributedLock distributedLock);
    }

    public class DistributedLock : IDisposable
    {
        private readonly IDistributedLockProvder distributedLockProvder;

        public string LockKey { get; }

        public DistributedLock(string key, IDistributedLockProvder distributedLockProvder)
        {
            this.LockKey = key;
            this.distributedLockProvder = distributedLockProvder;
        }

        public void Dispose()
        {
            distributedLockProvder.Release(this);
        }
    }
}
