using Bitstamp.Net.Http.Configurators;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading.Tasks;

namespace Bitstamp {
    internal static partial class Extensions {
        internal static StreamContent GetStreamContent(this IEnumerable<byte> source) {
            var stream = new MemoryStream(source.ToArray());
            var content = new StreamContent(stream);
            content.Headers.Add("Content-Type", "application/json");

            return content;
        }

        internal static string FormatWith(this string format, params object[] args) {
            return string.Format(format, args);
        }

        internal static void Add(this NameValueCollection self, string name, long value) {
            self.Add(name, value.ToString());
        }

        internal static string ToQueryString(this NameValueCollection self) {
            return string.Join("&", Array.ConvertAll(self.AllKeys, key => "{0}={1}".FormatWith(key.UrlEncode(), self[key].UrlEncode())));
        }

        /// <summary>
        /// Uses Uri.EscapeDataString() based on recommendations on MSDN
        /// http://blogs.msdn.com/b/yangxind/archive/2006/11/09/don-t-use-net-system-uri-unescapedatastring-in-url-decoding.aspx
        /// </summary>
        internal static string UrlEncode(this string self) {
            return Uri.EscapeDataString(self);
        }

        internal static string UrlEncode(this object self) {
            return UrlEncode(self.ToString());
        }

        internal static string GetString(this SecureString value) {
            if(null == value) {
                throw new ArgumentNullException("value");
            }

            IntPtr ptr = IntPtr.Zero;
            try {

                ptr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(ptr);
            } finally {
                Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        internal static SecureString GetSecureString(this string value) {
            if(null == value) {
                throw new ArgumentNullException("value");
            }

            unsafe {
                fixed(char* chars = value) {
                    var secure = new SecureString(chars, value.Length);
                    secure.MakeReadOnly();
                    return secure;
                }
            }
        }

        internal static Task<T> ReadAsAsync<T>(this HttpContent content, MediaTypeFormatter formatter) {
            return content.ReadAsAsync<T>(new[] { formatter });
        }

        internal static T Populate<T>(this JsonSerializer serializer, JsonReader reader) where T : class,new() {
            var target = new T();
            serializer.Populate(reader, target);
            return target;
        }

        private const string InvalidUnixEpochErrorMessage = "Unix epoc starts January 1st, 1970";
        private static readonly DateTimeOffset Epoch = new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

        internal static DateTimeOffset FromUnixTime(this Int64 self) {
            return Epoch.AddSeconds(self);
        }

        internal static Int64 ToUnixTime(this DateTimeOffset self) {
            if(self == DateTimeOffset.MinValue) {
                return 0;
            }

            var delta = self - Epoch;

            if(delta.TotalSeconds < 0) {
                throw new ArgumentOutOfRangeException(InvalidUnixEpochErrorMessage);
            }

            return (long)delta.TotalMilliseconds;
        }

        internal static Uri Append(this Uri self, params object[] segments) {
            return new Uri(segments.Aggregate(self.AbsoluteUri, (current, segment) => string.Format("{0}/{1}", current.TrimEnd('/'), segment)));
        }

        internal static string Join(this IEnumerable<string> source, string seperator = "&") {
            return string.Join(seperator, source);
        }

        internal static string ToQueryString(this object values) {
            return (values == null) ? string.Empty : TypeDescriptor.GetProperties(values)
                .Where(x => x.GetValue(values) != null)
                    .Select(x => string.Format("{0}={1}", x.Name.UrlEncode(), x.GetValue(values).UrlEncode()))
                    .Join();
        }

        internal static IEnumerable<TResult> Select<TResult>(this PropertyDescriptorCollection collection, Func<PropertyDescriptor, TResult> selector) {
            foreach(PropertyDescriptor item in collection) {
                yield return selector(item);
            }
        }

        internal static IEnumerable<PropertyDescriptor> Where(this PropertyDescriptorCollection collection, Func<PropertyDescriptor, bool> predicate) {
            foreach(PropertyDescriptor item in collection) {
                if(predicate(item)) {
                    yield return item;
                }
            }
        }

        public static async Task EnsureSuccessStatusCode(this HttpResponseMessage message, bool @throw = true) {
            if(@throw && !message.IsSuccessStatusCode) {
                throw new ApiException(message, "The API query failed with status code {0}: {1}".FormatWith(message.StatusCode, message.ReasonPhrase));
            }
        }

        private static TResult Configure<TSource, TResult>(Action<TSource> configure) where TResult : TSource, new() {
            var result = new TResult();
            configure(result);
            return result;
        }

        internal static Task<HttpResponseMessage> SendAsync(this HttpClient client, Action<IHttpRequestMessageConfigurator> configure) {
            var request = Configure<IHttpRequestMessageConfigurator, HttpRequestMessageConfigurator>(configure).Build();
            return client.SendAsync(request);
        }

        internal static IHttpRequestMessageConfigurator Address(this IHttpRequestMessageConfigurator self, string format, params object[] args) {
            return self.Address(format.FormatWith(args));
        }

        internal static IHttpRequestMessageConfigurator Content(this IHttpRequestMessageConfigurator self, object value, MediaTypeFormatter formatter) {
            return self.Content(new ObjectContent(value.GetType(), value, formatter));
        }

        internal static Task<T> ReadAsAsync<T>(this Task<HttpResponseMessage> message, params MediaTypeFormatter[] formatters) {
            var response = message
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsAsync<T>(formatters);
        }

        internal static Task<string> ReadAsStringAsync(this Task<HttpResponseMessage> message) {
            var response = message
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync();
        }

        //internal static Task<T> ReadAsAsync<T>(this HttpResponseMessage message, MediaTypeFormatter formatter) {
        //    return ReadAsAsync<T>(message.Content, formatter);
        //}

        //internal static Task<T> ReadAsAsync<T>(this HttpContent content, MediaTypeFormatter formatter) {
        //    return content.ReadAsAsync<T>(new[] { formatter });
        //}
    }
}