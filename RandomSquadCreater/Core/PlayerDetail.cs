using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace RandomSquadCreater.Core
{
    public class PlayerDetail
    {
        public int PlayerDetailId { get; set; }
        public int PlayerId { get; set; }
        public string ConnectionId { get; set; }
        public string UserName { get; set; }
        public bool IsOnline { get; set; }

    }
}