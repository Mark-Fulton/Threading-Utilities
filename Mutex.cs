﻿using System;
using Monitor = System.Threading.Monitor;

namespace ThreadingUtilities
{
	/// <summary>
	/// Mutex.
	/// 
	/// Mutex is an expansion of a Semapore, This new piece of data allows only one token to be ever present.
	/// this is done by limiting the Release token only to one token. 
	/// If you do try to release two tokens or send the release token method twice, it will error out
	/// Acquire token still works in the same vain as a Semaphore and a Mutex can be used as a Lock.
	/// </summary>
	public class Mutex : Semaphore
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Mark.Utilities.MAC.Mutex"/> class.
		/// 
		/// Initialises a new Mutex class and passes a variable to the inherited Semaphore class of 0 tokens. 
		/// This makes you release a token first to then let a thread / tool acquire it.
		/// </summary>
		public Mutex(bool Available): base(0)
		{
			if (Available) {
				base.ReleaseToken ();
			}
		}

		/// <summary>
		/// Releases the token.
		/// 
		/// This allows you to increment the tokens by ONE, If tokens release is more then one it will throw a ArgumentOutOfRangeException.
		/// This will allow a token to be able to be released so a thread will be able to acquire it.
		/// </summary>
		/// <param name="TokensToRelease">Tokens to release.</param>
		public override void ReleaseToken (ulong TokensToRelease)
		{
			lock (base.SemaphoreLock){
				if (base.TokenCount == 0){
					if (TokensToRelease == 1) {
						TokenCount++;
						Monitor.PulseAll(SemaphoreLock);
					}
					else
						throw new System.ArgumentOutOfRangeException ("Token was released when there was a token already there, This is Impossible for a Mutex to do");
				}
				else
					throw new System.ArgumentOutOfRangeException ("Token was released when there was a token already there, This is Impossible for a Mutex to do");

			}
		}
	}
}