using LobbyServer.Managers;
using LobbyServer.Utils;

namespace LobbyServer.Models
{
    public class User
    {
        private string _id;
        private string _mail;
        private string _password;
        private int _money;
        private int _xp;
        private int _level;
        private int _diamonds;
        private string _user_name;
        private string _lli; //= last log in



        public string id { get { return _id; } }
        public string email { get { return _mail; } }
        public string password { get { return _password; } }
        public int money { get { return _money; } set { _money = value; } }
        public int xp { get { return _xp; } set { _xp = value; } }
        public int level { get { return _level; } set { _level = value; } }
        public int diamonds { get { return _diamonds; } set { _diamonds = value; } }
        public string last_log_in { get { return _lli; } set { _lli = value; } }
        public string user_name { get { return _user_name; } set { _user_name = value; } }




        public User(string id, string mail, string password, int money = 0, int xp = 0, int level = 1,
            int diamonds = -1, string user_name = "", string lli = "")
        {

            _id = id;
            _mail = mail;
            _password = password;
            _money = money;
            _xp = xp;
            _level = level;
            _diamonds = diamonds;
            _lli = "";
            _user_name = user_name;
            _diamonds = diamonds;
            _lli = lli;
            
        }

        public static User ToUser(Dictionary<string, string> user_dict)
        {
            try
            {
                string id = String.Empty, password = String.Empty, user_name = String.Empty, lli = String.Empty;
                int money = -1, xp = -1, level = -1, diamonds = -1;
                string mail = string.Empty;

                if (user_dict.ContainsKey("Id") && user_dict.ContainsKey("Mail"))
                {
                    id = user_dict["Id"];
                    mail = user_dict["Mail"];
                    password = user_dict["Password"];
                    money = int.Parse(user_dict["Money"]);
                    xp = int.Parse(user_dict["XP"]);
                    level = int.Parse(user_dict["Level"]);
                    diamonds = int.Parse(user_dict["Diamonds"]);
                    user_name = user_dict["UserName"];
                    lli = user_dict["LastLogIn"];
                }

                if (id != String.Empty && mail != string.Empty)
                    return new User(id: id, mail: mail, password: password, money: money, xp: xp,
                        level: level, diamonds: diamonds, user_name: user_name, lli: lli);

            }
            catch (Exception e) { PrintService.Print("User.cs Exceotion: " + e.Message); }
            return null;
        }

        public Dictionary<string, string> ToDict()
        {
            return new Dictionary<string, string>()
            {
                {
                    "Id", _id
                },
                {
                    "Mail", _mail
                },
                {
                    "Password", _password
                },
                {
                    "LastLogIn", _lli
                },
                {
                    "UserName", _user_name
                },
                {
                    "Money", _money.ToString()
                },
                {
                    "XP", _xp.ToString()
                },
                {
                    "Level", _level.ToString()
                },
                {
                    "Diamonds", _diamonds.ToString()
                }

            };
        }
    }
}
