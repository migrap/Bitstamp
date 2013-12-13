using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Bitstamp;
using PusherClient;
using System.Reactive.Linq;
using Newtonsoft.Json;

namespace Sandbox {
    class MainClass {
        public static void Main(string[] args) {
            var bitstamp = new BitstampClient();

            bitstamp.ConnectAsync().Wait();

            bitstamp.GetTickerObservable()
                .Subscribe(x=>Console.WriteLine(x));

            //bitstamp.GetTradesObservable()
            //    .Subscribe(x => Console.WriteLine(x));

            //bitstamp.GetOrderBookObservable()
            //    .Subscribe(x => Console.WriteLine(x));

            Console.ReadLine();
        }
    }
}