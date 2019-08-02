using System.Web.Http;
using RandomSquadCreater.Api.Models;


namespace RandomSquadCreater.Api.Controllers
{
    public class HomeController : ApiController
    {
        [System.Web.Http.HttpGet]
        public IHttpActionResult GetMatchRoster()
        {
            return Json(new AppGlobal().GetMatchRoster());
        }




    }
}
