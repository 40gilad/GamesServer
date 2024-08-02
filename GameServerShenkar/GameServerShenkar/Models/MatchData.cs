using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Models
{
    public class MatchData
    {
        private Dictionary<string, SearchData> _playersData;
        public Dictionary<string, SearchData> PlayersData { get => _playersData;}

        private int matchId;
        public int MatchId { get => matchId;}

        private string matchDate;

        private string first_player;
        public string FirstPlayer { get => first_player;}

        public MatchData(int CurMatchId,List<SearchData> PassedPlayersData,string FirstPlayerId)
        {
            matchId = CurMatchId;
            first_player=FirstPlayerId;
            matchDate = DateTime.UtcNow.ToString();

            _playersData = new Dictionary<string, SearchData>();
            foreach(SearchData s in PassedPlayersData)
            {
                s.IsReady = false;
                _playersData.Add(s.UserId,s);
            }
        }

        public void ChangePlayerReady(string UserId, bool IsReady)
        {
            if (_playersData.ContainsKey(UserId))
                _playersData[UserId].IsReady = IsReady;
        }

        public bool IsAllReady()
        {
            foreach(string userId in _playersData.Keys)
            {
                if (_playersData[userId].IsReady == false)
                    return false;
            }
            return true;
        }
    }
}
