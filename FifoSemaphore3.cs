//using System;
//using System.Collections.Generic;
//using System.Threading;
//
//namespace Mark.Utilities.MAC
//{
//	public class FifoSemaphore3 :Semaphore
//	{
//		private readonly Queue<Semaphore> SemaphoreQueue;
//		private readonly object FifoSemaphoreObject = new object ();
//
//		public FifoSemaphore3 (ulong NumberOfTokens): base(NumberOfTokens)
//		{
//			SemaphoreQueue = new Queue<Semaphore> ();
//		}
//
//		public override void AcquireToken ()
//		{
//			Semaphore Sem = new Semaphore(0);
//			lock (FifoSemaphoreObject) {
//				Console.WriteLine (SemaphoreQueue.Count + ": SemQueue | > 0 ");
//				if (SemaphoreQueue.Count < 0) {
//					base.TokenCount--;
//					return;
//				}
//				Sem.AcquireToken ();
//			}
//			SemaphoreQueue.Enqueue (Sem);
//		}
//
//		public override void ReleaseToken()
//		{
//			ReleaseToken (1);
//		}
//
//		public override void ReleaseToken(ulong NumberOfTokens)
//		{
//			Semaphore Sem;
//			lock (FifoSemaphoreObject){
//				if (SemaphoreQueue.Count == 0){
//					base.TokenCount+= NumberOfTokens;
//					return;
//				}
//				Sem = SemaphoreQueue.Dequeue();
//			}
//			Sem.ReleaseToken();
//		}
//
//
//	}
//}
//
