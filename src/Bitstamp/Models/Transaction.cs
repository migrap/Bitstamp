using Bitstamp.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstamp.Models {
    public class Transaction {
        [JsonProperty("date")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTimeOffset Datetime { get; set; }

        [JsonProperty("tid")]
        public long Tid { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }
    }
}
