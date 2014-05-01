using System.Collections.Generic;
using Spooker;
using Spooker.Graphics;
using Spooker.Graphics.TiledMap;
using Spooker.Time;

namespace OccultClassic.World
{
	public class MapManager
	{
		public static string LocalIndex;
		public static Dictionary<string, Map> Maps = new Dictionary<string, Map> ();

		public static Map Local
		{
			get { return Maps [LocalIndex]; }
		}

		public static void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Draw (Maps [LocalIndex]);
		}
	}
}