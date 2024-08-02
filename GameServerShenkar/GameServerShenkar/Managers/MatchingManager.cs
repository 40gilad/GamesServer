using GameServerShenkar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Managers
{
    internal class MatchingManager
    {
        private Dictionary<string, MatchData> _allMatchesData;

        #region Singleton
        private static MatchingManager instance;

        internal static MatchingManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new MatchingManager();
                return instance;
            }
        }
        #endregion
        public MatchingManager() 
        {
            _allMatchesData = new Dictionary<string, MatchData>();
        }

        public void AddToMatchingData(string MatchId,MatchData Data)
        {
            if (_allMatchesData == null)
                _allMatchesData = new Dictionary<string, MatchData>();

            if (_allMatchesData.ContainsKey(MatchId))
                _allMatchesData[MatchId] = Data;
            else _allMatchesData.Add(MatchId, Data);
        }

        public void RemoveFromMatchingData(string MatchId)
        {
            if(_allMatchesData != null && _allMatchesData.ContainsKey(MatchId))
                _allMatchesData.Remove(MatchId);
        }

        public MatchData GetMatchingData(string MatchId)
        {
            if (_allMatchesData != null && _allMatchesData.ContainsKey(MatchId))
               return _allMatchesData[MatchId];
            else return null;
        }
    }
}
