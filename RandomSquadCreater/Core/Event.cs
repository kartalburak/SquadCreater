using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RandomSquadCreater.Core
{
    public class Event
    {

        public int EventId { get; set; }

        public string EventName { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime EventCreatedDate { get; set; }

        public int EventQuota { get; set; }

        public bool IsWin { get; set; }

        public string ResultTeam1 { get; set; }

        public string ResultTeam2 { get; set; }





    }
}