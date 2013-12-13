using Bitstamp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PusherClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstamp {
    public partial class BitstampClient {
        private Pusher _pusher;
        private Channel _orders;
        private Channel _trades;

        public event EventHandler<Order> OrderDeleted;
        public event EventHandler<Order> OrderCreated;
        public event EventHandler<Order> OrderChanged;
        public event EventHandler<Trade> TradeOccured;

        public async Task<bool> ConnectAsync(string applicationkey = "de504dc5763aeef9ff52") {
            var tcs = new TaskCompletionSource<bool>();
            _pusher = new Pusher(applicationkey);

            var connected = (ConnectedEventHandler)null;
            var orders = (SubscriptionEventHandler)null;
            var trades = (SubscriptionEventHandler)null;

            orders = (sender) => {
                _orders.Subscribed -= orders;
                _orders.Bind("order_deleted", (d) => On(OrderDeleted, (d as JObject).AsOrder()));
                _orders.Bind("order_created", (d) => On(OrderCreated, (d as JObject).AsOrder()));
                _orders.Bind("order_changed", (d) => On(OrderChanged, (d as JObject).AsOrder()));
            };

            trades = (sender) => {
                _trades.Subscribed -= trades;
                _trades.Bind("trade", (d) => On(TradeOccured, (d as JObject).AsTrade()));
            };

            _pusher.Connected += connected = (sender) => {
                _pusher.Connected -= connected;

                _trades = _pusher.Subscribe("live_trades");
                _trades.Subscribed += trades;

                _orders = _pusher.Subscribe("live_orders");
                _orders.Subscribed += orders;
            };

            _pusher.ConnectionStateChanged += (s, e) => {
            };

            _pusher.Connect();

            return await tcs.Task;
        }

        private void On<T>(EventHandler<T> handler, T value) {
            if(null != handler) {
                handler(this, value);
            }
        }
    }

    internal static partial class Extensions {
        private static JsonSerializer _serializer = new JsonSerializer();
        public static Order AsOrder(this JObject self) {
            return self.As<Order>();
        }

        public static Trade AsTrade(this JObject self) {
            return self.As<Trade>();
        }

        private static T As<T>(this JObject self) {
            return (T)_serializer.Deserialize<T>(new JTokenReader(self));
        }
    }
}