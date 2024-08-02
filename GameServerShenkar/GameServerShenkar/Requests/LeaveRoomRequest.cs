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
    internal class LeaveRoomRequest
    {
        public static Dictionary<string, object> Get(User CurUser, Dictionary<string, object> Details)
        {

            Dictionary<string, object> response = new Dictionary<string, object>();
            GameThread room = RoomsManager.Instance.GetRoomByMatchId(CurUser.MatchId);
            string CurUserId= CurUser.UserId;
            response.Add("Response", "LeaveRoom");
            if (Details.ContainsKey("RoomId"))
            {
                if (CurUser.CurUserState == User.UserState.PrePlay)
                    CurUser.CurUserState = User.UserState.Matching;

                response.Add("RoomId", room.RoomId);
                response.Add("MaxUsers", room.MaxUsersCount.ToString());
                response.Add("IsSuccess", "true");
                response.Add("Name", room.RoomName);
                response.Add("Owner", string.Empty);

                room.Users.Remove(CurUserId);
                if (room.Users.Count == 0)
                {
                    room.CloseRoom();
                    return response;
                }
                if (room != null) //room wasent closed
                {
                    room.JoindUserCount = room.JoindUserCount - 1;
                    if (room.RoomOwner == CurUserId)
                    {
                        room.RoomOwner = room.SecondPlayer;
                        room.SecondPlayer = string.Empty;
                        
                    }
                    else if (room.SecondPlayer == CurUserId)
                    {
                        room.SecondPlayer = string.Empty;
                    }
                    response["Owner"] = room.RoomOwner;
                }
            }

            return response;
        }
        }
}
