using Microsoft.AspNetCore.Mvc;

namespace LobbyServer.Utils
{
    public class PrintService : Controller
    {
        private static void print_method(bool get = false, bool post = false,bool put=false,bool delete=false)
        {
            if (get)
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.Write("GET:");

            }
            else if (post)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write("POST:");
            }
            else if (put)
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
                Console.Write("PUT:");
            }
            else if (delete)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write("DELETE:");
            }
            Console.ResetColor();
            Console.Write(" ");
        }

        public static void Print(string txt,ConsoleColor txt_color=ConsoleColor.White, ConsoleColor background_color=ConsoleColor.Black,
            bool get=false, bool post= false, bool put = false, bool delete = false)
        {
            print_method(get:get,post:post,put:put,delete:delete);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(DateTime.UtcNow.ToString() + ": ");
            Console.ForegroundColor = txt_color;
            Console.BackgroundColor = background_color;
            Console.WriteLine(txt);

        }
    }
}
