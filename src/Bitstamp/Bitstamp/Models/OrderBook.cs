using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Bitstamp {
	public class OrderBook {
		public OrderBook(){
			Bids = new Levels ();
			Asks = new Levels ();
		}

		public Levels Bids{get;set;}
		public Levels Asks{get;set;}
	}

	[DebuggerDisplay("Price = {Price}\tVolume = {Volume}")]
	public class Level {
		public double Price{get;set;}
		public double Volume{get;set;}
	}

	public class Levels:List<Level>{
	}
}