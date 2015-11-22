using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Mark.Utilities.MAC
{
	public class Semaphore
    {
        private ulong Count = 0;
        private Object SemaphoreLock = new Object();

        public Semaphore(ulong NumberOfTokens)
        {
			Count = NumberOfTokens;
        }

		public void Release()
        {
            lock (SemaphoreLock)
            {
				Count++;
                Monitor.PulseAll(SemaphoreLock);
            }
        }
		public void Aquire()
        {
			lock (SemaphoreLock) 
			{
				while (Count == 0) 
				{
					Monitor.Wait (SemaphoreLock);
				}
				Count--;
			}
        }
    }
}