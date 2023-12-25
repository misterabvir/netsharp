using HW1.Models;

namespace HW1.Infrastructure
{
    internal static class Text
    {
        public static void Information(string message)
        {
            Console.WriteLine(message);
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }



        public static string Input()
        {
            Console.Write("> ");
            return Console.ReadLine() ?? "empty message";
        }
    }
}
