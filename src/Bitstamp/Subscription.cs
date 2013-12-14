using PusherClient;

namespace Bitstamp {
    internal class Subscription {
        public Channel Channel { get; set; }
        public SubscriptionEventHandler Handler { get; set; }
    }
}
