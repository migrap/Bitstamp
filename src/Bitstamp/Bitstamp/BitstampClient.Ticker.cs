using System;
using System.Threading.Tasks;

namespace Bitstamp {
	public partial class BitstampClient {
		public async Task<Ticker> GetTickeAsync() {
			return await GetAsync("ticker")
				.ConfigureAwait(false)
					.GetAwaiter()
					.GetResult()
					.Content.ReadAsAsync<Ticker>(_formatter)
					.ConfigureAwait(false);
		}
	}
}

