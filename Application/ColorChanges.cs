using System;

namespace LibraryApp.UI
{
    public static class ColorChanges
    {
        public static void WriteInColor(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write($"{text}");
            Console.ResetColor();
        }
    }
}
