using GameServerShenkar.Managers;
using GameServerShenkar.Models;
using GameServerShenkar.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace GameServerShenkar.Handlers
{
    internal class GameServerHandler : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            try
            {
                if(Context.QueryString.AllKeys.Contains("data"))
                    ConnectionRequest.Get(Sessions[ID], Context.QueryString["data"]);
            }
            catch(Exception e)
            {
                Console.WriteLine("OnOpen Failed (" + e.Message + ")");
                Sessions.CloseSession(ID);
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Console.WriteLine("OnClose " + ID);
            CloseConnectionRequest.Get(ID);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            ProcessRequest.Get(Sessions[ID], e.Data);
        }

        protected override void OnError(ErrorEventArgs e)
        {
        }
    }
}
