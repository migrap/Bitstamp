using Bitstamp.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bitstamp {
    public partial class BitstampClient {
        public Task<ConversionRate> GetConversionRateAsync() {
            return _client.SendAsync(x => x
                .Method(HttpMethod.Get)
                .Address("eur_usd")
            ).ReadAsAsync<ConversionRate>(_formatter);
        }

        public Task<Ticker> GetTickerAsync() {
            return _client.SendAsync(x => x
                .Method(HttpMethod.Get)
                .Address("ticker")
            ).ReadAsAsync<Ticker>(_formatter);
        }
        public Task<OrderBook> GetOrderBookAsync(int count = 0) {
            return _client.SendAsync(x => x
                .Method(HttpMethod.Get)
                .Address("order_book")
            ).ReadAsAsync<OrderBook>(_formatter);
        }

        //[{"date": "1390438512", "tid": 3177360, "price": "817.06", "amount": "2.52640385"}]
        public Task<Transactions> GetTransactionsAsync() {
            return _client.SendAsync(x => x
                .Method(HttpMethod.Get)
                .Address("transactions")
            ).ReadAsAsync<Transactions>(_formatter);
        }

        private static OrderBook OrderBook(JObject value, int count) {
            var result = new OrderBook();
            result.Bids.AddRange(OrderBook(value, "bids", count));
            result.Asks.AddRange(OrderBook(value, "asks", count));
            return result;
        }

        private static IEnumerable<Level> OrderBook(JObject value, string name, int count) {
            var array = value[name].OfType<JArray>();
            array = (count == 0) ? array : array.Take(count);
            return array.Select(Level);
        }

        private static Level Level(JArray value) {
            var price = double.Parse(value[0].ToString());
            var volume = double.Parse(value[1].ToString());

            return new Level {
                Price = price,
                Volume = volume,
            };
        }
    }
}