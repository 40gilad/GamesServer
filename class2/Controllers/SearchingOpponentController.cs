using LobbyServer.Utils;
using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Controllers
{
    [Route("api/")]
    [ApiController]
    public class SearchingOpponentController : Controller
    {
        private int port = 7890;
        [HttpGet("SearchingOpponent/{userId}")]
        public Dictionary<string, object> SearchingOpponent(string userId)
        {
            PrintService.Print(txt:"user "+userId+" SearchingOpponent", get: true);

            Dictionary<string, object> ret = new Dictionary<string, object>();
            return new Dictionary<string, object>()
            {
                {"ConnectionUrl","ws://localhost:7890/GameServer"},
                {"Response","SearchingOpponent" }
            };
        }
    }
}
