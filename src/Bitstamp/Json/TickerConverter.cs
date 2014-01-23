using Bitstamp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitstamp.Json {
    //internal class TickerConverter : JsonConverter {
    //    public override bool CanConvert(Type objectType) {
    //        return typeof(Ticker).IsAssignableFrom(objectType);
    //    }

    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) {
    //        var jobject = JObject.Load(reader);
    //        var kvp = jobject.FirstOrDefault<KeyValuePair<string, JToken>>();

    //        return kvp.Value.ToObject(objectType);
    //    }

    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
    //        throw new NotImplementedException();
    //    }
    //}
}
