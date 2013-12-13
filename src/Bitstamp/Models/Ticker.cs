using System;
using Newtonsoft.Json;
using Bitstamp.Net.Http.Formatting;

namespace Bitstamp.Models {
    public class Ticker {
        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Bid { get; set; }

        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Ask { get; set; }

        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Last { get; set; }

        [JsonConverter(typeof(StringToDoubleConverter))]
        public double High { get; set; }

        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Low { get; set; }

        [JsonConverter(typeof(StringToDoubleConverter))]
        public double Volume { get; set; }

        public override string ToString() {
            return string.Format("[Ticker: Bid={0}, Ask={1}, Last={2}, High={3}, Low={4}, Volume={5}]", Bid, Ask, Last, High, Low, Volume);
        }
    }
}