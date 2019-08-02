using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;


namespace RandomSquadCreater.Api.Models
{
    public class AppGlobal
    {
        SquadContext _context = new SquadContext();

        public List<Player> GetMatchRoster()
        {
            List<string> result = new List<string>();
            List<Player> matchRoster = new List<Player>();
            List<Player> comingPlayers = new List<Player>();
            List<Player> players = _context.Player.Where(x => x.PlayerIsComing == true).ToList();//event date control 


            foreach (var player in players)
            {
                if (player.PlayerVoteDate.HasValue)
                {
                    if (DateTime.Now.Subtract(player.PlayerVoteDate.Value).TotalDays < 7)
                    {
                        comingPlayers.Add(player);
                    }
                }
            }


            if (comingPlayers.Where(x => x.PlayerPosition == "Forward").Count() == 2)
            {

                if (comingPlayers.Where(x => x.PlayerPosition == "MidField").Count() + comingPlayers.Where(x => x.PlayerPosition == "Deffence").Count() == 10)
                {
                    if (comingPlayers.Where(x => x.PlayerPosition == "GoalKeeper").Count() == 2)
                    {
                        matchRoster.AddRange(comingPlayers.Where(x => x.PlayerPosition == "GoalKeeper").ToList());
                    }
                    else
                    {
                        matchRoster.AddRange(comingPlayers.Where(x => x.PlayerPosition == "GoalKeeper").ToList());
                        result.Add("Kaleciniz bulunmamaktadır. ");
                    }

                    matchRoster.AddRange(comingPlayers.Where(x => x.PlayerPosition == "MidField").ToList());
                    matchRoster.AddRange(comingPlayers.Where(x => x.PlayerPosition == "Deffence").ToList());
                }
                else
                {
                    result.Add("Orta Saha ve Defans : Yeterli sayıda oyuncu yok. ");
                }
                matchRoster.AddRange(comingPlayers.Where(x => x.PlayerPosition == "Forward").ToList());
            }
            else
            {
                result.Add("Forvetiniz bulunmamaktadır.");
            }


            return matchRoster;

        }

        public List<string> ConvertListToJson(List<Score> scores)
        {

            List<string> result = new List<string>();
            foreach (var item in scores)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var jsonObjects = serializer.Serialize(scores);
                result.Add(jsonObjects);
            }

            return result;
        }

        public List<string> ConvertListToJson(List<Player> players)
        {

            List<string> result = new List<string>();
            foreach (var item in players)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var jsonObjects = serializer.Serialize(players);
                result.Add(jsonObjects);
            }

            return result;
        }
    }
}