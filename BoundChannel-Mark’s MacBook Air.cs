using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.Utilities.MAC
{
	public class BoundChannel<T> : Channel<T>
    {
		public readonly Semaphore BoundChannelSemaphore;
        public BoundChannel(ulong Limit)
        {
            BoundChannelSemaphore = new Semaphore(Limit);
        }

		public void putObjectIn(T Object)
        {
			BoundChannelSemaphore.Aquire();
			base.putObject(Object);
        }

		public T takeObjectOut()
        {
			BoundChannelSemaphore.Release();
            return base.takeObject();
        }
    }
}