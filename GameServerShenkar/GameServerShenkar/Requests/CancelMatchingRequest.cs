using GameServerShenkar.Globals;
using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Requests
{
    internal class CancelMatchingRequest
    {
        public static Dictionary<string, object> Get(User CurUser, string Message)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            GameThread room = RoomsManager.Instance.GetRoomByMatchId(CurUser.MatchId);
            response.Add("Response", "CancelMatching");
            if (room != null && room.IsRoomActive)
            {
                
                if (CurUser.CurUserState == User.UserState.PrePlay)
                {
                    CurUser.CurUserState = User.UserState.Matching;
                    if(room.ReomovePlayer(CurUser.UserId))
                        response.Add("IsSuccess", true);
                    else
                        response.Add("ErrorCode", GlobalEnums.ErrorCodes.Unknown);//change to the matching error
                }
                else
                {
                    response.Add("IsSuccess", false);
                    response.Add("ErrorCode", GlobalEnums.ErrorCodes.Unknown);//change to the matching error
                }
            }
            else
            {
                response.Add("IsSuccess", false);
                response.Add("ErrorCode", GlobalEnums.ErrorCodes.RoomDoesntExist);//change to the matching error
            }
            return response;
        }
    }
}
