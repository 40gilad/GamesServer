using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Services
{
    public class MatchingService
    {
        public static Dictionary<string,object> Create(MatchData CurMatchData,string roomName=null)
        {
            return new Dictionary<string, object>()
            {
                { "Service","StartMatch"},
                { "MatchId",CreateRoom(CurMatchData,roomName)}
            }; 
        }

        private static string CreateRoom(MatchData CurMatchData,string roomName)
        {
            if (roomName == null)
                roomName = "Room" + CurMatchData.MatchId;
            int dbMatchId = 1;
            string redisMatchId = RedisService.GetMatchId();
            if (redisMatchId != null && redisMatchId != string.Empty)
                dbMatchId = int.Parse(redisMatchId) + 1;

            RedisService.SetMatchId(dbMatchId.ToString());

            GameThread gameroom = new GameThread(dbMatchId.ToString(), CurMatchData, roomName);
            gameroom.JoindUserCount = gameroom.JoindUserCount + 1;
            RoomsManager.Instance.AddRoom(dbMatchId.ToString(), gameroom);
            //gameroom.StartGame();

            return dbMatchId.ToString();
        }
    }
}
