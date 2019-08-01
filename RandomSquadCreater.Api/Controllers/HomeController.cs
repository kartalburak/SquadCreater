using RandomSquadCreater.Api.Models;
using System.Collections.Generic;
using System.Web.Http;


namespace RandomSquadCreater.Api.Controllers
{
    public class HomeController : ApiController
    {

        public List<string> GetMatchRoster()
        {
            return new AppGlobal().ConvertListToJson(new AppGlobal().GetMatchRoster());
        }




    }
}
