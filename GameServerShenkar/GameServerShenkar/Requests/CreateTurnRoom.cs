using GameServerShenkar.Globals;
using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Services;
using Pipelines.Sockets.Unofficial.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Requests
{
    internal class CreateTurnRoom
    {
        public static Dictionary<string, object> Get(User CurUser, Dictionary<string, object> Details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();

            if (Details.ContainsKey("Name") && Details.ContainsKey("Owner")
                && Details.ContainsKey("MaxUsers") && Details.ContainsKey("TableProperties")
                && Details.ContainsKey("TurnTime"))
            {
                int currMatchId = GlobalVariables.GetAndIncMatchId();
                SearchData currSearchData = new SearchData(CurUser.UserId, RedisService.GetPlayerRating(CurUser.UserId));
                List<SearchData> searchDataList = new List<SearchData>();
                searchDataList.Add(currSearchData);
                MatchData currmatchdata = new MatchData(currMatchId, searchDataList, CurUser.UserId);
                MatchingManager.Instance.AddToMatchingData(currMatchId.ToString(), currmatchdata);
                string newMatchId = ReadyToPlay(CurUser, currMatchId.ToString(),
                    Details["Name"].ToString())["MatchId"].ToString();
                response.Add("Response", "CreateTurnRoom");
                response.Add("IsSuccess", true);
                response.Add("RoomId", RoomsManager.Instance.GetRoomByMatchId(newMatchId).RoomId);

            }
            else
            {
                response.Add("Response", "CreateTurnRoom");
                response.Add("IsSucces", false);
            }

            return response;
        }

        private static Dictionary<string, object> ReadyToPlay(User CurUser, string MatchId,string roomName)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            try
            {
                MatchData curMatchData = MatchingManager.Instance.GetMatchingData(MatchId);
                if (curMatchData != null)
                {
                    curMatchData.ChangePlayerReady(CurUser.UserId, true);
                    if (curMatchData.IsAllReady())
                    {
                        MatchingManager.Instance.RemoveFromMatchingData(MatchId);
                        response = MatchingService.Create(curMatchData,roomName);
                    }
                    else Console.WriteLine("Waiting for more players");
                }
            }
            catch (Exception e)
            {

            }

            return response;
        }
    }
}
