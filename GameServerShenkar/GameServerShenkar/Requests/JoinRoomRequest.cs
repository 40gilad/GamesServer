using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Services;
using GameServerShenkar.Threads;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameServerShenkar.Requests
{
    internal class JoinRoomRequest
    {
        public static Dictionary<string, object> Get(User CurUser, Dictionary<string, object> Details)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            /*
             *             {"Service", "JoinRoom"},
                            {"RoomId", roomId}
            */
            if (Details.ContainsKey("RoomId"))
            {
                response.Add("Response", "JoinRoom");
                response.Add("RoomId", Details["RoomId"]);
                GameThread curr = RoomsManager.Instance.GetRoomByRoomId(Details["RoomId"].ToString());
                if (curr != null)
                {
                    string MatchId = curr.MatchId;

                    if (curr.RoomOwner != CurUser.UserId)
                    { // second player wants to join
                        if (curr.AddPlayer(CurUser.UserId)) 
                            response.Add("IsSuccess", true);
                        else { response.Add("IsSuccess", false); }
                        CurUser.MatchId= MatchId;
                    }
                    else if (curr.RoomOwner == CurUser.UserId) // owner wants to join room
                        response.Add("IsSuccess", true);



                    try
                    {
                        MatchData curMatchData = MatchingManager.Instance.GetMatchingData(MatchId);
                        if (curMatchData != null)
                        {
                            curMatchData.ChangePlayerReady(CurUser.UserId, true);
                            if (curMatchData.IsAllReady())
                            {
                                MatchingManager.Instance.RemoveFromMatchingData(MatchId);
                            }
                            else Console.WriteLine("Waiting for more players");
                        }
                    }
                    catch (Exception e) { }
                }
                else
                    response.Add("IsSuccess", false);
            }
            else
                response.Add("Response", null);

            return response;
        }
    }
}
