using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstamp.Models {
    public class Trade {
        [JsonProperty("id")]
        public long TradeId { get; set; }
        
        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }
        
        public override string ToString() {
            return (new { TradeId, Amount, Price }).ToString();
        }
    }
}

//Trade: {
//  "price": 124.4,
//  "amount": 5.44394508,
//  "24vol": 7768.93108898,
//  "id": 1446511
//}