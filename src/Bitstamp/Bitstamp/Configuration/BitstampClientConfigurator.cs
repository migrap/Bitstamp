using System;

namespace Bitstamp {
	internal class BitstampClientConfigurator : IBitstampClientConfigurator {
		//https://www.bitstamp.net/api/ticker/
		private string _scheme = "https";
		private string _host = "www.bitstamp.net";
		private int _port = 443;
		private string _path = "api";
		private string _key;
		private string _secret;

		public void Scheme(string value) {
			_scheme = value;
		}

		public void Host(string value) {
			_host = value;
		}

		public void Port(int value) {
			_port = value;
		}

		public void Path(string value) {
			_path = value;
		}

		public void Key(string value) {
			_key = value;
		}

		public void Secret(string value) {
			_secret = value;
		}

		public BitstampClient Build() {
			return new BitstampClient(_scheme, _host, _port, _path, _key, _secret);
		}
	}
}

