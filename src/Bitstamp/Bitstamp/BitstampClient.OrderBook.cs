using System;
using System.Threading.Tasks;

namespace Bitstamp {
	public partial class BitstampClient {
		public async Task<OrderBook> GetOrderBookAsync() {
			return await GetAsync("order_book")
				.ConfigureAwait(false)
					.GetAwaiter()
					.GetResult()
					.Content.ReadAsAsync<OrderBook>(_formatter)
					.ConfigureAwait(false);
		}
	}
}

