using Bitstamp.Net.Http;
using Bitstamp.Net.Http.Formatting;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security;
using System.Threading.Tasks;

namespace Bitstamp {
	public partial class BitstampClient {
		private BitstampMediaTypeFormatter _formatter = new BitstampMediaTypeFormatter();
		private HttpClient _client;
		private SecureString _key;
		private SecureString _secret;

		static BitstampClient() {
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, error) => true;
			ServicePointManager.FindServicePoint(new Uri("https://www.bitstamp.net")).ConnectionLimit = 8;
            SetAllowUnsafeHeaderParsing20();
		}

		public static BitstampClient New(Action<IBitstampClientConfigurator> configure) {
			var configurator = new BitstampClientConfigurator();
			configure(configurator);
			return configurator.Build();
		}

		public BitstampClient(string scheme = "https", string host = "www.bitstamp.net", int port = 443, string path = "api/", string key = "", string secret = "") {
			var builder = new UriBuilder { Scheme = scheme, Host = host, Port = port, Path = path };
			var handler = new BitstampDelegatingHandler();

			_client = new HttpClient(handler);
			_client.BaseAddress = builder.Uri;
			//_client.DefaultRequestHeaders.Add("User-Agent", "{0} {1}".FormatWith(typeof(BitstampClient).Assembly.GetName().Name, typeof(BitstampClient).Assembly.GetName().Version.ToString(4)));
			

			_key = key.GetSecureString();
			_secret = key.GetSecureString();
		}

        public static bool SetAllowUnsafeHeaderParsing20() {
            //Get the assembly that contains the internal class
            var assembly = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if(assembly != null) {
                //Use the assembly in order to get the internal type for the internal class
                var type = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if(type != null) {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created allready the property will create it for us.
                    var instance = type.InvokeMember("Section", BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
                    if(instance != null) {
                        //Locate the private bool field that tells the framework is unsafe header parsing should be allowed or not
                        FieldInfo useUnsafeHeaderParsing = type.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if(useUnsafeHeaderParsing != null) {
                            useUnsafeHeaderParsing.SetValue(instance, true);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
	}
}

