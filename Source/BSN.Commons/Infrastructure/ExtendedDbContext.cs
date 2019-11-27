using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Commons.Infrastructure
{
    public class ExtendedDbContext : DbContext
    {
        private Queue<Func<bool>> Queue { get; set; }
        public int QueueCount => Queue.Count;

        public ExtendedDbContext()
        {
            Queue = new Queue<Func<bool>>();
        }

        public void AddToQueue(Func<bool> func)
        {
            Queue.Enqueue(func);
        }

        public Func<bool> RemoveFromQueue()
        {
            return Queue.Dequeue();
        }
    }
}
