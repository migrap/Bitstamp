using System;

namespace Bitstamp {
	public interface IBitstampClientConfigurator {
		void Scheme(string value);
		void Host(string value);
		void Port(int value);
		void Path(string value);
		void Key(string value);
		void Secret(string value);
	}
}

