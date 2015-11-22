//using System;
//using System.Collections.Generic;
//using System.Threading;
//
//namespace Mark.Utilities.MAC
//{
//	/// <summary>
//	/// Rendezvous.
//	/// </summary>
//	public class Rendezvous<t>
//	{
//		private readonly Semaphore RendezvousSemaphoreT1;
//		private readonly Semaphore RendezvousSemaphoreT2;
//		private T ObjectHolderT1;
//		private T ObjectHolderT2;
//
//		/// <summary>
//		/// Initializes a new instance of the <see cref="Mark.Utilities.MAC.Rendezvous`1"/> class.
//		/// 
//		/// 
//		/// </summary>
//		public Rendezvous ()
//		{
//			RendezvousSemaphoreT1 = new Semaphore(2);
//			RendezvousSemaphoreT2 = new Semaphore(0);
//		}
//
//		/// <summary>
//		/// Exchange the specified Object.
//		/// </summary>
//		/// <param name="Object">Object.</param>
//		public t Exchange(t Object)
//		{
//
//			RendezvousSemaphoreT1.AcquireToken ();
//
//
//			if (ObjectHolderT1.Equals(null)) 
//			{
//				ObjectHolderT1 = Object;
//				RendezvousSemaphoreT2.AcquireToken ();
//				t ReturnObject = ObjectHolderT2;
//				return ReturnObject;
//			} 
//			else 
//			{
//				ObjectHolderT2 = Object;
//				RendezvousSemaphoreT2.ReleaseToken ();
//				RendezvousSemaphoreT1.ReleaseToken (2);
//				t ReturnObject = ObjectHolderT1;
//				return ReturnObject;
//			}
//		}
//	}
//}
//
