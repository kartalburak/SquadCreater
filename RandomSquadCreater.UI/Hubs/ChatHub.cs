using Microsoft.AspNet.SignalR;
using RandomSquadCreater.Core;
using RandomSquadCreater.UI.Infrastructure;
using RandomSquadCreater.UI.ServiceObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomSquadCreater.UI.Hubs
{
    public class ChatHub : Hub
    {


        //public void SendMessage(string divChat)
        //{
        //    Clients.Others.GetMessageOther(divChat);


        //    Clients.Caller.GetMessageCaller(divChat);

        //    //string id = Context.ConnectionId;
        //    //Clients.Client(id);

        //}

        //public override Task OnConnected()
        //{
        //    string id = Context.ConnectionId;
        //    return base.OnConnected();
        //}

        //public override Task OnDisconnected(bool stopCalled)
        //{

        //    return base.OnDisconnected(stopCalled);
        //}

        #region Data Members

        static List<PlayerDetail> ConnectedUsers = new List<PlayerDetail>();
        static List<MessageDetail> CurrentMessage = new List<MessageDetail>();

        #endregion

        #region Methods

        public void Connect(string userName)
        {
            var id = Context.ConnectionId;

            using (RandomSquadCreaterObject service = new RandomSquadCreaterObject())
            {
                Player connectedPlayer = service.GetPlayerByName(userName);
                if (connectedPlayer != null)
                {
                    connectedPlayer.PlayerIsOnline = true;
                    connectedPlayer.PlayerConnectionId = id;
                    connectedPlayer.PlayerLastLoginTime = DateTime.Now;
                    try
                    {
                        service.UpdatePlayer(connectedPlayer);
                        Log.Info(connectedPlayer.PlayerName + " " + connectedPlayer.PlayerSurname + " şu anda online.");

                    }
                    catch (Exception e)
                    {
                        Log.Error(e.Message);
                    }

                }
            }



            if (ConnectedUsers.Count(x => x.ConnectionId == id) == 0)
            {
                ConnectedUsers.Add(new PlayerDetail { ConnectionId = id, UserName = userName });

                // send to caller
                Clients.Caller.onConnected(id, userName, ConnectedUsers, CurrentMessage);

                // send to all except caller client
                Clients.AllExcept(id).onNewUserConnected(id, userName);

            }

        }

        public void SendMessageToAll(string userName, string message)
        {
            // store last 100 messages in cache
            AddMessageinCache(userName, message);
            try
            {
                // Broad cast message
                Clients.All.messageReceived(userName, message);
                Log.Info(userName + " tüm kullanıcılara mesaj gönderdi.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public void SendPrivateMessage(string toUserId, string message)
        {

            string fromUserId = Context.ConnectionId;

            var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
            var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

            if (toUser != null && fromUser != null)
            {
                // send to 
                Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

                // send to caller user
                Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
            }

        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                ConnectedUsers.Remove(item);

                using (RandomSquadCreaterObject service = new RandomSquadCreaterObject())
                {
                    Player connectedPlayer = service.GetPlayerByConnectedId(Context.ConnectionId);
                    if (connectedPlayer != null)
                    {
                        connectedPlayer.PlayerIsOnline = false;
                        try
                        {
                            service.UpdatePlayer(connectedPlayer);
                            Log.Info(connectedPlayer.PlayerName + " " + connectedPlayer.PlayerSurname + " şu anda online değil.");
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.Message);
                            throw;
                        }
                    }

                }


                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.UserName);

            }

            return base.OnDisconnected(stopCalled);
        }


        #endregion

        #region private Messages

        private void AddMessageinCache(string userName, string message)
        {
            CurrentMessage.Add(new MessageDetail { UserName = userName, Message = message });

            if (CurrentMessage.Count > 100)
                CurrentMessage.RemoveAt(0);
        }

        #endregion
    }
}