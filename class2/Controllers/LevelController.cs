using LobbyServer.Managers;
using LobbyServer.Models;
using LobbyServer.Services;
using LobbyServer.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{

    [Route("api/")]
    [ApiController]
    public class LevelController : Controller
    {
        [HttpPost("addXp")]
        public Dictionary<string, object> AddXp([FromBody] Dictionary<string, object> data)
        {
            /*
             {
                  "Email": "ka@ki.com",
                  "Xp": "150"
              }
             */
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("Response: ", "AddXP");
            try
            {
                PrintService.Print(txt: "AddXp", post: true);

                if (data.ContainsKey("Email") && data.ContainsKey("Xp"))
                {
                    string user_id = UserIdGenerator.ToId(data["Email"].ToString());
                    User curr=LoginManager.Instance.GetUser(user_id);
                    int xp = int.Parse(data["Xp"].ToString());
                    int xp_after_add=curr.xp+xp;
                    curr.xp = xp_after_add;
                    int new_level = 1;
                    if (xp_after_add > 100)
                    {
                        if (xp_after_add <200) { new_level = 2; }
                        else if (xp_after_add < 350) { new_level = 3; }
                        else if (xp_after_add < 575) { new_level = 4; }
                        else  { new_level = 5; }
                        curr.level = new_level;
                    }
                    LoginManager.Instance.UpdateUser(curr);
                    ret.Add("Xp: ", xp_after_add.ToString());
                    ret.Add("level: ", new_level.ToString());
                }
            }
            catch (Exception e){ ret.Add("Exception Message", e); }
            return ret;
        }

    }
}
