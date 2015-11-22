using System;
using Monitor = System.Threading.Monitor;

namespace ThreadingUtilities
{
	/// <summary>
	/// Fifo semaphore.
	/// 
	/// FIFO semaphore allows a First in First out queue senario. This Backs of the Semaphore class however has the extra security of being first in first out
	/// this is achieved by using a number as the declaration of the thread trying to access the semaphore
	/// This allows the first in and first out senario
	/// </summary>
	public class FifoSemaphore :Semaphore
	{
		private ulong ThreadCount;
		private ulong ThreadNumber;
		private readonly object FIFOLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="Mark.Utilities.MAC.FifoSemaphore"/> class.
		/// 
		/// This accepts a ulong number as the number of tokens that are in the base semaphore class so you can then create the FIFO semaphore 
		/// With the amount of desired tokens. This Constructor also creates the ulongs for the threadcount and the ThreadNumber 
		/// </summary>
		/// <param name="NumberOfTokens">Number of tokens.</param>
		public FifoSemaphore (ulong NumberOfTokens): base(NumberOfTokens)
		{
			ThreadCount = 0;
			ThreadNumber = 0;
			FIFOLock = new object ();
		}

		/// <summary>
		/// Acquires the token.
		/// 
		/// The thread blocks itself and cannot continue until another thread increments the semaphore.
		/// this is done by waiting for the token count to be more then one and once it is decreases the Token count.
		/// To Increment the Token count Please use the ReleaseToken() method of this class. If you need to release more then
		/// one 
		/// Put the number of tokens you want to release in the parameters
		/// 
		/// This Method also allows the FIFO technology, 
		/// </summary>
		public override void AcquireToken ()
		{
			ulong ThreadIndNumber;
			lock (FIFOLock) 
			{
				ThreadIndNumber = ThreadCount;
				ThreadCount++;
			}
			lock (FIFOLock) 
			{
				while (ThreadIndNumber != ThreadNumber) 
				{
					Monitor.Wait (FIFOLock);
				}
			}
			base.AcquireToken ();
			lock (FIFOLock) 
			{
				ThreadNumber++;
				Monitor.PulseAll (FIFOLock);
			}
		}
	}
}

