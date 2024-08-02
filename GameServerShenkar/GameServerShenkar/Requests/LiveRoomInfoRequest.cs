using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Requests
{
    internal class LiveRoomInfoRequest
    {
        public static string RoomPropertiesPassword = "Shenkar";
        public static Dictionary<string, object> Get(User CurUser, Dictionary<string, object> Details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            Dictionary<string, object> prop = new Dictionary<string, object>();
            if (Details.ContainsKey("RoomId"))
            {
                response.Add("Response", "GetLiveRoomInfo");
                
                // Users list
                GameThread room = RoomsManager.Instance.GetRoomByRoomId(Details["RoomId"].ToString());
                Dictionary<string, User> uid_user_pair = room.Users;
                List<string> users = new List<string>();
                foreach (KeyValuePair<string,User> pair in uid_user_pair)
                    users.Add(pair.Key);
                response.Add("Users", users);

                //Room properties
                prop["Password"] = RoomPropertiesPassword;
                response.Add("RoomProperties", prop);

                //Room Data
                if (room != null)
                {
                    Dictionary<string, object> roomDetails = new Dictionary<string, object>();
                    roomDetails.Add("RoomId", room.RoomId);
                    roomDetails.Add("Name", room.RoomName);
                    roomDetails.Add("TurnTime", room.TurnTime);
                    roomDetails.Add("Owner", room.RoomOwner);
                    roomDetails.Add("MaxUsersCount", room.MaxUsersCount);
                    roomDetails.Add("JoinedUsersCount", room.JoindUserCount);
                    response.Add("RoomData", roomDetails);
                }
                else
                    response.Add("RoomData", null);

            }
            else
                response.Add("Response", "GetLiveRoomInfo");
            return response;

        }
    }
}
