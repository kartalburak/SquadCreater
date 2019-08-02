using RandomSquadCreater.UI.Infrastructure;
using RandomSquadCreater.UI.Models;
using RandomSquadCreater.UI.ServiceObject;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RandomSquadCreater.UI.Controllers
{
    public class MailController : Controller
    {
        RandomSquadCreaterObject service = new RandomSquadCreaterObject();

        [ValidateInput(false)]
        public ActionResult SendMail()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendMail(FormCollection formCollection)
        {

            string to = formCollection["to"];
            string[] toList = to.Split(';');
            List<string> result = new List<string>();

            for (int i = 0; i < toList.Count(); i++)
            {
                var response = new SendMailModel().SendMails(formCollection["from"], formCollection["password"],
                    toList[i],
                    formCollection["subject"], formCollection["compose-textarea"]);

                if (response)
                {
                    Log.Info(toList[i] + "'a mail başarılı şekilde gönderildi");
                }
                else
                {
                   Log.Error(toList[i] + "'a mail gönderilemedi. Gönderici Mailin ayarlarını kontrol ediniz!");
                }

            }

            
            


            return View();
        }


        public ActionResult InboxMail()
        {
            return View();
        }

        public ActionResult SentMail()
        {
            return View();
        }

        public ActionResult DraftMail()
        {
            return View();
        }

        public ActionResult TrashMail()
        {
            return View();
        }
    }
}