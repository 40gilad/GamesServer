using GameServerShenkar.Models;
using GameServerShenkar.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Managers
{
    internal class SearchingManager
    {
        private Dictionary<string, int> _searchingList;
        private MatchingThread curMatchingThread;

        #region Singleton

        private static SearchingManager instance;
        public static SearchingManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SearchingManager();
                return instance;
            }
        }

        #endregion

        public SearchingManager()
        {
            _searchingList = new Dictionary<string, int>();

            curMatchingThread = new MatchingThread();
            curMatchingThread.StartThread();
        }

        public void AddToSearch(string userId,int rating)
        {
            if (_searchingList == null)
                _searchingList = new Dictionary<string, int>();

            if(_searchingList.ContainsKey(userId))
                _searchingList[userId] = rating;
            else _searchingList.Add(userId, rating);
        }

        public void RemoveFromSearch(string userId)
        {
            if(_searchingList != null && _searchingList.ContainsKey(userId))
                _searchingList.Remove(userId);
        }

        public List<SearchData> GetSearchingList()
        {
            List<SearchData> searchingData = new List<SearchData>();
            foreach(string userId in _searchingList.Keys)
                searchingData.Add(new SearchData(userId, _searchingList[userId]));

            return searchingData;
        }

    }
}
