using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace GameServerShenkar.Requests
{
    internal class ProcessRequest
    {
        public static void Get(IWebSocketSession Session, string Data)
        {
            Dictionary<string,object> response = new Dictionary<string,object>();
            try
            {
                User curUser = SessionsManager.Instance.GetUser(Session.ID);
                if(curUser != null)
                {
                    Dictionary<string, object> msgData = JsonLogic.Deserialize(Data);
                    if(msgData.ContainsKey("Service"))
                    {
                        string service = msgData["Service"].ToString();
                        switch(service)
                        {
                            case "ReadyToPlay":
                                response = ReadyToPlayRequest.Get(curUser, msgData);
                                break;
                            case "SendMove":
                                response = SendMoveRequest.Get(curUser, msgData);
                                break;
                            case "StopGame":
                                response = StopGameRequest.Get(curUser, msgData);
                                break;
                            case "GetRoomsInRange":
                                response = RoomInRangeRequest.Get(curUser, msgData);
                                break;
                            case "CreateTurnRoom":
                                   response = CreateTurnRoom.Get(curUser, msgData);
                                break;
                            case "GetLiveRoomInfo":
                                response = LiveRoomInfoRequest.Get(curUser, msgData);
                                break;
                            case "JoinRoom":
                                response = JoinRoomRequest.Get(curUser, msgData);
                                break;
                            case "StartGame":
                                response = StartGameRequest.Get(curUser, msgData);
                                break;
                            case "LeaveRoom":
                                response = LeaveRoomRequest.Get(curUser, msgData);
                                break;
                            case "SubscribeRoom":
                                response = SubscribeRoomRequest.Get(curUser, msgData);
                                break;
                            case "SendChat":
                                if(msgData.ContainsKey("Message"))
                                    response = SendChatRequest.Get(curUser, msgData["Message"].ToString());
                                break;
                            case "CancelMatching":
                                    response = CancelMatchingRequest.Get(curUser, msgData["Message"].ToString());
                                break;

                        }
                    }
                    else Console.WriteLine("No service is sent");
                }
                else Console.WriteLine("No user exist");
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            string retData = JsonLogic.Serialize(response);
            SendMessage(Session, retData);
        }

        private static void SendMessage(IWebSocketSession Session, string Message)
        {
            if (Session.ConnectionState == WebSocketSharp.WebSocketState.Open)
                Session.Context.WebSocket.Send(Message);
        }
    }
}
