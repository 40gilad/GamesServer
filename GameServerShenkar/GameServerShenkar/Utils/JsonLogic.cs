using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServerShenkar.Utils
{
    internal class JsonLogic
    {
        public static Dictionary<string, object> Deserialize(string _Json)
        {
            return (Dictionary<string, object>)MiniJSON.Json.Deserialize(_Json);
        }
        public static string Serialize(Dictionary<string, object> _Data)
        {
            return MiniJSON.Json.Serialize(_Data);
        }
        public static string Serialize(Dictionary<string, string> _Data)
        {
            return MiniJSON.Json.Serialize(_Data);
        }
    }
}
