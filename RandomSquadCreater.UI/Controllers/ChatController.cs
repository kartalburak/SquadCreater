using RandomSquadCreater.UI.ServiceObject;
using System.Web.Mvc;

namespace RandomSquadCreater.UI.Controllers
{   
    public class ChatController : Controller
    {
        RandomSquadCreaterObject service = new RandomSquadCreaterObject();
        public ActionResult Chat(string Name)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Name = Name;
            ViewBag.Players = service.GetAllPlayer();

            return View();
        }

        public ActionResult PrivateChat(string Name)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            ViewBag.Name = Name;
            ViewBag.Players = service.GetAllPlayer();

            return View();
        }
    }
}