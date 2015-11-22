using System;
using Monitor = System.Threading.Monitor;
using Thread = System.Threading.Thread;
using ThreadInterruptedException = System.Threading.ThreadInterruptedException;

namespace ThreadingUtilities
{
	/// <summary>
	/// Semaphore.
	/// 
    /// THREAD ABORT IS NOT SUPPORTED
    /// THREAD PAUSE IS NOT SUPPORTED
    /// 
	/// This code allows you to communicate to various threads and allowing access and releasing access to certain pieces of code
	/// In software, a semaphore is a data structure that is useful for solving a variety of synchronization problems.
	/// When a thread decrements the semaphore, if the result is negative, 
	/// the thread blocks itself and cannot continue until another thread increments the semaphore.
	/// When a thread increments the semaphore, if there are other threads wait- ing, one of the waiting threads gets unblocked
	/// </summary>
	public class Semaphore
    {
		protected ulong TokenCount;
		protected readonly Object SemaphoreLock;
        private ulong NumberOfThreads;

		/// <summary>
		/// Initializes a new instance of the <see cref="Mark.Utilities.MAC.Semaphore"/> class.
		/// 
		/// Initialisation of the tokencount is here, for a Semaphore that you want to acquire some tokens without releasing, put the Number in.
		/// Once all released and token count = 0 this will wait for you to release more tokens to allow more threads to acquire.
		/// </summary>
		/// <param name="NumberOfTokens">Number of tokens.</param>
        public Semaphore(ulong NumberOfTokens)
        {
			TokenCount = NumberOfTokens;
            NumberOfThreads = 0;
			SemaphoreLock = new Object();
        }

		/// <summary>
		/// Acquires the token.
		/// 
		/// The thread blocks itself and cannot continue until another thread increments the semaphore.
		/// this is done by waiting for the token count to be more then one and once it is decreases the Token count.
		/// To Increment the Token count Please use the ReleaseToken() method of this class. If you need to release more then one 
		/// Put the number of tokens you want to release in the parameters
		/// </summary>
		public virtual void AcquireToken()
		{
            TryAcquire(-1);
		}

		/// <summary>
		/// Releases the token.
		/// 
		/// This releases one token for the semaphore
		/// This also relies on the other overloaded release token method, more will be explained there.
		/// </summary>
		public virtual void ReleaseToken()
		{
			ReleaseToken (1);
		}

		/// <summary>
		/// Releases the token.
		/// 
		/// This allows you to increment the tokens by the given number inputted, being TokensToRelease
		/// This will allow a token to be able to be released so a thread will be able to acquire it. 
		/// </summary>
		/// <param name="TokensToRelease">Tokens to release.</param>
		public virtual void ReleaseToken(ulong TokensToRelease)
		{
			lock (SemaphoreLock)
			{
				TokenCount += TokensToRelease;
                if(NumberOfThreads > 0)
                {
                    for (ulong i = 0; i < TokensToRelease; i++)
                    {
                        Monitor.Pulse(SemaphoreLock);
                    }
                }
			}
		}

		/// <summary>
		/// Forces the release.
		/// 
		/// What this method tries to do is make an unbreakable way to release a set amount of tokens. this cannot be interuppted by the 
		/// ThreadInterruptedException, except it logs that this has happened and then
		/// </summary>
		public virtual void ForceRelease(ulong TokensToRelease)
        {
			bool Interuppted = false;
            while(true)
            {
                try
                {
					ReleaseToken(TokensToRelease);
					break;
                }
				catch(ThreadInterruptedException)
                {
					Interuppted = true;
                }
            }
			if (Interuppted) 
			{
                Monitor.Pulse(SemaphoreLock);
                Thread.CurrentThread.Interrupt();
			}
        }

		/// <summary>
		/// Tries the acquire.
		/// 
		/// What this Tries to do is sees it the token for the semaphore is available, if it is not however, it waits for the 
		/// Token to be available, This then waits for a specified amount of time, this is provided by the user as a integer field
		/// If it acquires the semaphore during this time it will return true to the calling method
		/// However if it times out of the acquire method it will return false to the user, signaling that the thread left while waiting
		/// 
		/// </summary>
		/// <returns><c>true</c>, if acquire was sucessful, <c>false</c> otherwise.</returns>
		/// <param name="MSToWait">MS to wait.</param>
        public virtual bool TryAcquire(int MSToWait)
        {
            lock(SemaphoreLock)
            {
                try
                {
					DateTime QuitThreadTime = DateTime.Now.AddMilliseconds(MSToWait);
					NumberOfThreads++;
					while (TokenCount == 0)
					{
						TimeSpan ActualTime;
						if (MSToWait == -1)
						{
							ActualTime = new TimeSpan();
							ActualTime = ActualTime.Subtract(TimeSpan.FromMilliseconds(1.00));
						}
						else
						{
							ActualTime = QuitThreadTime - DateTime.Now;
						}
						if ((int)ActualTime.TotalMilliseconds < -1)
						{
							NumberOfThreads--;
							return false;
						}
						if (!Monitor.Wait(SemaphoreLock, (int)ActualTime.TotalMilliseconds))
						{
							NumberOfThreads--;
							return false;
						}
					}
					TokenCount--;
					NumberOfThreads--;
					return true;
				}
				catch (ThreadInterruptedException)
				{
					NumberOfThreads--;
                    Monitor.Pulse(SemaphoreLock);
					throw;
				}
            }
        }

    }
}