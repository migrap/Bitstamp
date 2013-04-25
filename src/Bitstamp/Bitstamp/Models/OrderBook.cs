using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Bitstamp {
	public class OrderBook {
		public OrderBook(){
			Bids = new Levels ();
			Asks = new Levels ();
		}

		public Levels Bids{get;set;}
		public Levels Asks{get;set;}
	}
#if MGP_LATER
	public class Level {
		//[JsonConverter(typeof(StringToDoubleConverter))]
		public string Price{get;set;}

		//[JsonConverter(typeof(StringToDoubleConverter))]
		public string Volume{get;set;}
	}
#else
	public class Level:List<string>{
	}
#endif

	public class Levels:List<Level>{
	}
}

