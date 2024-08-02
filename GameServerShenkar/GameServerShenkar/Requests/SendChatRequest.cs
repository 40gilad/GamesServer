using GameServerShenkar.Globals;
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
    public class SendChatRequest
    {
        public static Dictionary<string, object> Get(User CurUser, string Message)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            GameThread room = RoomsManager.Instance.GetRoomByMatchId(CurUser.MatchId);
            if (room != null && room.IsRoomActive)
            {
                response = room.SendChat(CurUser, Message);
                response.Add("IsSucces", true);
            }
            else response.Add("ErrorCode", GlobalEnums.ErrorCodes.RoomClosed);

            if (response.ContainsKey("IsSucces") == false)
                response.Add("IsSucces", false);

            return response;
        }
    }
}
