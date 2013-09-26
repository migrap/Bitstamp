using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Bitstamp;

namespace Sandbox {
    class MainClass {
        public static void Main(string[] args) {
            var bitstamp = new BitstampClient();
            bitstamp.TradeOccured += (s, t) => Console.WriteLine(t);

            bitstamp.ConnectAsync().Wait();
            Console.ReadLine();
        }
    }
}