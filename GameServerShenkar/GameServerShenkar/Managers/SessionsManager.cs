using GameServerShenkar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Managers
{
    internal class SessionsManager
    {
        private Dictionary<string, User> _usersSessions;
        public Dictionary<string, User> UsersSessions { get => _usersSessions; }

        #region Singleton

        private static SessionsManager instance;
        public static SessionsManager Instance
        {
            get
            {
                if(instance == null)
                    instance = new SessionsManager();
                return instance;
            }
        }

        #endregion

        public SessionsManager()
        {
            _usersSessions = new Dictionary<string, User>();
        }

        public void AddUser(User user)
        {
            if (_usersSessions == null)
                _usersSessions = new Dictionary<string, User>();

            if (_usersSessions.ContainsKey(user.UserId))
            {
                User storedUser = _usersSessions[user.UserId];
                if (storedUser.GetConnectionState() == WebSocketSharp.WebSocketState.Open)
                    storedUser.CloseConnection(Globals.GlobalEnums.CloseCode.DuplicateConnection, "Closed Old Connection");

                _usersSessions.Remove(storedUser.Session.ID);
                _usersSessions[user.UserId] = user;
            }
            else _usersSessions.Add(user.UserId, user);

            if (_usersSessions.ContainsKey(user.Session.ID))
                _usersSessions[user.Session.ID] = user;
            else _usersSessions.Add(user.Session.ID, user);
        }

        public void UpdateUser(User user)
        {
            if (_usersSessions == null)
                _usersSessions = new Dictionary<string, User>();

            if (_usersSessions.ContainsKey(user.UserId))
                _usersSessions[user.UserId] = user;
            else _usersSessions.Add(user.UserId, user);

            if (_usersSessions.ContainsKey(user.Session.ID))
                _usersSessions[user.Session.ID] = user;
            else _usersSessions.Add(user.Session.ID, user);
        }

        public User GetUser(string Id)
        {
            if(_usersSessions != null && _usersSessions.ContainsKey(Id))
                return _usersSessions[Id];
            else return null;
        }
    }
}
