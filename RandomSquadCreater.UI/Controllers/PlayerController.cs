using RandomSquadCreater.Core;
using RandomSquadCreater.UI.Infrastructure;
using RandomSquadCreater.UI.ServiceObject;
using System;
using System.Linq;
using System.Web.Mvc;

namespace RandomSquadCreater.UI.Controllers
{
    public class PlayerController : Controller
    {
        RandomSquadCreaterObject service = new RandomSquadCreaterObject();

        [HttpPost]
        public ActionResult PlayerList(string point, string sender)
        {
            Player currentPlayer = Session["user"] as Player;
            Player senderPlayer = service.GetAllPlayer().Where(x => x.PlayerName + " " + x.PlayerSurname == sender)
                .FirstOrDefault();

            #region AddRating
            Rating rating = new Rating();
            rating.Rated = currentPlayer.PlayerId;
            rating.Scored = senderPlayer.PlayerId;
            rating.Point = Int32.Parse(point);
            rating.EventDate = DateTime.Now;
            try
            {
                service.AddRating(rating);
                Log.Info(currentPlayer.PlayerName + " " + currentPlayer.PlayerSurname + ", " + senderPlayer.PlayerName + " " + senderPlayer.PlayerSurname + "'a " + point + "puan verdi.");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

            }

            #endregion

            //Ortalama Hesabı yapılacak
            int ratingCount = service.GetAllRatings().Where(x => x.Scored == senderPlayer.PlayerId).Count();
            int ratingSum = service.GetAllRatings().Where(x => x.Scored == senderPlayer.PlayerId).Sum(x => x.Point);
            float averagePoint = ratingSum / ratingCount;

            #region UpdatePlayer
            senderPlayer.PlayerPower = (int)averagePoint;
            try
            {
                service.UpdatePlayer(senderPlayer);
                Log.Info(senderPlayer.PlayerName + " " + senderPlayer.PlayerSurname + "'ın ortalama güç değeri güncellendi.");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);

            }

            #endregion

            #region UpdateorAddScore
            Score lastScore = service.GetAllScore().Where(x => x.PlayerId == senderPlayer.PlayerId).FirstOrDefault();

            if (lastScore == null)
            {
                Score score = new Score();

                score.EventDate = DateTime.Now;
                score.AvaragePoint = averagePoint;
                score.PlayerId = senderPlayer.PlayerId;
                try
                {
                    service.AddScore(score);
                    Log.Info(senderPlayer.PlayerName + " " + senderPlayer.PlayerSurname + "'ın yeni ortalama güç değeri " + averagePoint + " olarak eklendi.");
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);

                }

            }
            else
            {
                lastScore.EventDate = DateTime.Now;
                lastScore.AvaragePoint = averagePoint;
                try
                {
                    service.UpdateScore(lastScore);
                    Log.Info(senderPlayer.PlayerName + " " + senderPlayer.PlayerSurname + "'ın yeni ortalama güç değeri " + averagePoint + " olarak güncellendi.");
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);

                }

            }
            #endregion

            return RedirectToAction("PlayerList", "Player", service.GetAllPlayer());
        }

        [HttpGet]
        public ActionResult PlayerList()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            if (Session["Players"] == null)
            {
                return View(service.GetAllPlayer());
            }
            else
            {
                return View(Session["Players"]);
            }

        }

        [LoginAuthorize(Roles = "Admin")]
        public ActionResult ScoreList()
        {
            ViewBag.PlayerList = service.GetAllPlayer();



            return View(service.GetAllScore());
        }

        [HttpPost]
        public ActionResult Vote(string myvote)
        {
            string isComing = myvote;


            Player player = Session["user"] as Player;
            player.PlayerVoteDate = DateTime.Now;
            if (isComing == "yes")
            {
                player.PlayerIsComing = true;
            }
            else
            {
                player.PlayerIsComing = false;
            }

            try
            {
                service.UpdatePlayer(player);
                Log.Info(player.PlayerName + "" + player.PlayerSurname + " oyunu " + isComing + " olarak kullandı.");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }

            //Redirect("~/Player/PlayerList");
            //RedirectToAction("PlayerList");
            //return RedirectToAction("PlayerList", "Player", service.GetAllPlayer());
            return RedirectToAction("PlayerList", "Player");

            //return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult PlayerListByKey(FormCollection formCollection)
        {

            string key = formCollection["randomPlayer"];


            Session["Players"] = service.GetAllPlayer()
                .Where(x => x.PlayerName.ToLower().Contains(key.ToLower()) || x.PlayerSurname.ToLower().Contains(key.ToLower()))
                .ToList();




            return RedirectToAction("PlayerList", "Player");

        }


        public ActionResult PlayerProfile()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Grant(string txtusername)
        {
            Player player = service.GetAllPlayer().Where(x => x.PlayerUserName == txtusername).FirstOrDefault();
            player.PlayerIsAdmin = true;
            try
            {
                service.UpdatePlayer(player);
                Log.Info(player.PlayerName + " " + player.PlayerSurname + "'a yetki verildi.");
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }

            return RedirectToAction("PlayerProfile", "Player");
        }

        public ActionResult EditPlayer(FormCollection formCollection)
        {

            if (formCollection["inputPassword"] == formCollection["inputRePassword"])
            {
                Player player = service.GetPlayer(formCollection["inputEmail"]);

                player.PlayerUserName = formCollection["inputUsername"];
                player.PlayerName = formCollection["inputName"];
                player.PlayerSurname = formCollection["inputSurname"];
                player.PlayerPassword = formCollection["inputPassword"];
                player.PlayerEmail = formCollection["inputEmail"];
                player.PlayerPower = Convert.ToInt32(formCollection["inputPower"]);
                player.PlayerIsAdmin = formCollection["playerAdmin"] == null ? false : true;
                player.PlayerPosition = formCollection["inputPosition"].Contains("GoalKeeper") || formCollection["inputPosition"].Contains("Deffence") || formCollection["inputPosition"].Contains("MidField") || formCollection["inputPosition"].Contains("Forward") ? formCollection["inputPosition"] : null;

                try
                {
                    service.UpdatePlayer(player);
                    Session["user"] = player;
                    Log.Info(player.PlayerName + " " + player.PlayerSurname + "'ın bilgileri güncellendi.");
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }




            return RedirectToAction("PlayerProfile", "Player");
        }

    }
}