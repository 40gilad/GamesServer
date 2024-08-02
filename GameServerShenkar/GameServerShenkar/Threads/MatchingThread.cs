using GameServerShenkar.Globals;
using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GameServerShenkar.Threads
{
    internal class MatchingThread
    {
        private bool isMatchingRunning = false;
        private Thread currentThread;
        private int ratingDiffrence = 50;
        private int matchId;

        public MatchingThread(){ }

        public void StartThread()
        {
           matchId = GlobalVariables.MatchId;
           isMatchingRunning = true;
           currentThread = new Thread(new ThreadStart(RunThread));
           currentThread.Start();
        }

        public void RunThread() 
        { 
            while(isMatchingRunning)
            {
                Console.WriteLine(DateTime.Now.ToString());
                MatchingPlayers();
                Thread.Sleep(1000);
            }
        }

        private void MatchingPlayers()
        {
            bool shouldContinue = false;
            List<SearchData> searchData = SearchingManager.Instance.GetSearchingList();
            if(searchData != null && searchData.Count > 1)
            {
                List<SearchData> sortedData = searchData.OrderBy(value => value.Rating).ToList();
                for(int i=0;i < sortedData.Count;i++)
                {
                    shouldContinue = true;
                    SearchData firstUser = sortedData[i];
                    for (int j = i + 1; j < sortedData.Count && shouldContinue; j++)
                    {
                        SearchData secondUser = sortedData[j];
                        if(RatingMatched(firstUser.Rating,secondUser.Rating))
                        {
                            List<User> matchedUsers = new List<User>()
                            {
                                SessionsManager.Instance.UsersSessions[firstUser.UserId],
                                SessionsManager.Instance.UsersSessions[secondUser.UserId]
                            };

                            bool isValidUsers = CheckValidity(matchedUsers);

                            if(isValidUsers)
                            {
                                Dictionary<string, object> data = new Dictionary<string, object>()
                                {
                                    {"Service","ReadyToPlay"},
                                    {"TempMatchId",matchId}
                                };

                                string broadcastMessage = MiniJSON.Json.Serialize(data);
                                foreach (User u in matchedUsers)
                                {
                                    u.SendMessage(broadcastMessage);
                                    SearchingManager.Instance.RemoveFromSearch(u.UserId);
                                    u.CurUserState = User.UserState.PrePlay;
                                    SessionsManager.Instance.UpdateUser(u);
                                }

                                List<SearchData> tempSearchers = new List<SearchData>() 
                                { firstUser, secondUser };

                                MatchData matchData = new MatchData(matchId, tempSearchers,firstUser.UserId);
                                MatchingManager.Instance.AddToMatchingData(matchId.ToString(), matchData);
                                shouldContinue = false; 
                                //matchId++;
                            }
                        }
                    }
                }
            }    
        }

        private bool RatingMatched(int rating1,int rating2)
        {
            int calc = Math.Abs(rating1 - rating2);
            if (calc <= ratingDiffrence)
                return true;
            else return false;
        }
        
        
        private bool CheckValidity(List<User> matchedUsers)
        {
            foreach(User user in matchedUsers)
            {
                if (CheckIfUserIsValid(user) == false)
                    return false;
            }
            return true;
        }

        private bool CheckIfUserIsValid(User user)
        {
            if (user != null && user.CurUserState == User.UserState.Matching
                && user.IsLive())
                return true;
            else return false;
        }
    }
}
