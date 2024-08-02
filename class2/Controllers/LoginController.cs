using LobbyServer.Managers;
using LobbyServer.Models;
using LobbyServer.Services;
using LobbyServer.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{

    [Route("api/")]
    [ApiController]
    public class LoginController : Controller
    {
        public string game_server_uri = "ws://localhost:7890/GameServer";
        int start_diamonds_amount = 10;
        int start_rating = 500;
        #region Get User
        [HttpGet("Login/{Email}&{Password}")]
        public Dictionary<string, object> Login(string Email, string password)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            PrintService.Print(txt: "GetUser with id " + Email, get: true);
            User user = LoginManager.Instance.GetUser(UserIdGenerator.ToId(Email));

            ret.Add("Response", "Login");
            if (user != null)
            {

                bool is_password_match = (user.password == password);
                ret.Add("IsLoggedIn", is_password_match);



                if (!is_password_match)
                    ret.Add("Message", "Password incorrect");
                else
                { // if password match, calculate daily bonus and update in db
                    DailyBonusManager.Instance.DailyBouns(user: user);
                    LoginManager.Instance.UpdateUser(user);
                }

                ret.Add("UserId", user.id);
                ret.Add("UserName",user.user_name);
                ret.Add("GameServerUrl", game_server_uri);
                PrintService.Print(txt: "Returns User " + user.email);
                return ret;
            }
            ret.Add("Message", "User " + Email + " not exist");
            ret.Add("IsLoggedIn", "false");
            PrintService.Print(txt: "Returned error: User" + Email + " not exist ");
            return ret;
        }

        #endregion

        #region Post User
        [HttpPost("Register")]
        public Dictionary<string, object> Register([FromBody] Dictionary<string, object> data)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("Response: ", "Regiser");
            PrintService.Print(txt: "UserRegiser", post: true);

            if (data.ContainsKey("Email") && data.ContainsKey("Password"))
            {
                string u_mail = data["Email"].ToString();
                string u_password = data["Password"].ToString();
                string u_id = UserIdGenerator.ToId(u_mail);
                string user_name = UserNameManager.Instance.GenrateUserName();
                int diamonds = start_diamonds_amount;
                string lli = DateTime.UtcNow.ToString();

                // // FOR QA ONLY- sets register time to 26 hours ago
                // // Subtract 26 hours using a TimeSpan
                // DateTime timeMinus26Hours = DateTime.UtcNow.Subtract(new TimeSpan(26, 0, 0));
                // string lli = timeMinus26Hours.ToString("yyyy-MM-dd HH:mm:ss");

                User curr = new User(id: u_id, mail: u_mail, password: u_password, diamonds: diamonds,
                    user_name: user_name, lli: lli);
                RedisService.SetUserRating(u_id, start_rating.ToString());

                if (LoginManager.Instance.AddUser(curr))
                {//new user has added
                    PrintService.Print(txt: "User " + data["Email"] + " Added to UserManeger");
                    ret.Add("Message", "User " + data["Email"] + " Added to UserManeger");
                }
                else
                {
                    ret.Add("Message", " User is null or already in the system");
                    PrintService.Print(txt: "User " + data["Email"] + ", " + " is null or already in the system");
                }

            }
            return ret;
        }


        #endregion

    }
}
