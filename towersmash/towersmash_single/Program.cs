using System;

namespace towersmash
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (towersmash game = new towersmash())
            {
                game.Run();
            }
        }
    }
}

