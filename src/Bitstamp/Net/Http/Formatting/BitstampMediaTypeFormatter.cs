using System;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;

namespace Bitstamp.Net.Http.Formatting {
	internal class BitstampMediaTypeFormatter : JsonMediaTypeFormatter {
		public BitstampMediaTypeFormatter() {
            SerializerSettings.Converters.Add(new StringToDoubleConverter());
            SerializerSettings.Converters.Add(new StringToDateTimeOffsetConverter());
		}

		public override bool CanReadType(Type type) {
			return true;
		}

		public override bool CanWriteType(Type type) {
			return true;
		}

		public override Task<object> ReadFromStreamAsync(Type type, Stream stream, HttpContent content, IFormatterLogger formatterLogger) {
			return base.ReadFromStreamAsync(type, stream, content, formatterLogger);
		}

		public override Task WriteToStreamAsync(Type type, object value, Stream stream, HttpContent content, System.Net.TransportContext transportContext) {
			return base.WriteToStreamAsync(type, value, stream, content, transportContext);
		}
	}
}
