using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace GolGuru
{
    [HubName("golGuru")]
    public class GolGuruHub : Hub
    {
        private readonly GolGuru _golGuru;

        public GolGuruHub() : this(GolGuru.Instance) { }

        public GolGuruHub(GolGuru golGuru)
        {
            _golGuru = golGuru;
        }

        //public string GetMatchStatus()
        //{
        //    return _golGuru.GetMatchStatus();
        //}

        //public string MatchLiveQuestions()
        //{
        //     _golGuru.MatchLiveQuestions();
        //}
    }
}
