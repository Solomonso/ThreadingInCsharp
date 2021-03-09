using System;

namespace ThreadingInCsharp
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Global())
                game.Run();
        }
    }
}
