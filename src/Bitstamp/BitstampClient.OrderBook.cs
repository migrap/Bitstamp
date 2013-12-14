using Bitstamp.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bitstamp {
	public partial class BitstampClient {
		public async Task<OrderBook> GetOrderBookAsync(int count = 0) {
			return await GetAsync ("order_book")
				.ConfigureAwait (false)
				.GetAwaiter ()
				.GetResult ()
				.Content.ReadAsAsync<JObject> (_formatter)
					.ContinueWith (x => OrderBook (x.Result, count));
		}

		private static OrderBook OrderBook(JObject value, int count){
			var result = new OrderBook ();
			result.Bids.AddRange (OrderBook (value, "bids", count));
			result.Asks.AddRange (OrderBook (value, "asks", count));
			return result;
		}

		private static IEnumerable<Level> OrderBook(JObject value, string name, int count){
			var array = value [name].OfType<JArray> ();
			array = (count == 0) ? array : array.Take (count);
			return array.Select(Level);
		}

		private static Level Level(JArray value){
			var price = double.Parse (value [0].ToString ());
			var volume = double.Parse (value [1].ToString ());

			return new Level {
				Price = price,
				Volume=volume,
			};
		}
	}
}