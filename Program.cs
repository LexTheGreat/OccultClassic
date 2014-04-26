using System;
using Spooker.Core;

namespace OccultClassic
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            /// Initialize game settings
			var gameSettings = new GameSettings("settings.xml");

			//Initialize game window
			using (Main game = new Main (gameSettings))
				game.Run();
        } 
    }
}