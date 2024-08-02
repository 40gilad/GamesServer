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
    internal class StopGameRequest
    {
        public static Dictionary<string, object> Get(User CurUser, Dictionary<string, object> Details)
        {
            GameThread room = RoomsManager.Instance.GetRoomByMatchId(CurUser.MatchId);
            Dictionary<string, object> response = new Dictionary<string, object>();
            if (Details.ContainsKey("Winner"))
            {
                string winner = Details["Winner"].ToString();
                if (winner != "Tie")
                    room.SendChat(CurUser,"Winner is: \n"+winner);
            }
            
            return response;
        }
    }
}
