using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RandomSquadCreater.Api.Models;


namespace RandomSquadCreater.Api.Controllers
{
    public class HomeController : ApiController
    {
     
        public List<string> GetMatchRoster()
        {
            return new AppGlobal().GetMatchRoster();
        }




    }
}
