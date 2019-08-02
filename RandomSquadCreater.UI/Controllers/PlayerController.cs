using RandomSquadCreater.Core;
using RandomSquadCreater.UI.Infrastructure;
using RandomSquadCreater.UI.ServiceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RandomSquadCreater.UI.Controllers
{
    public class PlayerController : Controller
    {
        RandomSquadCreaterObject service = new RandomSquadCreaterObject();

        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }

        [HttpPost]
        public ActionResult PlayerList(string point, string sender)
        {
            if (!IsNumeric(point))
            {
                return Json(new { success = "isNumeric" }, JsonRequestBehavior.AllowGet);
            }

            Player currentPlayer = Session["user"] as Player;
            Player senderPlayer = service.GetAllPlayer().Where(x => x.PlayerName + " " + x.PlayerSurname == sender)
                .FirstOrDefault();

            DateTime lastRatingDate = service.GetAllRatings()
                .Where(x => x.Rated == currentPlayer.PlayerId && x.Scored == senderPlayer.PlayerId)
                .OrderByDescending(x => x.EventDate).Select(x => x.EventDate).FirstOrDefault();

            if ((DateTime.Now - lastRatingDate).TotalDays < 6)
            {
                return Json(new { success = "timeout" }, JsonRequestBehavior.AllowGet);
            }

            Rating rating = new Rating();
            rating.Rated = currentPlayer.PlayerId;
            rating.Scored = senderPlayer.PlayerId;
            rating.Point = Int32.Parse(point);
            rating.EventDate = DateTime.Now;
            try
            {
                service.AddRating(rating);
                Log.Info(currentPlayer.PlayerName + " " + currentPlayer.PlayerSurname + ", " + senderPlayer.PlayerName + " " + senderPlayer.PlayerSurname + "'a " + point + "puan verdi.");


                //Ortalama Hesabı yapılacak
                int ratingCount = service.GetAllRatings().Where(x => x.Scored == senderPlayer.PlayerId).Count();
                int ratingSum = service.GetAllRatings().Where(x => x.Scored == senderPlayer.PlayerId).Sum(x => x.Point);
                float averagePoint = ratingSum / ratingCount;

                #region UpdatePlayer
                senderPlayer.PlayerPower = (int)averagePoint;
                service.UpdatePlayer(senderPlayer);
                Log.Info(senderPlayer.PlayerName + " " + senderPlayer.PlayerSurname + "'ın ortalama güç değeri güncellendi.");
                #endregion

                #region UpdateorAddScore
                Score lastScore = service.GetAllScore().Where(x => x.PlayerId == senderPlayer.PlayerId).FirstOrDefault();

                if (lastScore == null)
                {
                    Score score = new Score();

                    score.EventDate = DateTime.Now;
                    score.AvaragePoint = averagePoint;
                    score.PlayerId = senderPlayer.PlayerId;

                    service.AddScore(score);
                    Log.Info(senderPlayer.PlayerName + " " + senderPlayer.PlayerSurname + "'ın yeni ortalama güç değeri " + averagePoint + " olarak eklendi.");


                }
                else
                {
                    lastScore.EventDate = DateTime.Now;
                    lastScore.AvaragePoint = averagePoint;

                    service.UpdateScore(lastScore);
                    Log.Info(senderPlayer.PlayerName + " " + senderPlayer.PlayerSurname + "'ın yeni ortalama güç değeri " + averagePoint + " olarak güncellendi.");


                }
                #endregion


                return Json(new { Result = true });
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return Json(new { Result = false });

            }


        }

        [HttpGet]
        public ActionResult PlayerList()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            //Player player = Session["user"] as Player;
            //ViewBag.RateModel = service.GetAllRatings().Where(x => x.Rated == player.PlayerId).FirstOrDefault();

            List<Player> comingPlayers = new List<Player>();
            List<Player> players = service.GetAllPlayer().Where(x => x.PlayerIsComing == true).ToList();//event date control 


            foreach (var player in players)
            {
                if (DateTime.Now.Subtract(player.PlayerVoteDate).TotalDays < 7)
                {
                    comingPlayers.Add(player);
                }
            }



            ViewBag.GetAllPlayers = comingPlayers;
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
                return Json(new { Result = true });
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return Json(new { Result = false }, JsonRequestBehavior.AllowGet);
            }



        }


        [HttpPost]
        public ActionResult PlayerListByKey(FormCollection formCollection)
        {

            string key = formCollection["randomPlayer"];
            if (key != null && key.Length > 0)
            {
                Session["Players"] = service.GetAllPlayer()
                    .Where(x => x.PlayerName.ToLower().Contains(key.ToLower()) || x.PlayerSurname.ToLower().Contains(key.ToLower()))
                    .ToList();
            }
            else
            {
                Session["Players"] = service.GetAllPlayer();
            }






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
                return Json(new { Result = true });
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                return Json(new { Result = false });

            }


            //return RedirectToAction("PlayerProfile", "Player");
        }

        //[HttpPost]
        //public ActionResult PlayerProfile(FormCollection formCollection)
        //{

        //    if (formCollection["Password"] == formCollection["RePassword"])
        //    {
        //        Player player = service.GetPlayer(formCollection["Email"]);

        //        player.PlayerUserName = formCollection["Username"];
        //        player.PlayerName = formCollection["Name"];
        //        player.PlayerSurname = formCollection["Surname"];
        //        player.PlayerPassword = formCollection["Password"];
        //        player.PlayerEmail = formCollection["Email"];
        //        player.PlayerPower = Convert.ToInt32(formCollection["Power"]);
        //        player.PlayerIsAdmin = formCollection["playerAdmin"] == null ? false : true;
        //        player.PlayerPosition = formCollection["Position"].Contains("GoalKeeper") || formCollection["Position"].Contains("Deffence") || formCollection["Position"].Contains("MidField") || formCollection["Position"].Contains("Forward") ? formCollection["Position"] : null;

        //        try
        //        {
        //            service.UpdatePlayer(player);
        //            Session["user"] = player;
        //            Log.Info(player.PlayerName + " " + player.PlayerSurname + "'ın bilgileri güncellendi.");
        //            //ToastrService.AddToUserQueue(new Toastr("Oyuncu bilgileri güncellendi."));
        //        }
        //        catch (Exception e)
        //        {
        //            Log.Error(e.Message);
        //        }
        //    }




        //    //return RedirectToAction("PlayerProfile", "Player");
        //    return View(Session["user"] as Player);
        //}


        [HttpPost]
        public JsonResult AjaxPlayerProfile(FormCollection formCollection)
        {

            if (formCollection["Password"] == formCollection["RePassword"])
            {
                Player player = service.GetPlayer(formCollection["Email"]);

                if (player.PlayerIsAdmin)
                {
                    player.PlayerPower = Convert.ToInt32(formCollection["Power"]);
                    player.PlayerIsAdmin = formCollection["playerAdmin"] == null ? false : true;
                    player.PlayerPosition = formCollection["Position"].Contains("GoalKeeper") || formCollection["Position"].Contains("Deffence") || formCollection["Position"].Contains("MidField") || formCollection["Position"].Contains("Forward") ? formCollection["Position"] : null;
                }

                player.PlayerUserName = formCollection["Username"];
                player.PlayerName = formCollection["Name"];
                player.PlayerSurname = formCollection["Surname"];
                player.PlayerPassword = formCollection["Password"];
                player.PlayerEmail = formCollection["Email"];


                try
                {
                    service.UpdatePlayer(player);
                    Session["user"] = player;
                    Log.Info(player.PlayerName + " " + player.PlayerSurname + "'ın bilgileri güncellendi.");
                    //ToastrService.AddToUserQueue(new Toastr("Oyuncu bilgileri güncellendi."));
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }

            return Json(new { Result = true });
        }


    }
}