using GameServerShenkar.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameServerShenkar.Managers
{
    public class RoomsManager
    {
        private Dictionary<string, GameThread> activeRooms;
        public Dictionary<string, GameThread> ActiveRooms { get => activeRooms; }

        private Dictionary<string, string> match_room_id;

       public Dictionary<string,string> MatchRoomId { get => match_room_id; }


        #region Singleton
        private static RoomsManager instance;

        internal static RoomsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new RoomsManager();
                return instance;
            }
        }

        #endregion

        public RoomsManager()
        {
            activeRooms = new Dictionary<string, GameThread>();
            match_room_id = new Dictionary<string, string>();
        }

        public void AddRoom(string MatchId,GameThread Room)
        {
            if (ActiveRooms == null)
                activeRooms = new Dictionary<string, GameThread>();

            ActiveRooms.Add(MatchId, Room);
            ActiveRooms[MatchId] = Room;
            match_room_id[Room.RoomId.ToString()] =MatchId;
        }

        public void RemoveRoom(string MatchId)
        {
            if (ActiveRooms != null && ActiveRooms.ContainsKey(MatchId))
                ActiveRooms.Remove(MatchId);
        }

        public GameThread GetRoomByMatchId(string MatchId)
        {
            if (ActiveRooms != null && ActiveRooms.ContainsKey(MatchId))
                return ActiveRooms[MatchId];
            return null;
        }

        public GameThread GetRoomByRoomId(string RoomId)
        {
            if (ActiveRooms != null)
            {
                try
                {
                    string match_id = match_room_id[RoomId];
                    if (match_id != null && match_id != string.Empty)
                        return activeRooms[match_id];
                }
                catch { return null; }

            }
            return null;
        }
        public bool IsRoomExist(string MatchId)
        {
            if (GetRoomByMatchId(MatchId) != null)
                return true;
            else return false;
        }

    }
}
