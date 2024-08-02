using GameServerShenkar.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GameServerShenkar.Threads
{
    internal class GameLoopThread
    {
        private bool isMatchingRunning = false;
        private Thread currentThread;
        public void StartThread()
        {
            isMatchingRunning = true;
            currentThread = new Thread(new ThreadStart(RunThread));
            currentThread.Start();
        }


        public void RunThread()
        {
            while (isMatchingRunning)
            {
                Console.WriteLine(DateTime.Now.ToString());
                GameLoopUpdate();
                Thread.Sleep(500);
            }
        }

        private void GameLoopUpdate()
        {
            Dictionary<string, GameThread> rooms = RoomsManager.Instance.ActiveRooms;
            foreach(string key in rooms.Keys)
            {
                rooms[key].GameLoop();
            }
        }
    }
}
