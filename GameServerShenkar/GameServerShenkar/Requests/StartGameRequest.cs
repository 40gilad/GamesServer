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
    internal class StartGameRequest
    {
        public static Dictionary<string, object> Get(User CurUser, Dictionary<string, object> Details)
        {
            string match_id = CurUser.MatchId;
            GameThread curr = RoomsManager.Instance.GetRoomByMatchId(match_id); ;

            // { "Service", "StartGame" }
            Dictionary<string, object> response = new Dictionary<string, object>();
            List<string> PlayersList = new List<string>()
            {
                curr.RoomOwner,
                curr.SecondPlayer
            };
            response.Add("Service", "StartGame");
            response.Add("Sender", CurUser.UserId);
            response.Add("RoomId", curr.RoomId);
            response.Add("NextTurn", CurUser.UserId);
            response.Add("TurnsList", PlayersList);
            response.Add("TurnTime", curr.TurnTime);
            curr.Users[curr.SecondPlayer].SendMessage(JsonLogic.Serialize(response));
            curr.StartGame();
            return response;
        }
    }
}
