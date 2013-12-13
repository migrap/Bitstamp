using Bitstamp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PusherClient;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Reactive.Concurrency;

namespace Bitstamp {
    public partial class BitstampClient {
        private Pusher _pusher;
        private ConcurrentDictionary<Type, object> _messages = new ConcurrentDictionary<Type, object>();
        private ISubject<object> _observable = new Subject<object>();
        private Subscription _orders = new Subscription();
        private Subscription _trades = new Subscription();
        private Subscription _depth = new Subscription();

        public async Task<bool> ConnectAsync(string applicationkey = "de504dc5763aeef9ff52") {
            var tcs = new TaskCompletionSource<bool>();
            _pusher = new Pusher(applicationkey);

            var connected = (ConnectedEventHandler)null;
            
            _orders.Handler  = (sender) => {
                _orders.Channel.Subscribed -= _orders.Handler;
                //_orders.Channel.Bind("order_deleted", (d) => On(OrderDeleted, (d as JObject).AsOrder()));
                //_orders.Channel.Bind("order_created", (d) => On(OrderCreated, (d as JObject).AsOrder()));
                //_orders.Channel.Bind("order_changed", (d) => On(OrderChanged, (d as JObject).AsOrder()));
            };

            _trades.Handler = (sender) => {
                _trades.Channel.Subscribed -= _trades.Handler;
                _trades.Channel.Bind("trade", (d) => OnNext((d as JObject).AsTrade()));
            };

            _depth.Handler = (sender) => {
                _depth.Channel.Subscribed -= _depth.Handler;
                _depth.Channel.Bind("data", (d) => OnNext((d as JObject).AsOrderBook()));
            };

            _pusher.Connected += connected = (sender) => {
                _pusher.Connected -= connected;
                tcs.SetResult(true);
            };

            _pusher.ConnectionStateChanged += (s, e) => {
            };

            _pusher.Connect();

            return await tcs.Task;
        }

        private void OnNext<T>(T value) {
            _observable.OnNext(value);
        }
        
        private IObservable<T> GetObservable<T>(string channel, Subscription subscription) {
            return (IObservable<T>)_messages.GetOrAdd(typeof(T), type => {
                subscription.Channel = _pusher.Subscribe(channel);
                subscription.Channel.Subscribed += subscription.Handler;
                return _observable.OfType<T>();
            });
        }

        public IObservable<Trade> GetTradesObservable() {
            return (IObservable<Trade>)GetObservable<Trade>("live_trades", _trades);
        }

        public IObservable<OrderBook> GetOrderBookObservable() {
            return (IObservable<OrderBook>)GetObservable<OrderBook>("order_book", _depth);
        }

        public IObservable<Ticker> GetTickerObservable() {
            return (IObservable<Ticker>)_messages.GetOrAdd(typeof(Ticker), type => {
                return Observable.Create<Ticker>(observer => {
                    var ticker = GetTickeAsync().Result;
                    var disposed = false;
                    var gate = new AsyncLock();
                    var cancel = new CancellationTokenSource();


                    Observable.CombineLatest(GetTradesObservable(), GetOrderBookObservable(), (trade, book) => new { Trade = trade, OrderBook = book })
                        .Subscribe(a => {
                            var ask = a.OrderBook.Asks[0];
                            var bid = a.OrderBook.Bids[0];
                            var value = new Ticker {
                                Ask = ask.Price,
                                Bid = bid.Price,
                                Last = a.Trade.Price,
                                Volume = ticker.Volume += a.Trade.Amount,
                                High = Math.Min(ticker.High, a.Trade.Price),
                                Low = Math.Min(ticker.Low, a.Trade.Price),
                            };
                            ticker = value;
                            observer.OnNext(value);
                        });

                    observer.OnNext(ticker);

                    return new CompositeDisposable(Disposable.Create(() => gate.Wait(() => {
                        cancel.Cancel();
                    })), Disposable.Create(() => gate.Wait(() => {
                        disposed = true;
                    })));
                });
            });            
        }
    }    

    internal static partial class Extensions {
        private static JsonSerializer _serializer = new JsonSerializer();
        public static Order AsOrder(this JObject self) {
            return self.As<Order>();
        }

        public static Trade AsTrade(this JObject self) {
            return self.As<Trade>();
        }

        public static OrderBook AsOrderBook(this JObject self, int count=0) {
            var result = new OrderBook();
            result.Bids.AddRange(OrderBook(self, "bids", count));
            result.Asks.AddRange(OrderBook(self, "asks", count));
            return result;
        }

        private static IEnumerable<Level> OrderBook(JObject value, string name, int count) {
            var array = value[name].OfType<JArray>();
            array = (count == 0) ? array : array.Take(count);
            return array.Select(Level);
        }

        private static Level Level(JArray value) {
            var price = double.Parse(value[0].ToString());
            var volume = double.Parse(value[1].ToString());

            return new Level {
                Price = price,
                Volume = volume,
            };
        }

        private static T As<T>(this JObject self) {
            return (T)_serializer.Deserialize<T>(new JTokenReader(self));
        }
    }
}