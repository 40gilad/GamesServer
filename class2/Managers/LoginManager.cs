using LobbyServer.Models;
using LobbyServer.Services;

namespace LobbyServer.Managers
{
    public class LoginManager
    {
        

        #region Singleton
        public static LoginManager _instance;
        public static LoginManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LoginManager();
                return _instance;
            }
        }
        #endregion
        public LoginManager() { }

        public User GetUser(string id)
        {
            Dictionary<string, string> res_user = null;
            if (id != String.Empty)
                res_user = RedisService.GetUserDetails(id.ToString());
            else
                return null;

            if (res_user != null && res_user.Count <= 0)
                return null;

            if (res_user != null)
                return User.ToUser(res_user);
            return null;
        }

        public bool AddUser(User user)
        {
            //new user. initial diamonds amount, user_name and last_log_in date will be added as now
  

            string user_id = user.id;
            if (user == null)
                return false;

            Dictionary<string, string> temp_user = RedisService.GetUserDetails(user_id);
            if (temp_user.Count == 0) // Uid not exist
            {
                Dictionary<string, string> user_to_add = user.ToDict();
                RedisService.SetUserDetails(user_id, user.ToDict());
                return true;
            }
            else return false;
        }

        public bool UpdateUser(User user)
        {
            Dictionary<string, string> temp_user = RedisService.GetUserDetails(user.id.ToString());
            if (user == null || temp_user.Count <= 0)
                return false;
            RedisService.SetUserDetails(user.id.ToString(), user.ToDict());
            return true;
        }

        public bool DeleteUser(int id)
        {
            bool is_success = RedisService.DeleteUserDetails(id.ToString());
            if (id < 0 || !is_success)
                return false;
            return true;
        }
    }
}

