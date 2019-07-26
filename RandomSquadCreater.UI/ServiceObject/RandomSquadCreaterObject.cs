using RandomSquadCreater.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Xml;
using Microsoft.Ajax.Utilities;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using  RandomSquadCreater.UI.Infrastructure;

namespace RandomSquadCreater.UI.ServiceObject
{
    public class RandomSquadCreaterObject:IDisposable
    {
        private RandomServiceHelper.RandomSquadServiceClient Service;
        public string Ticket = string.Empty;
        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        public RandomSquadCreaterObject()
        {
            var binding = new BasicHttpBinding()
            {
                Security = new BasicHttpSecurity() { Mode = BasicHttpSecurityMode.None },
                MaxReceivedMessageSize = 2147483647,UseDefaultWebProxy = true,
                MaxBufferSize = 2147483647,
                MaxBufferPoolSize = 2147483647,
                OpenTimeout = new TimeSpan(0, 20, 0),
                CloseTimeout = new TimeSpan(0, 20, 0),
                SendTimeout = new TimeSpan(0, 20, 0),
                ReceiveTimeout = new TimeSpan(0, 20, 0),
                ReaderQuotas = new XmlDictionaryReaderQuotas()
                {
                    MaxDepth = 32,
                    MaxArrayLength = 2147483647,
                    MaxStringContentLength = 2147483647
                }
            }; 
            var uri = "http://localhost:63746/RandomSquadService.svc?wsdl";

            Service = new RandomServiceHelper.RandomSquadServiceClient(binding, new EndpointAddress(uri));

        }

        public bool Login(string userName, string password)
        {

            try
            {
                var response = Service.Login(userName, password);
                return response == true ? true : false;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
            


            

        }
        public List<Rating> GetAllRatings()
        {
            var response = Service.GetAllRating().ToList();
            return response;

        }
        public List<Player> GetAllPlayer()
        {
            var response = Service.GetAllPlayer().ToList();
            return response;

        }

        public List<Score> GetAllScore()
        {
            var response = Service.GetAllScore().ToList();
            return response;

        }
        public Player GetPlayer(int id)
        {
            var response = Service.GetPlayer(id);
            return response;

        }
        public Player GetPlayerByName(string name)
        {
            var response = Service.GetPlayerByName(name);
            return response;

        }

        public Player GetPlayer(string email)
        {
            var response = Service.GetAllPlayer().Where(x => x.PlayerEmail == email).FirstOrDefault();
            return response;

        }

        public Player GetPlayerByConnectedId(string connectionId)
        {
            try
            {
                var response = Service.GetAllPlayer().Where(x => x.PlayerConnectionId == connectionId).FirstOrDefault();
                return response;
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
                throw;
            }
          

        }

        public bool SavePlayer(Player player)
        {
            var response = Service.AddPlayer(player);
            return response;
        }

        public bool AddRating(Rating rating)
        {
            var response = Service.AddRating(rating);
            return response;
        }

        public bool AddScore(Score score)
        {
            var response = Service.AddScore(score);
            return response;
        }

        public bool UpdateScore(Score score)
        {
            var response = Service.UpdateScore(score);
            return response;
        }

        public bool UpdatePlayer(Player player)
        {
            var response = Service.UpdatePlayer(player);
            return response;
        }

        public List<string> GetScoreJsonList()
        {
            return AppGlobal.ConvertListToJson(Service.GetAllScore().ToList());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}