using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.Utilities.MAC
{
	public class Channel<T>
    {
		Semaphore ChannelSemaphore = new Semaphore(0);
		Queue<T> WorkQueue = new Queue<T>();
        Object ChannelLock = new Object();

		public void putObject(T Object)
        {
            lock(ChannelLock)
            {
				WorkQueue.Enqueue(Object);
				ChannelSemaphore.Release();
            }
        }

		public T takeObject()
        {
			ChannelSemaphore.Aquire();
            lock(ChannelLock)
            {
                return WorkQueue.Dequeue();
            }
        }
    }
}