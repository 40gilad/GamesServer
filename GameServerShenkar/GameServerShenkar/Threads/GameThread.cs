using Armetis.Utils;
using GameServerShenkar.Globals;
using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServerShenkar.Threads
{
    public class GameThread
    {
        #region Variables

        private string matchId;
        public string MatchId {  get { return matchId; } }
        private int turnTime = GlobalVariables.TurnTime;
        public int TurnTime {get {return turnTime;} }
        private int _timeOutTime = GlobalVariables.TimeOutTime;
        private int moveCounter;
        private bool isRoomActive;


        /* FROM CLIENT VARIABLES */
        int roomId = -1;
        public int RoomId { get { return roomId; } }
        string roomName = string.Empty;
        public string RoomName { get { return roomName; } }
        string roomOwner = string.Empty;
        public string RoomOwner { get { return roomOwner; } set { roomOwner = value; } }

        string secondPlayer = string.Empty;
        public string SecondPlayer { get { return secondPlayer; } set { secondPlayer = value; } }

        int maxUsersCount = GlobalVariables.MaxPlayers;
        public int MaxUsersCount { get { return maxUsersCount; } }
        int joinedUsersCount = -1;
        public int JoindUserCount {  get { return joinedUsersCount; } set { joinedUsersCount = value; } }
        public bool IsRoomActive { get => isRoomActive; }
        private bool isDetroyThread;
        
        private int turnIndex;

        private List<string> playersOrder;
        private Dictionary<string, User> users;
        public Dictionary<string, User> Users { get { return users; } } 
        private Thread currentThread;

        private RoomTime roomTime;

        #endregion

        #region Constructor 
        public GameThread(string MatchId, MatchData CurMatchData,string _roomName)
        {

            matchId = MatchId;
            isRoomActive = false;
            moveCounter = 0;
            turnIndex = 0;
            roomTime = new RoomTime(turnTime, _timeOutTime);

            /* FROM CLIENT VARIABLES */

            roomId = GlobalVariables.GetAndIncRoomId();
            roomName = _roomName;
            roomOwner = CurMatchData.FirstPlayer;
            int joinedUsersCount = 1;

            //TODO: Init Destroy Time

            playersOrder = new List<string>();
            users = new Dictionary<string, User>();
            foreach(string userId in CurMatchData.PlayersData.Keys)
            {
                User curUser = SessionsManager.Instance.GetUser(userId);
                if (curUser != null)
                {
                    curUser.CurUserState = User.UserState.Playing;
                    curUser.MatchId = matchId;
                    SessionsManager.Instance.UpdateUser(curUser);

                    users.Add(userId,curUser);
                    playersOrder.Add(userId);
                }
            }
        }

        #endregion

        #region Thread
        public void GameLoop()
        {
            while (IsRoomActive)
            {
                try
                {
                    if (roomTime.IsCurrentTimeActive() == false)
                        ChangeTurn();

                    if (isDetroyThread && roomTime.IsRoomTimeOut() == false)
                    {
                        Console.WriteLine("Killed Room");
                        CloseRoom();
                    }

                    Thread.Sleep(500);
                }
                catch (Exception e)
                {
                    Console.WriteLine("MatchThread " + e.Message);
                    CloseRoom();
                }
            }
        }

        #endregion

        #region Requests
        public void StartGame()
        {
            turnIndex = UtilFunctions.GetRandomNumber(0,1);

            Dictionary<string, object> sendData = new Dictionary<string, object>()
            {
                {"Service","StartGame"},
                {"MI",matchId},
                {"TT",UtilFunctions.GetUtcTime()},
                {"MTT",turnTime},
                {"CP",playersOrder[turnIndex]},
                {"Players",playersOrder},
                {"MC",moveCounter}
            };

            string toSend = JsonLogic.Serialize(sendData);
            BroadcastToRoom(toSend);

            isRoomActive = true;
            isDetroyThread = true;
            currentThread = new Thread(GameLoop);
            currentThread.Start();
            roomTime.ResetTimer();
        }

        public Dictionary<string,object> ReceivedMove(User curUser,string boardIndex)
        {
            Dictionary<string, object> response = new Dictionary<string, object>();
            if (playersOrder[turnIndex] == curUser.UserId)
            {
                PassTurn();
                response = new Dictionary<string, object>()
                {
                    {"Service","BroadcastMove"},
                    {"Sender",curUser.UserId},
                    {"MoveData",boardIndex},
                    {"RoomId",roomId}
                };

                string next_turn = null;
                if (curUser.UserId == RoomOwner)
                    next_turn = secondPlayer;
                else if (curUser.UserId == secondPlayer)
                    next_turn = RoomOwner;
                response.Add("NextTurn", next_turn);

                string toSend = JsonLogic.Serialize(response);
                BroadcastToRoom(toSend);

                //TODO: Save Moves to DataBase
            }
            else response.Add("ErrorCode", GlobalEnums.ErrorCodes.NotPlayerTurn);

            return response;
        }

        //TODO: Stop Game

        #endregion

        #region Global Functions

        private void PassTurn()
        {
            moveCounter++;
            turnIndex = turnIndex == 0 ? 1 : 0;
            roomTime.ResetTimer();
        }

        private void BroadcastToRoom(string ToSend)
        {
            foreach (string userId in users.Keys)
                users[userId].SendMessage(ToSend);
        }

        private void ChangeTurn()
        {
            PassTurn();
            Dictionary<string, object> notifyData = new Dictionary<string, object>()
            {
                {"Service","PassTurn"},
                {"CP",playersOrder[turnIndex]},
                {"MC",moveCounter}
            };

            string toSend = JsonLogic.Serialize(notifyData);
            BroadcastToRoom(toSend);
        }

        public void CloseRoom()
        {
            Console.WriteLine("Closed Room " + DateTime.UtcNow.ToShortTimeString());
            isRoomActive = false;
            RoomsManager.Instance.RemoveRoom(matchId);
        }

        public Dictionary<string, object> SendChat(User curUser, string message)
        {
            Dictionary<string, object> broadcastData = new Dictionary<string, object>()
            {
                {"Service","SendChat"},
                {"Sender",curUser.UserId},
                {"RoomId",roomId},
                {"Message",message}
            };

            string toSend = JsonLogic.Serialize(broadcastData);
            BroadcastToRoom(toSend);

            return new Dictionary<string, object>();
        }
        public bool AddPlayer(string uid)
        {
            if (joinedUsersCount == 0// when opening room, joined user count = 0 althoguh it has 1 player- the one who created it
                && (secondPlayer == null || secondPlayer==string.Empty)) 
                /* if room is active, there is place for one more player and second player is indeed null */
            {
                joinedUsersCount++;
                secondPlayer = uid;
                playersOrder.Add(uid);

                /* change second user state to pre play */
                User curr = SessionsManager.Instance.GetUser(uid);
                curr.CurUserState = User.UserState.PrePlay;
                return true;
            }
            return false;
        }

        public bool AddPlayerToRoomBrodcast(string uid)
        {
            try { 
                User curr = SessionsManager.Instance.GetUser(uid);
                users.Add(uid, curr);
                return true;
                }
            catch { return false; }
        }

        public bool ReomovePlayer(string uid)
        {
            try
            {
                if (RoomOwner == uid)
                {
                    RoomOwner = secondPlayer;
                    secondPlayer = string.Empty;
                }
                else if (secondPlayer == uid)
                {
                    secondPlayer = string.Empty;
                }
                playersOrder.Remove(uid);
                joinedUsersCount--;

                if (Users.Count == 0)
                    CloseRoom();
                return true;
            }
            catch { return false; }
        }

        #endregion
    }
}
