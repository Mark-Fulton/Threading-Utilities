using System;
using Monitor = System.Threading.Monitor;

namespace ThreadingUtilities
{
	/// <summary>
	/// Reader writer.
	/// 
	/// This class Solves a fequent problem with databases in mind. 
	/// This allows you to read multiple times with a lightswitch and allows you to write using the semaphore token.
	/// This means you can have multiple reads going on at a time and only one write at a time.
	/// This allows us to keep the integrety of the data.
	/// 
	/// This also has some fixes to this problem, there will be a number of reads that will be able to go through and when there is a write, it allows it through
	/// this is by using turnstyles to allow this through.
	/// </summary>
	public class ReaderWriter
	{
		private readonly Semaphore ReaderWriterLockSemaphore;
		private readonly Lightswitch ReaderWriterLockLightswitch;
		private readonly Semaphore ReaderBlock;
		private readonly Semaphore WriteBlock;
		private ulong NumberOfReaders;
		private readonly object ReaderWriterLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="Mark.Utilities.MAC.ReaderWriter"/> class.
		/// 
		/// This Initializes the Reader Writer. All this will do is Initialize all of the global variables for the class to work.
		/// </summary>
		public ReaderWriter ()
		{
			ReaderWriterLockSemaphore = new Semaphore (1);
			ReaderWriterLockLightswitch = new Lightswitch (ReaderWriterLockSemaphore);
			ReaderBlock = new Semaphore (1);
			WriteBlock = new Semaphore (1);
			NumberOfReaders = 0;
			ReaderWriterLock = new object ();
		}

		/// <summary>
		/// Read Acquire Method.
		/// 
		/// This allows you to acquire the token for the intent of READING only. 
		/// This works by using a lightswitch and allows to pass multiple threads for the price of only one token.
		/// 
		/// This will hold onto the token until a point where the Writer has the ReaderBlock token, otherwise will keep on reading
		/// </summary>
		public void ReadAcquire()
		{
			ReaderBlock.AcquireToken ();
			ReaderBlock.ReleaseToken ();
			lock(ReaderWriterLock)
			{
				NumberOfReaders++;
			}
			ReaderWriterLockLightswitch.Acquire ();
			lock(ReaderWriterLock)
			{
				NumberOfReaders--;
				Monitor.PulseAll (ReaderWriterLock);
			}

		}

		/// <summary>
		/// Read Release Method.
		/// 
		/// This allows you to release the thread from the pool of threads.
		/// This will use the method in Lightswitch.Release to handle this request.
		/// </summary>
		public void ReadRelease()
		{
			ReaderWriterLockLightswitch.Release ();
		}

		/// <summary>
		/// Writer Acquire Method.
		/// 
		/// Writer acquire will be used for WRITING only this is designed only to have this go through at a time.
		/// This works by using the full semaphore as the way of letting in the token.
		/// 
		/// Once the thread has started in the methd it will make sure that no Readers can interuppt and allow it to write safely.
		/// </summary>
		public void WriterAcquire()
		{
			WriteBlock.AcquireToken ();
			ReaderBlock.AcquireToken ();
			ReaderWriterLockSemaphore.AcquireToken ();
			ReaderBlock.ReleaseToken ();
		}

		/// <summary>
		/// Write release for Writing to a source.
		/// 
		/// This allows you to release the thread from the one Thread.
		/// This will use the method in the Semaphore.Release to handle this request.
		/// </summary>
		public void WriteRelease()
		{
			ReaderWriterLockSemaphore.ReleaseToken ();
			lock (ReaderWriterLock) 
			{
				if(NumberOfReaders > 0)
				{
					Monitor.Wait (ReaderWriterLock);
				}
			}
			WriteBlock.ReleaseToken ();
		}
	}
}