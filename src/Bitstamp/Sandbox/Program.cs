using System;
using Bitstamp;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Sandbox {
	class MainClass {
		public static void Main (string[] args) {
			var bitstamp = new BitstampClient ();
			var ticker = bitstamp.GetTickeAsync().Result;
			var orderbook = bitstamp.GetOrderBookAsync (5).Result;

			Console.WriteLine (ticker);

			(new AutoResetEvent (false)).WaitOne ();
		}
	}
}

/*
System.AggregateException:  ---> System.Exception: Error reading string. Unexpected token: StartArray. Path 'bids[0]', line 1, position 11.
  at Newtonsoft.Json.JsonReader.ReadAsStringInternal () [0x00000] in <filename unknown>:0
  at Newtonsoft.Json.JsonTextReader.ReadAsString () [0x00000] in <filename unknown>:0
  at Newtonsoft.Json.Serialization.JsonSerializerInternalReader.ReadForType (Newtonsoft.Json.JsonReader reader, Newtonsoft.Json.Serialization.JsonContract contract, Boolean hasConverter) [0x00000] in <filename unknown>:0
  at Newtonsoft.Json.Serialization.JsonSerializerInternalReader.PopulateList (IWrappedCollection wrappedList, Newtonsoft.Json.JsonReader reader, Newtonsoft.Json.Serialization.JsonArrayContract contract, Newtonsoft.Json.Serialization.JsonProperty containerProperty, System.String id) [0x00000] in <filename unknown>:0
  at --- End of stack trace from previous location where exception was thrown ---
  at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw () [0x0000b] in /private/tmp/source/bockbuild-crypto-mono/profiles/mono-mac-xamarin/build-root/mono-3.0.10/mcs/class/corlib/System.Runtime.ExceptionServices/ExceptionDispatchInfo.cs:62
  at System.Runtime.CompilerServices.ConfiguredTaskAwaitable`1+ConfiguredTaskAwaiter[Bitstamp.OrderBook].GetResult () [0x00021] in /private/tmp/source/bockbuild-crypto-mono/profiles/mono-mac-xamarin/build-root/mono-3.0.10/mcs/class/corlib/System.Runtime.CompilerServices/ConfiguredTaskAwaitable_T.cs:58
  at Bitstamp.BitstampClient+<GetOrderBookAsync>c__async4.MoveNext () [0x00022] in /Users/Michael/Projects/Bitstamp/src/Bitstamp/Bitstamp/BitstampClient.OrderBook.cs:7
  --- End of inner exception stack trace ---
  at System.Threading.Tasks.Task`1[Bitstamp.OrderBook].get_Result () [0x0003c] in /private/tmp/source/bockbuild-crypto-mono/profiles/mono-mac-xamarin/build-root/mono-3.0.10/mcs/class/corlib/System.Threading.Tasks/Task_T.cs:52
  at Sandbox.MainClass.Main (System.String[] args) [0x00037] in /Users/Michael/Projects/Bitstamp/src/Bitstamp/Sandbox/Program.cs:10
*/