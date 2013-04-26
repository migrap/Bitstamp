using System;
using Bitstamp;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Sandbox {
	class MainClass {
		public static void Main (string[] args) {
			var bitstamp = new BitstampClient ();
			var ticker = bitstamp.GetTickeAsync().Result;
			var orderbook = bitstamp.GetOrderBookAsync ().Result;

			Console.WriteLine (ticker);

			(new AutoResetEvent (false)).WaitOne ();
		}
	}
}