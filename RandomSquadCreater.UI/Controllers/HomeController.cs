using RandomSquadCreater.Core;
using RandomSquadCreater.UI.Infrastructure;
using RandomSquadCreater.UI.Models;
using RandomSquadCreater.UI.ServiceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;

namespace RandomSquadCreater.UI.Controllers
{
    public class HomeController : Controller
    {
        RandomSquadCreaterObject service = new RandomSquadCreaterObject();

        //Puan Takip
        [LoginAuthorize(Roles = "Admin")]
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");
            }
            List<Rating> result = service.GetAllRatings();
            List<RateModel> rateModel = new List<RateModel>();
            foreach (Rating rate in result)
            {
                Player playerRated = service.GetPlayer(rate.Rated);
                Player playerScored = service.GetPlayer(rate.Scored);

                RateModel model = new RateModel();
                model.EventDate = rate.EventDate;
                model.Point = rate.Point;
                model.RatedName = playerRated.PlayerUserName;
                model.ScoredName = playerScored.PlayerUserName;

                rateModel.Add(model);
            }



            return View(rateModel.Take(100).OrderByDescending(x => x.EventDate));
        }
        public ActionResult Login()
        {

            //LoginModel model = new LoginModel();
            //HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName.ToString()];
            //if (authCookie != null)
            //{
            //    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
            //    if (authTicket != null & !authTicket.Expired)
            //    {
            //        model.email = authTicket.Name;
            //    }
            //}
            //model
            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password, string checkbox)
        {
            //if (checkbox != null)
            //{
            //    FormsAuthentication.SetAuthCookie(email, true);
            //}
            if (service.Login(email, password))
            {
                Session["user"] = service.GetPlayer(email);
                Log.Info(service.GetPlayer(email).PlayerName + " " + service.GetPlayer(email).PlayerSurname + " sisteme giriş yaptı.");
                return RedirectToAction("PlayerList", "Player");
            }
            else
            {
                return RedirectToAction("/Login");
            }
        }
        public ActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection formCollection)
        {
            Player player = new Player();

            player.PlayerName = formCollection["Name"];
            player.PlayerSurname = formCollection["Surname"];
            player.PlayerUserName = formCollection["UserName"];
            player.PlayerEmail = formCollection["Email"];
            player.PlayerPassword = formCollection["Password"];
            player.PlayerPosition = formCollection["Positions"];
            player.PlayerPower = Convert.ToInt32(formCollection["Power"]);
            player.PlayerIsAdmin = false;

            if (service.SavePlayer(player))
            {
                Log.Info(player.PlayerName + " " + player.PlayerSurname + " sisteme kayıt oldu.");
                return RedirectToAction("PlayerList", "Player");
            }
            else
            {
                return RedirectToAction("Register", "Home");
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Clear();
                Session.RemoveAll();
                Session.Abandon();
                return RedirectToAction("Login", "Home");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return RedirectToAction("Login", "Home");
            }


        }

        public ActionResult UnAuthorized()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(FormCollection formCollection)
        {
            string newPassword = System.Guid.NewGuid().ToString("N").Remove(8);
            string email = formCollection["reemail"];


            Player player = service.GetPlayer(email);
            player.PlayerPassword = newPassword;
            try
            {
                service.UpdatePlayer(player);
                Log.Info(player.PlayerName + " " + player.PlayerSurname + "'ın şifresi sıfırlandı.");

                SendMailModel sendMail = new SendMailModel();
                StringBuilder message = new StringBuilder();
                message.Append("Şifrenizi aşağıdaki bilgilerle login olarak güncelleyebilirsiniz.");
                message.Append("<br>");
                message.Append(@"<b>Email Adresiniz: <span style=""color: Red""> " + email + "</span></b>");
                message.Append("<br>");
                message.Append(@"<b>Şifreniz: <span style=""color: Red"">" + newPassword + "</span></b>");



                string from = System.Configuration.ConfigurationManager.AppSettings["DefaultMail"];
                string password = System.Configuration.ConfigurationManager.AppSettings["DefaultPassword"];

                sendMail.SendMails(from, password, email, "Şifre Sıfırlama", message.ToString());
                Log.Info(player.PlayerName + " " + player.PlayerSurname + "'a şifre sıfırlamak için bilgi maili gönderildi.");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }


            return RedirectToAction("Login", "Home");
        }

    }
}