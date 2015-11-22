using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Mark.Utilities.MAC
{
	public class StrongSemaphore :Semaphore
	{
		Queue<int> NumberOfThreads;
		public StrongSemaphore ():base(0)
		{
			NumberOfThreads = new Queue<int>();
		}

		public override void ReleaseToken ()
		{
			base.ReleaseToken ();
		}

		public override void AcquireToken ()
		{
			base.AcquireToken ();
		}
	}
}

