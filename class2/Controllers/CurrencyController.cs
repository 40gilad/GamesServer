using LobbyServer.Managers;
using LobbyServer.Models;
using LobbyServer.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{

    [Route("api/")]
    [ApiController]
    public class CurrencyController : Controller
    {
        [HttpPut("addCurrency")]

        public Dictionary<string, object> AddCurrency([FromBody] Dictionary<string, object> data)
        {
            /*
             {
                  "Email": "ka@ki.com",
                  "Currency": "150"
              }
             */
            Dictionary<string, object> ret = new Dictionary<string, object>();
            ret.Add("Response: ", "AddCurrency");
            try
            {
                PrintService.Print(txt: "AddCurrency", put: true);

                if (data.ContainsKey("Email") && data.ContainsKey("Currency"))
                {
                    string user_id = UserIdGenerator.ToId(data["Email"].ToString());
                    User curr = LoginManager.Instance.GetUser(user_id);
                    int money = int.Parse(data["Currency"].ToString());
                    int money_after_add = curr.money + money;
                    curr.money = money_after_add;
                    LoginManager.Instance.UpdateUser(curr);
                    ret.Add("Currency: ", money_after_add.ToString());
                }
            }
            catch (Exception e) { ret.Add("Exception Message", e); }
            return ret;
        }

    }
}
