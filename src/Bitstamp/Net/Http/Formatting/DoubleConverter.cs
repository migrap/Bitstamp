using Newtonsoft.Json;
using System;

namespace Bitstamp.Net.Http.Formatting {
	public class DoubleConverter :JsonConverter{
		public DoubleConverter(){
		}

		public override bool CanConvert(Type objectType) {
			return objectType == typeof(double);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
			if (reader.TokenType == JsonToken.String)
			{
					var value = default(double);

					if (double.TryParse((string)reader.Value, out value)){
						return value;
					}

				throw new JsonReaderException(string.Format("Expected double, got {0}", reader.Value));
			}
			throw new JsonReaderException(string.Format("Unexcepted token {0}", reader.TokenType));
		}

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer) {
			throw new NotImplementedException ();
		}
	}
}


