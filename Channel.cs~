using System;
using System.Collections.Generic;
using Monitor = System.Threading.Monitor;
using ThreadInterruptedException = System.Threading.ThreadInterruptedException;

namespace ThreadingUtilities
{
	/// <summary>
	/// Channel.
	/// 
    /// THREAD ABORT IS NOT SUPPORTED
    /// THREAD PAUSE IS NOT SUPPORTED
    /// 
	/// This allows you to add items to a queue and then remove items from a queue that it thread safe.
	/// this will take any object that you declare the type for in the channel class. 
	/// EG. channel = new Channel<int>
	/// This channel will then acept any of the type int
	/// You will use this when you need to have a shared data storage that requires multiple threads acquiring and then depositing the items.
	/// </summary>
	public class Channel<T>
    {
		private readonly Semaphore ChannelSemaphore = new Semaphore(0);
		private Queue<T> WorkQueue = new Queue<T>();
		protected readonly Object ChannelLock = new Object();

		/// <summary>
		/// Puts the object.
		/// 
		/// This accepts the type of object that you have declared in the use of the channel.
		/// this then once the object has been given to the Queue it gives the semaphore a token to acknowledge that there is work to do. 
		/// </summary>
		/// <param name="Object">Object.</param>
		public virtual void PutObject(T Object)
        {
            lock(ChannelLock)
            {
				WorkQueue.Enqueue(Object);
				ChannelSemaphore.ReleaseToken();
            }
        }

		/// <summary>
		/// Takes the object.
		/// 
		/// This is for an object that you want to recieve in the Queue
		/// Once in the method we acquire a token so we dont trick any other threads trying to get an object
		/// Then in a thread safe area we dequeue the queue. and return the object wanted.
		/// </summary>
		/// <returns>The object.</returns>
		public virtual T TakeObject()
		{
			ChannelSemaphore.AcquireToken();
			lock(ChannelLock)
			{
				return WorkQueue.Dequeue();
			}
		}

		/// <summary>
		/// Poll the specified TimeToWait and result.
		/// 
		/// What this class does is it keeps track of the time that is required to dequeue an object
		/// What this means is that if the item is under the required time that the user inputs in MS it will return true
		/// If it does not however it will return false
		/// </summary>
		/// <param name="TimeToWait">Time to wait.</param>
		/// <param name="result">Result.</param>
		public virtual bool Poll(int TimeToWait,out T result)
        {
			if (ChannelSemaphore.TryAcquire (TimeToWait)) 
			{
				try
				{
					lock (ChannelLock)
					{
						result = WorkQueue.Dequeue();
						return true;
					}
				}
				catch(ThreadInterruptedException)
				{
					ChannelSemaphore.ReleaseToken ();
                    Monitor.Pulse(ChannelLock);
					throw;
				}
			}
			result = default(T);
			return false;
        }
    }
}