using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading.Tasks;
using SocketIOClient;
using PusherClient;

namespace Sandbox {
	class MainClass {
		public static void Main (string[] args) {
            var pusher = new Pusher("de504dc5763aeef9ff52");
            var channel = (Channel)null;
            pusher.Connected += (s) => {
                Console.WriteLine("Connected");
                channel = pusher.Subscribe("live_orders");
                channel.Subscribed += channel_Subscribed;
                
            };
            pusher.ConnectionStateChanged += (s, cs) => Console.WriteLine(cs);

            pusher.Connect();

            Console.ReadLine();

			(new AutoResetEvent (false)).WaitOne();
		}

        static void channel_Subscribed(object sender) {
            var channel = sender as Channel;
            channel.Bind("order_changed", (d) => {
                Console.WriteLine("order_changed: {0}", d.ToString());
            });
        }
	}
}