using Bitstamp;
using System;

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