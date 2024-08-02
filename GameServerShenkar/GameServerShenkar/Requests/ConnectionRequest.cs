using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Services;
using GameServerShenkar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using static GameServerShenkar.Globals.GlobalEnums;

namespace GameServerShenkar.Requests
{
    internal class ConnectionRequest
    {
        public static void Get(IWebSocketSession session,string data)
        {
            Console.WriteLine("Open Connection, Id: " + session.ID);
            try
            {
                Dictionary<string, object> userData = JsonLogic.Deserialize(data);
                if(userData != null && userData.ContainsKey("UserId")) 
                {
                    string userId = userData["UserId"].ToString();
                    User newUser = new User(userId, session);
                    newUser.SetMatchingState();

                    SessionsManager.Instance.AddUser(newUser);

                    int playerRating = RedisService.GetPlayerRating(newUser.UserId);
                    SearchingManager.Instance.AddToSearch(newUser.UserId, playerRating);
                }
                else
                {
                    session.Context.WebSocket.Close((ushort)CloseCode.MissingUserId, "User Id was missing on Open Connection");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ConnectionRequest, Exception: " + e.Message);
            }
        }
    }
}
