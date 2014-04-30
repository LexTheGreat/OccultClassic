using System;
using Spooker.Core;
using OccultClassic.World.Script;

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
            LuaManager.init();

            /// Initialize game settings
			var gameSettings = new GameSettings("settings.xml");

			//Initialize game window
			using (Main game = new Main (gameSettings))
				game.Run();
        }
    }
}