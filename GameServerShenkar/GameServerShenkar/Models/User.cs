using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using static GameServerShenkar.Globals.GlobalEnums;

namespace GameServerShenkar.Models
{
    public class User
    {
        public enum UserState
        {
            Idle = 3001 ,Matching = 3002, PrePlay = 3003,Playing = 3004
        };

        private string _userId;
        public string UserId { get { return _userId; } }

        private IWebSocketSession _session;
        public IWebSocketSession Session { get { return _session; } }

        private UserState _curUserState;
        public UserState CurUserState{ 
            get { return _curUserState; } 
            set { _curUserState = value; }
        }

        private DateTime _matchingDate;
        public DateTime MatchingDate { get { return _matchingDate; } }

        private string matchId;
        public string MatchId { get => matchId; set => matchId = value; }

        public User(string userId, IWebSocketSession session)
        {
            _userId = userId;
            _session = session;
            CurUserState = UserState.Idle;
            matchId = string.Empty;
        }
        public void SetMatchingState()
        {
            _curUserState = UserState.Matching;
            _matchingDate = DateTime.Now;
        }

        public WebSocketSharp.WebSocketState GetConnectionState()
        { return Session.ConnectionState; }

        public void CloseConnection(CloseCode _CloseCode, string _Message)
        { Session.Context.WebSocket.Close((ushort)_CloseCode, _Message); }

        public bool IsLive()
        {
            if (Session != null && Session.ConnectionState == WebSocketSharp.WebSocketState.Open)
                return true;
            return false;
        }

        public void SendMessage(string Message)
        {
            try
            {
                if(IsLive())
                  Session.Context.WebSocket.Send(Message);
                else Console.WriteLine("Socket is not open, UserId: " + UserId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
