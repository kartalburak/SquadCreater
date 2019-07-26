using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RandomSquadCreater.UI.Models
{
    public class RateModel
    {
        public string RatedName { get; set; }

        public string ScoredName { get; set; }

        public DateTime EventDate { get; set; }

        public int Point { get; set; }
    }
}