using GolGuru.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace GolGuru
{
    public class GolGuru
    {
        //// Singleton instance   
        private readonly static Lazy<GolGuru> _instance = new Lazy<GolGuru>(() => new GolGuru(GlobalHost.ConnectionManager.GetHubContext<GolGuruHub>().Clients));

        private readonly object _matchStatusLock = new object();

        private static long _matchStatusLiveQuestionId;

        Stopwatch stopWatch = new Stopwatch();

        private static string _matchStatus = Helpers.Helpers.MatchStatusEnum.NotReady.ToString();

        private static string _matchHour;

        private static string _equipo1;

        private static string _equipo2;

        public static GolGuru Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }

        private GolGuru(IHubConnectionContext clients)
        {
            Clients = clients;
        }
        public long GetMatchLiveQuestionId()
        {
            return _matchStatusLiveQuestionId++;

        }
        public long GetMatchLiveQuestionIdResponse()
        {
            return _matchStatusLiveQuestionId;

        }

        public string GetMatchStatus()
        {
            return _matchStatus;

        }
        public Int64 GetMatchTime()
        {
            var time = 0;
            if (stopWatch.IsRunning)
            {
                time = stopWatch.Elapsed.Minutes * 60 + stopWatch.Elapsed.Seconds;
            }
            return time;

        }

        public string GetMatchHour()
        {
            return _matchHour;

        }
        public void SetMatchHour(string matchHour)
        {
            _matchHour = matchHour;

        }
        public string GetMatchEquipo1()
        {
            return _equipo1;

        }
        public void SetMatchEquipo1(string equipo)
        {
            _equipo1 = equipo;

        }
        public string GetMatchEquipo2()
        {
            return _equipo2;

        }
        public void SetMatchEquipo2(string equipo)
        {
            _equipo2 = equipo;

        }

        public void SetMatchStatus(Helpers.Helpers.MatchStatusEnum status)
        {
            lock (_matchStatusLock)
            {
                _matchStatus = status.ToString();

                switch (status)
                {
                    case Helpers.Helpers.MatchStatusEnum.StartedFirst:
                        stopWatch.Restart();
                        break;
                    case Helpers.Helpers.MatchStatusEnum.FinishedFirst:
                        stopWatch.Restart();
                        break;
                    case Helpers.Helpers.MatchStatusEnum.StartedSecond:
                        stopWatch.Restart();
                        break;
                    case Helpers.Helpers.MatchStatusEnum.FinishedSecond:
                        stopWatch.Stop();
                        break;
                }
            }
            BroadcastMatchStatus();
        }
        public void BrodcastMatchLiveQuestions(string typeQuestion, Models.Question jsonQuestion)
        {
            Clients.All.MatchLiveQuestions(typeQuestion, JsonConvert.SerializeObject(jsonQuestion));
        }

        private void BroadcastMatchStatus()
        {
            Clients.All.getMatchStatus(_matchStatus);
        }

        public void BrodcastMatchLiveAnswer(Answer answer)
        {
            Clients.All.MatchLiveAnswer(answer);
        }

        public void BrodcastMatchLiveEvents(string typeEvent, Event jsonEvent)
        {
            Clients.All.MatchLiveEvents(typeEvent, JsonConvert.SerializeObject(jsonEvent));
        }

    }

}

/*
  public class StockTicker
    {
        // Singleton instance
        private readonly static Lazy<StockTicker> _instance = new Lazy<StockTicker>(
            () => new StockTicker(GlobalHost.ConnectionManager.GetHubContext<StockTickerHub>().Clients));

        private readonly object _marketStateLock = new object();
        private readonly object _updateStockPricesLock = new object();

        private readonly ConcurrentDictionary<string, Stock> _stocks = new ConcurrentDictionary<string, Stock>();

        // Stock can go up or down by a percentage of this factor on each change
        private readonly double _rangePercent = 0.002;
        
        private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);
        private readonly Random _updateOrNotRandom = new Random();

        private Timer _timer;
        private volatile bool _updatingStockPrices;
        private volatile MarketState _marketState;

        private StockTicker(IHubConnectionContext clients)
        {
            Clients = clients;
            LoadDefaultStocks();
        }

        public static StockTicker Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }

        public MarketState MarketState
        {
            get { return _marketState; }
            private set { _marketState = value; }
        }

        public IEnumerable<Stock> GetAllStocks()
        {
            return _stocks.Values;
        }

        public void OpenMarket()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Open)
                {
                    _timer = new Timer(UpdateStockPrices, null, _updateInterval, _updateInterval);

                    MarketState = MarketState.Open;

                    BroadcastMarketStateChange(MarketState.Open);
                }
            }
        }

        public void CloseMarket()
        {
            lock (_marketStateLock)
            {
                if (MarketState == MarketState.Open)
                {
                    if (_timer != null)
                    {
                        _timer.Dispose();
                    }

                    MarketState = MarketState.Closed;

                    BroadcastMarketStateChange(MarketState.Closed);
                }
            }
        }

        public void Reset()
        {
            lock (_marketStateLock)
            {
                if (MarketState != MarketState.Closed)
                {
                    throw new InvalidOperationException("Market must be closed before it can be reset.");
                }
                
                LoadDefaultStocks();
                BroadcastMarketReset();
            }
        }

        private void LoadDefaultStocks()
        {
            _stocks.Clear();

            var stocks = new List<Stock>
            {
                new Stock { Symbol = "MSFT", Price = 30.31m },
                new Stock { Symbol = "APPL", Price = 578.18m },
                new Stock { Symbol = "GOOG", Price = 570.30m }
            };

            stocks.ForEach(stock => _stocks.TryAdd(stock.Symbol, stock));
        }

        private void UpdateStockPrices(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            lock (_updateStockPricesLock)
            {
                if (!_updatingStockPrices)
                {
                    _updatingStockPrices = true;

                    foreach (var stock in _stocks.Values)
                    {
                        if (TryUpdateStockPrice(stock))
                        {
                            BroadcastStockPrice(stock);
                        }
                    }

                    _updatingStockPrices = false;
                }
            }
        }

        private bool TryUpdateStockPrice(Stock stock)
        {
            // Randomly choose whether to udpate this stock or not
            var r = _updateOrNotRandom.NextDouble();
            if (r > 0.1)
            {
                return false;
            }

            // Update the stock price by a random factor of the range percent
            var random = new Random((int)Math.Floor(stock.Price));
            var percentChange = random.NextDouble() * _rangePercent;
            var pos = random.NextDouble() > 0.51;
            var change = Math.Round(stock.Price * (decimal)percentChange, 2);
            change = pos ? change : -change;

            stock.Price += change;
            return true;
        }

        private void BroadcastMarketStateChange(MarketState marketState)
        {
            switch (marketState)
            {
                case MarketState.Open:
                    Clients.All.marketOpened();
                    break;
                case MarketState.Closed:
                    Clients.All.marketClosed();
                    break;
                default:
                    break;
            }
        }

        private void BroadcastMarketReset()
        {
            Clients.All.marketReset();
        }

        public void SendQuestion()
        {
            Clients.All.sendQuestion();
        }

        private void BroadcastStockPrice(Stock stock)
        {
            Clients.All.updateStockPrice(stock);
        }
    }

    public enum MarketState
    {
        Closed,
        Open
    }
}

*/