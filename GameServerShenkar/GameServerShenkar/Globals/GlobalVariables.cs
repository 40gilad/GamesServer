using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Globals
{
    internal class GlobalVariables
    {
        private static string _serverUrl = "ws://localhost:7890"; 
        public static string ServerUrl { get { return _serverUrl; } }

        private static int _turnTime = 10;
        public static int TurnTime { get { return _turnTime; } }

        private static int _timeOutTime = 360;
        public static int TimeOutTime { get { return _timeOutTime; } }

        private static int _roomId = 1;
        public static int RoomId { get { return _roomId; } }
        public static int GetAndIncRoomId()
        {
            return _roomId++;
        }

        private static int min_players = 1;
        public static int MinPlayers { get { return min_players; } }

        private static int max_players = 2;
        public static int MaxPlayers { get { return max_players; } }

        private static int match_id= 1;
        public static int MatchId { get { return match_id; } }
        public static int GetAndIncMatchId()
        {
            return match_id++;
        }

    }
}
