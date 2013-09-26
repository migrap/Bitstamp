using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Bitstamp.Models {
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
		public double Value{ get { return Price * Volume; } }
		public Aggregate Aggregate{ get; set; }
	}

	public class Levels:ICollection<Level>{
		private List<Level> _levels = new List<Level> ();

		public Level this[int index]{
			get{ return _levels [index];}
		}

		public void AddRange(IEnumerable<Level> collection){
			foreach (var item in collection) {
				Add (item);
			}
		}

		public void Add (Level item) {
			item.Aggregate = item;
			if (Count != 0) {
				item.Aggregate += _levels [Count - 1].Aggregate;
			}
			_levels.Add (item);
		}

		public void Clear () {
			_levels.Clear ();
		}

		public bool Contains (Level item) {
			return _levels.Contains (item);
		}

		public void CopyTo (Level[] array, int arrayIndex) {
			_levels.CopyTo (array, arrayIndex);
		}

		public bool Remove (Level item) {
			return _levels.Remove (item);
		}

		public int Count {
			get{ return _levels.Count;}
		}

		public bool IsReadOnly {
			get{ return false;}
		}

		public IEnumerator<Level> GetEnumerator () {
			return _levels.GetEnumerator ();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			return _levels.GetEnumerator ();
		}
	}

	public class Aggregate{
		public double Price{get;set;}
		public double Volume{get;set;}
		public double Value{ get; set; }

		public static implicit operator Aggregate(Level value){
			return new Aggregate {
				Price=value.Price,
				Volume=value.Volume,
				Value=value.Value,
			};
		}

		public static Aggregate operator +(Aggregate lhs, Aggregate rhs){
			lhs.Volume += rhs.Volume;
			lhs.Value += rhs.Value;
			lhs.Price = (lhs.Volume / lhs.Value);
			return lhs;
		}
	}
}