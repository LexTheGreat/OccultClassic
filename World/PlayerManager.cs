using System.Collections.Generic;
using Spooker;
using Spooker.Graphics;
using Spooker.Time;

namespace OccultClassic.World
{
	public class PlayerManager
	{
		public static int LocalIndex;
		public static Dictionary<int, Player> Players = new Dictionary<int, Player> ();

		public static Player Local
		{
			get { return Players [LocalIndex]; }
		}

		public static void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			foreach (var player in Players.Values)
				player.Draw (spriteBatch, effects);
		}

		public static void Update(GameTime gameTime)
		{
			foreach (var player in Players.Values)
				player.Update (gameTime);
		}
	}
}