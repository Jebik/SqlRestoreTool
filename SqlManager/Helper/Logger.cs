using System;

namespace SqlManager.Tools
{
    internal static class Logger
    {
        public static void Error(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        public static void Success(string s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        public static void Warn(string s)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(s);
            Console.ResetColor();
        }

        public static void Info(string s)
        {
            Console.WriteLine(s);
        }
    }
}
