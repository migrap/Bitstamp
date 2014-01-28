using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Bitstamp.Net.Http.Configurators {    
    internal class HttpRequestMessageConfigurator : IHttpRequestMessageConfigurator {
        private HttpRequestMessage _request = new HttpRequestMessage();

        public HttpRequestMessageConfigurator() {
            _request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IHttpRequestMessageConfigurator Method(HttpMethod value) {
            _request.Method = value;
            return this;
        }

        public IHttpRequestMessageConfigurator Content(HttpContent value) {
            _request.Content = value;
            return this;
        }

        public IHttpRequestMessageConfigurator Properties(string name, object value) {
            _request.Properties.Add(name, value);
            return this;
        }

        public IHttpRequestMessageConfigurator Address(string value) {
            _request.RequestUri = new Uri(value, UriKind.RelativeOrAbsolute);
            return this;
        }

        public IHttpRequestMessageConfigurator Header(string name, string value) {
            _request.Headers.Add(name, value);
            return this;
        }

        public HttpRequestMessage Build() {
            return _request;
        }
    }
}
