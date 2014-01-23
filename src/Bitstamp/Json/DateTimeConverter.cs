using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstamp.Json {
    internal class DateTimeConverter : JsonConverter {
        private static readonly DateTimeOffset Epoch = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        public override bool CanConvert(Type objectType) {
            return typeof(DateTimeOffset) == objectType;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
            if(reader.TokenType == JsonToken.String) {
                var value = default(long);

                if(long.TryParse((string)reader.Value, out value)) {
                    return Epoch.AddSeconds(value);
                }

                throw new JsonReaderException(string.Format("Expected long, got {0}", reader.Value));
            }
            throw new JsonReaderException(string.Format("Unexcepted token {0}", reader.TokenType));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            throw new NotImplementedException();
        }
    }
}
