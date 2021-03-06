﻿using System;
using System.Threading;

namespace ThreadingUtilities
{
	/// <summary>
	/// Exchange.
	/// 
	/// This code allows you to exchange objects between threads safely without the need of a channel or a bound channel.
	/// What you will need to do is make sure that you declare the Exchange to the type that you need and
	/// Then use the Exchange method to exchnge data between threads. This Exchange method is re-useable and accepts two threads at a time
	/// After the data exchanges it will accept another two threads to exchange.
	/// </summary>
	public class Exchange<T>
	{
		private readonly Semaphore ThreadLimiter;
		private readonly Semaphore ThreadOrganiser;
		private readonly Semaphore ThreadSignaler;
		private T ObjectHolderT1;
		private T ObjectHolderT2;
		private bool isFirst;

		/// <summary>
		/// Initializes a new instance of the <see cref="Mark.Utilities.MAC.Exchange`1"/> class.
		/// 
		/// Initializes the Semaphores that are used in the Exchange there is no Passed in variables so it is able to be called and used instantly.
		/// </summary>
		public Exchange ()
		{
			ThreadLimiter = new Semaphore(2);
			ThreadOrganiser = new Semaphore(0);
			ThreadSignaler = new Semaphore(1);
			ObjectHolderT1 = default(T);
			ObjectHolderT2 = default(T);
			isFirst = true;
		}
        
		/// <summary>
		/// Exchange the specified Object.
		/// 
		/// This Method is where all the work happends, This will only accept two threads at a time and until the process is complete and returning data
		/// it will halt all of the threads that enter the Method, This Will then allow one thread to check if there is data and if there isnt add data
		/// This will then signal the second thread to start. This will then run the check for the data, But there is some due to the first thread.
		/// This will then Take that data deposited into a new variable so it can be cleared before it returns. And then reset the semaphores to accept 
		/// Another two threads. The last being the semaphore that is blocking the two threads.
		/// </summary>
		/// <param name="Object">Object.</param>
		public T ExchangeItem(T Object)
		{ 
			ThreadLimiter.AcquireToken ();
			ThreadSignaler.AcquireToken ();
			if (isFirst) 
			{
				ObjectHolderT1 = Object;
                isFirst = false;
				ThreadSignaler.ReleaseToken();
				ThreadOrganiser.AcquireToken();
                isFirst = true;
				ThreadSignaler.ReleaseToken();
				return ObjectHolderT2;
			}
			ObjectHolderT2 = Object;
			ThreadOrganiser.ReleaseToken ();
			ThreadLimiter.ReleaseToken (2);
			return ObjectHolderT1;
		}
	}
}

