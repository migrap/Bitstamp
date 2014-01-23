using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Bitstamp.Net.Http.Configurators {
    internal interface IHttpRequestMessageConfigurator {
        IHttpRequestMessageConfigurator Method(HttpMethod value);
        IHttpRequestMessageConfigurator Content(HttpContent value);
        IHttpRequestMessageConfigurator Properties(string name, object value);
        IHttpRequestMessageConfigurator Address(string value);
        IHttpRequestMessageConfigurator Header(string name, string value);
    }
}
