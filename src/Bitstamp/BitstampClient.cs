using System;
using System.Net.Http;
using System.Security;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Bitstamp.Net.Http.Formatting;

namespace Bitstamp {
	public partial class BitstampClient {
		private BitstampMediaTypeFormatter _formatter = new BitstampMediaTypeFormatter();
		private HttpClient _http;
		private SecureString _key;
		private SecureString _secret;

		public static BitstampClient New(Action<IBitstampClientConfigurator> configure) {
			var configurator = new BitstampClientConfigurator();
			configure(configurator);
			return configurator.Build();
		}

		public BitstampClient(string scheme = "https", string host = "www.bitstamp.net", int port = 443, string path = "api", string key = "", string secret = "") {
			var builder = new UriBuilder { Scheme = scheme, Host = host, Port = port, Path = path };
			var handler = new HttpClientHandler();

			_http = new HttpClient(handler);
			_http.BaseAddress = builder.Uri;
			_http.DefaultRequestHeaders.Add("User-Agent", "{0} {1}".FormatWith(typeof(BitstampClient).Assembly.GetName().Name, typeof(BitstampClient).Assembly.GetName().Version.ToString(4)));
			_http.DefaultRequestHeaders.Add("Rest-Key", key);
			_http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			_key = key.GetSecureString();
			_secret = key.GetSecureString();
		}

		private async Task<HttpResponseMessage> SendAsync(HttpMethod method, Action<IHttpRequestMessageConfigurator> configure) {
			var configurator = new HttpRequestMessageConfigurator();

			configurator.Method(method);
			configurator.BaseAddress(_http.BaseAddress);
			configure(configurator);

			var request = configurator.Build();
#if MGP_LATER
			var response = await _http.SendAsync(request).ConfigureAwait(false);
			await response.EnsureSuccessStatusCode(true).ConfigureAwait(false);
			return response;
#else
			var task = _http.SendAsync(request);
			var response = task.Result;
			response.EnsureSuccessStatusCode(true).Wait();
			return response;
#endif

		}

		private async Task<HttpResponseMessage> GetAsync(string path, object values = null) {
			return await SendAsync(HttpMethod.Get, x => {
				x.Path(path);
				x.Values(values);
			});
		}

		private async Task<HttpResponseMessage> PostAsync(string path) {
			return await SendAsync(HttpMethod.Post, x => {
				x.Path(path);
			});
		}
	}
}

