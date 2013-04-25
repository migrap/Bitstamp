using System;
using Bitstamp;
using System.Threading;

namespace Sandbox {
	class MainClass {
		public static void Main (string[] args) {
			var bitstamp = new BitstampClient ();
			var ticker = bitstamp.GetTickeAsync().Result;

			Console.WriteLine (ticker);

			(new AutoResetEvent (false)).WaitOne ();
		}
	}
}
