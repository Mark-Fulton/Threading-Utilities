using System;

namespace ThreadingUtilities
{
	/// <summary>
	/// Lightswitch.
	/// 
	/// Lightswitch class allows you to have the ability to let multiple threads go to a task without assigning multiple tokens
	/// this means one token can have more then one thread working on somthing.
	/// 
	/// this works on the premace that you will pass in a semaphore of your choice to handle the requests
	/// This lightswitch everytime it will take a token will allow multiple threads WITH REFRENCE TO THE LIGHTSWITCH to enter
	/// Once all the threads have done what they need to do, they will exit and return the token that they consumed.
	/// </summary>
	public class Lightswitch
	{
		private readonly Semaphore LightswitchSemaphore;
		private ulong ThreadCount;
		private readonly object LightswitchLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="Mark.Utilities.MAC.Lightswitch"/> class.
		/// 
		/// This allows the creation of the Lightswitch, this accepts a Semaphore as a paramater and only allows this to be passed in
		/// this is detrement to what you want to do, make sure you pick the right semaphore to pass in. 
		/// </summary>
		/// <param name="LightswitchSemaphore">Lightswitch semaphore.</param>
		public Lightswitch (Semaphore LightswitchSemaphore)
		{
			ThreadCount = 0;
			this.LightswitchSemaphore = LightswitchSemaphore;
			LightswitchLock = new object ();
		}

		/// <summary>
		/// Acquire this instance.
		/// 
		/// This is the Acquire method for the Lightswitch
		/// This will allow multiple threads to go in with only one token. This is done by the first thread acquiring and then the others going in after
		/// This is possible by using an other variable to keep track of the threads coming in.
		/// </summary>
		public void Acquire()
		{
			lock (LightswitchLock) 
			{
				if (ThreadCount == 0) 
					LightswitchSemaphore.AcquireToken ();
				ThreadCount++;
			}
		}

		/// <summary>
		/// Release this instance.
		/// 
		/// This is the release method for the lightswitch
		/// This will allow threads to leave the lightswitch without giving up the token. This is only possible when checking if there are still threads 
		/// being exicuted. after the last thread leaves, the Lightswitch will give up the token.
		/// </summary>
		public void Release()
		{
			lock (LightswitchLock) 
			{
				if (ThreadCount == 0) {
                    throw new System.ArgumentOutOfRangeException("Token was released when there was no threads in the lightswitch. therefore no Semaphore token");
				}
				ThreadCount--;
				if (ThreadCount == 0)
					LightswitchSemaphore.ReleaseToken ();
			}
		}

	}
}