﻿using System;

namespace ThreadingUtilities
{
	/// <summary>
	/// Latch.
	/// 
	/// Latch basically allows you to hold a line of threads until you want to release then at a certain point to continue their work.
	/// This uses a semaphore that allows you to wait until a point where you want to open the door.
	/// The threads wait at the get through method until you open the door, this allows every thread through.
	/// </summary>
	public class Latch
	{
		private readonly Semaphore LatchSemaphore;

		/// <summary>
		/// Initializes a new instance of the <see cref="Mark.Utilities.MAC.Latch"/> class.
		/// 
		/// This allows the creation of the Latch and the semaphore that is within the latch, 
		/// The semaphore will take a argument of 0, this will allow to the open door method to be effective.
		/// </summary>
		public Latch ()
		{
			LatchSemaphore = new Semaphore (0);
		}

		/// <summary>
		/// Get through Method.
		/// 
		/// This is the main method of the Latch, this will allow the thread to wait at this point. 
		/// Untill you add a token to the semaphore this will not allow you to pass.
		/// This is effective when you want to wait until a task is completed then let threads go though to another stage.
		/// </summary>
		public void GetThrough()
		{
			LatchSemaphore.AcquireToken ();
			LatchSemaphore.ReleaseToken ();
		}

		/// <summary>
		/// Opens the door.
		/// 
		/// This is the allowing method for this class, this will allow a method or thread to trip this and allow every thread through
		/// This releases a token to the semaphore and since the GoThrough method is waiting for a token, this will instantaniously let it though
		/// </summary>
		public void OpenDoor()
		{
			LatchSemaphore.ReleaseToken ();
		}
	}
}

