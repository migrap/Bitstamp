using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Bitstamp {
	public class ApiException : Exception, ISerializable {
		public HttpStatusCode Status { get; protected set; }
		public HttpResponseMessage ResponseMessage { get; protected set; }

		public ApiException(HttpResponseMessage responseMessage, string message, Exception innerException = null)
			: base(message, innerException) {
			ResponseMessage = responseMessage;
			Status = responseMessage.StatusCode;
		}
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {         
        }
    }
}

