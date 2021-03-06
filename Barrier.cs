﻿using System;

namespace ThreadingUtilities
{
	/// <summary>
	/// Barrier.
	/// 
	/// Barrier's allow you to specify a number and this number is the amount of threads allowed through
	/// This works by using two different turnstyles and allows a number of threads to enter, and once it reaches the number, goes through the Second.
	/// Turnstyle. This makes sure that the number of threads that have been specified is the number that exits.
	/// This will provide an output in a boolean state.
	/// </summary>
	public class Barrier
	{
		private ulong Count;
		private readonly ulong Limit;
		protected readonly object BarrierLock;
		private readonly Semaphore Turnstyle;
		private readonly Semaphore Turnstyle2;

		/// <summary>
		/// Initializes a new instance of the <see cref="Mark.Utilities.MAC.Barrier"/> class.
		/// 
		/// This is the constructor for the Barrier class, This accepts a uLong as the limit to the barrier
		/// Please take note this will be the limit of the Threads that you want to go through at a time.
		/// </summary>
		/// <param name="Limit">Limit.</param>
		public Barrier (ulong Limit)
		{
			Count = 0;
			this.Limit = Limit; 
			BarrierLock = new object ();
			Turnstyle = new Semaphore (Limit);
			Turnstyle2 = new Semaphore (0);
		}

		/// <summary>
		/// Arrive this instance.
		/// 
		/// This is the Main method of the Barrier class. 
		/// This uses the two Turnstyles to get the desired result. It also returns a boolean result of the thread that set the start of the process.
		/// 
		/// The main difference is that the first turnstyle counts till the limit and sets the result area
		/// Once it reaches the Boolean value it fires off the second turnstyle. to make sure that there are no other threads entering and throwing out the 
		/// balance. once it reaches this it returns the boolean value.
		/// </summary>
		public bool Arrive()
		{
			Turnstyle.AcquireToken ();
			lock(BarrierLock)
			{
				Count++;
				if (Count == Limit) 
				{
					Count--;
					Turnstyle2.ReleaseToken (Limit - 1);
					return true;
				}
			}
			Turnstyle2.AcquireToken ();
			lock (BarrierLock) 
			{
				Count--;
				if (Count == 0) 
				{
					Turnstyle.ReleaseToken (Limit);
				}
			}
			return false;
		}
	}
}