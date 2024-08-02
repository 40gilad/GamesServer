using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Threads;
using GameServerShenkar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Requests
{
    internal class SubscribeRoomRequest
    {
        public static Dictionary<string, object> Get(User CurUser, Dictionary<string, object> Details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            /*
            {"Service", "SubscribeRoom"},
            {"RoomId", roomId}
            */
            GameThread curr = null;
            Dictionary<string, object> roomDetails = new Dictionary<string, object>();

            if (Details.ContainsKey("RoomId"))
            {
                response.Add("Response", "SubscribeRoom");
                response.Add("RoomId", Details["RoomId"]);

                curr = RoomsManager.Instance.GetRoomByRoomId(Details["RoomId"].ToString());
                if (curr != null)
                {
                    string MatchId = curr.MatchId;

                    if (curr.RoomOwner != CurUser.UserId)
                    { // second player wants to subscribe
                        if (curr.AddPlayerToRoomBrodcast(CurUser.UserId))
                            response.Add("IsSuccess", true);
                        else { response.Add("IsSuccess", false); }
                    }
                    else if (curr.RoomOwner == CurUser.UserId) // owner wants to brodcast room
                        response.Add("IsSuccess", true);
                    //adding RoomData
                    roomDetails.Add("RoomId", curr.RoomId);
                    roomDetails.Add("Name", curr.RoomName);
                    roomDetails.Add("TurnTime", curr.TurnTime);
                    roomDetails.Add("Owner", curr.RoomOwner);
                    roomDetails.Add("MaxUsersCount", curr.MaxUsersCount);
                    roomDetails.Add("JoinedUsersCount", curr.JoindUserCount);
                    response.Add("RoomData", roomDetails);
                }
                else
                    response.Add("Response", "SubscribeRoom");

            }
            bool IsSuccess;
            if (bool.TryParse(response["IsSuccess"].ToString(), out IsSuccess))
            {
                if (IsSuccess)
                {
                    response.Add("Service", "UserJoinRoom");
                    response.Add("UserId", CurUser.UserId);

                    string OwnerId = curr.RoomOwner;
                    if(OwnerId != CurUser.UserId)
                    {
                        // need to send joindroom to owner
                       Dictionary<string, object> sendToOwner = new Dictionary<string, object>();
                        sendToOwner.Add("Service", "UserJoinRoom");
                        sendToOwner.Add("UserId", CurUser.UserId);
                        sendToOwner.Add("RoomData", roomDetails);
                        string retData = JsonLogic.Serialize(sendToOwner);
                        curr.Users[OwnerId].SendMessage(retData);
                    }

                    
                }
            }
            return response;
        }
    }
}
