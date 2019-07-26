using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace RandomSquadCreater.Api.Models
{
    public class AppGlobal
    {
        SquadContext _context=new SquadContext();

        public List<string> GetMatchRoster()
        {
            List<string> result = new List<string>();
            List<Player> matchRoster = new List<Player>();
            List<Player> players = _context.Player.Where(x => x.PlayerIsComing == true).ToList(); 

            if (players.Where(x => x.PlayerPosition == "Forward").Count() == 2)
            {
                if (players.Where(x => x.PlayerPosition == "MidField").Count() + players.Where(x => x.PlayerPosition == "Deffence").Count() == 10)
                {
                    if (players.Where(x => x.PlayerPosition == "GoalKeeper").Count() == 2)
                    {
                        matchRoster.AddRange(players.Where(x => x.PlayerPosition == "GoalKeeper").ToList());
                    }
                    else
                    {
                        matchRoster.AddRange(players.Where(x => x.PlayerPosition == "GoalKeeper").ToList());
                        result.Add("Kaleciniz bulunmamaktadır. ");
                    }

                    matchRoster.AddRange(players.Where(x => x.PlayerPosition == "MidField").ToList());
                    matchRoster.AddRange(players.Where(x => x.PlayerPosition == "Deffence").ToList());
                }
                else
                {
                    result.Add("Orta Saha ve Defans : Yeterli sayıda oyuncu yok. ");
                }
                matchRoster.AddRange(players.Where(x => x.PlayerPosition == "Forward").ToList());
            }
            else
            {
                result.Add("Forvetiniz bulunmamaktadır.");
            }



            List<string> matchPlayerRoster = AppGlobal.ConvertListToJson(matchRoster);

            if (matchPlayerRoster.Count == 14)
            {
                return matchPlayerRoster;
            }
            else
            {
                return result;
            }
        }

        public static List<string> ConvertListToJson(List<Score> objects)
        {

            List<string> result = new List<string>();
            foreach (var item in objects)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var jsonObjects = serializer.Serialize(objects);
                result.Add(jsonObjects);
            }

            return result;
        }

        public static List<string> ConvertListToJson(List<Player> objects)
        {

            List<string> result = new List<string>();
            foreach (var item in objects)
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var jsonObjects = serializer.Serialize(objects);
                result.Add(jsonObjects);
            }

            return result;
        }
    }
}