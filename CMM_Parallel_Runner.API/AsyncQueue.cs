using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CMM_Parallel_Runner.API
{
    public class AsyncQueue<T> : IDisposable
    {
        private readonly Queue<T> _queue = new Queue<T>();
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(initialCount: 0);

        public void Enqueue(T item)
        {
            lock (_queue)
            {
                _queue.Enqueue(item);
            }

            _semaphore.Release();
        }

        public async Task<T> DequeueAsync(CancellationToken cancellationToken = default)
        {
            await _semaphore.WaitAsync(cancellationToken);

            lock (_queue)
            {
                return _queue.Dequeue();
            }
        }

        public int Count
        {
            get
            {
                lock (_queue)
                {
                    return _queue.Count;
                }
            }
        }

        public void Dispose()
        {
            _semaphore?.Dispose();
        }
    }
}