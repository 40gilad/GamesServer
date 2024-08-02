using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Requests
{
    public class ReadyToPlayRequest
    {
        public static Dictionary<string,object> Get(User CurUser,Dictionary<string,object> Details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            if(Details.ContainsKey("MatchId"))
            {
                ReadyToPlay(CurUser, Details["MatchId"].ToString());
                response.Add("IsSucces", true);
            }
            else response.Add("IsSucces", false);

            return response;
        }

        private static Dictionary<string,object> ReadyToPlay(User CurUser,string MatchId)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                MatchData curMatchData = MatchingManager.Instance.GetMatchingData(MatchId);
                if(curMatchData != null)
                {
                    curMatchData.ChangePlayerReady(CurUser.UserId, true);
                    if(curMatchData.IsAllReady())
                    {
                        MatchingManager.Instance.RemoveFromMatchingData(MatchId);
                        response = MatchingService.Create(curMatchData);
                    }
                    else Console.WriteLine("Waiting for more players");
                }
            }
            catch(Exception e)
            {

            }

            return response;
        }

    }
}
