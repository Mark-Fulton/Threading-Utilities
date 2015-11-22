using System;
using ThreadInterruptedException = System.Threading.ThreadInterruptedException;
using Monitor = System.Threading.Monitor;

namespace ThreadingUtilities
{
	/// <summary>
	/// Bound channel.
	/// 
	/// This is an editation of the Channel class.
	/// This requires you to enter a limit of how many items you would like to have in the queue
	/// This uses a Semaphore to add a limit to the class, the semaphore adds a thread save count to make sure that it does not get past the limit.
	/// </summary>
	public class BoundChannel<T> : Channel<T>
    {
		protected readonly Semaphore BoundChannelSemaphore;

		/// <summary>
		/// Initializes a new instance of the <see cref="Mark.Utilities.MAC.BoundChannel`1"/> class.
		/// 
		/// This backs the Channel class and allows the functionality for the Limit.
		/// This is in a Ulong and gets ported into a Semaphore and allows this functionallity.
		/// </summary>
		/// <param name="Limit">Limit.</param>
        public BoundChannel(ulong Limit)
        {
            BoundChannelSemaphore = new Semaphore(Limit);
        }

		/// <summary>
		/// Puts the object.
		/// 
		/// This accepts the type of object that you have declared in the use of the channel.
		/// this then once the object has been given to the Queue it gives the semaphore a token to acknowledge that there is
		/// work to do.
		/// 
		/// This added code for acquiring the token will fill out the requirements of the bound channel.
		/// This will not allow you to put any objects in the queue without a token being in the semaphore
		/// </summary>
		/// <param name="Object">Object.</param>
		public override void PutObject(T Object)
        {
			BoundChannelSemaphore.AcquireToken();
			base.PutObject(Object);
        }

		/// <summary>
		/// Takes the object.
		/// 
		/// This is for an object that you want to recieve in the Queue
		/// Once in the method we acquire a token so we dont trick any other threads trying to get an object
		/// Then in a thread safe area we dequeue the queue. and return the object wanted.
		/// 
		/// This added code will allow you to when you remove an item from the queue it will allocate a token to the semaphore
		/// This will allow you to put an object back into the queue
		/// </summary>
		/// <returns>The object.</returns>
		public override T TakeObject()
        {
			T result = base.TakeObject();
			BoundChannelSemaphore.ReleaseToken();
			return result;
        }

		/// <summary>
		/// Offer the specified TimeToWait and Object.
		/// 
		/// This Method Accepts a Time to wait and the object that you want to put into the channel.
		/// This then once the Method is running checks if the Bound channel is full, Once it is full it will wait
		/// If the time waited is greater than the time spent in the channel, this will return False.
		/// </summary>
		/// <param name="TimeToWait">Time to wait.</param>
		/// <param name="Object">Object.</param>
		public virtual bool Offer(int TimeToWait,T Object)
		{
			if (BoundChannelSemaphore.TryAcquire (TimeToWait)) 
			{
				try
				{
					lock (base.ChannelLock)
					{
						base.PutObject(Object);
						return true;
					}
				}
				catch(ThreadInterruptedException)
				{
					BoundChannelSemaphore.ReleaseToken ();
                    Monitor.Pulse(base.ChannelLock);
					throw;
				}
			}
			return false;
		}
    }
}
