using GameServerShenkar.Globals;
using GameServerShenkar.Handlers;
using System;
using WebSocketSharp.Server;


namespace GameServerShenkar
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WebSocketServer server = new WebSocketServer(GlobalVariables.ServerUrl);
            server.AddWebSocketService<GameServerHandler>("/GameServer");
            server.Start();

            Console.WriteLine("Main Live Server: " + GlobalVariables.ServerUrl);
            Console.ReadKey();
            server.Stop();
        }
    }
}
