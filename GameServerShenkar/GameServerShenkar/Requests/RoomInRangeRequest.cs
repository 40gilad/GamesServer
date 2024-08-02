using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Requests
{
    internal class RoomInRangeRequest
    {
        public static Dictionary<string, object> Get(User CurUser, Dictionary<string, object> Details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            Dictionary<string, GameThread> activeRooms = RoomsManager.Instance.ActiveRooms;
            List<object> roomsList = new List<object>();

            if (activeRooms != null && activeRooms.Count > 0)
            {
                foreach (var roomEntry in activeRooms)
                {
                    GameThread room = roomEntry.Value;
                    Dictionary<string, object> roomDetails = new Dictionary<string, object>();
                    roomDetails.Add("RoomId", room.RoomId);
                    roomDetails.Add("Name", room.RoomName);
                    roomDetails.Add("TurnTime", room.TurnTime);
                    roomDetails.Add("Owner", room.RoomOwner);
                    roomDetails.Add("MaxUsersCount", room.MaxUsersCount);
                    roomDetails.Add("JoinedUsersCount", room.JoindUserCount);
                    roomsList.Add(roomDetails);
                }
                response.Add("Response", "GetRoomsInRange");
                response.Add("Rooms", roomsList);
            }
            else
            {
                response.Add("Response", "GetRoomsInRange");
            }

            return response;
        }
    }
}
