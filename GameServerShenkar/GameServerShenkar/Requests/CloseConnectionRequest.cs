using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace GameServerShenkar.Requests
{
    internal class CloseConnectionRequest
    {
        public static void Get(string id)
        {
            User curUser = SessionsManager.Instance.UsersSessions[id];
            if (curUser != null)
            {
                SearchingManager.Instance.RemoveFromSearch(curUser.UserId);
            }
        }
    }
}
