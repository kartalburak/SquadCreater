using RandomSquadCreater.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Web;
using System.Web.Script.Serialization;


namespace RandomSquadCreater
{
    public static class AppGlobal
    {

        public static List<string> ConvertListToJson(List<Score> objects)
        {

            List<string> result=new List<string>();
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