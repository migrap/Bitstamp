using System;
using Newtonsoft.Json;

namespace Bitstamp {
	public class Ticker {
		[JsonConverter(typeof(StringToDoubleConverter))]
		public double Bid{ get; internal set;}

		[JsonConverter(typeof(StringToDoubleConverter))]
		public double Ask{ get; internal set;}

		[JsonConverter(typeof(StringToDoubleConverter))]
		public double Last{get;internal set;}

		[JsonConverter(typeof(StringToDoubleConverter))]
		public double High{get;internal set;}

		[JsonConverter(typeof(StringToDoubleConverter))]
		public double Low{get;internal set;}

		[JsonConverter(typeof(StringToDoubleConverter))]
		public double Volume{get;internal set;}

		public override string ToString () {
			return string.Format ("[Ticker: Bid={0}, Ask={1}, Last={2}, High={3}, Low={4}, Volume={5}]", Bid, Ask, Last, High, Low, Volume);
		}
	}
}