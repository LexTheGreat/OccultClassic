using System;
using System.Collections.Generic;
using Spooker;
using Spooker.Graphics;
using Spooker.Graphics.Animations;
using Spooker.Graphics.TiledMap;
using Spooker.Time;

namespace OccultClassic.World
{
	public class Player : IDrawable, IUpdateable
	{
		public Text Name;
		public FrameAnimator Sprite;
		private float _elapsedTime;

		public Vector2 Position
		{
			get { return Sprite.Position; }
			set { 
				Sprite.Position = value; 
				Name.Position = new Vector2 (value.X + Sprite.SourceRect.Width / 2 - Name.Size.X / 2,
					value.Y - Sprite.SourceRect.Height);
			}
		}

		public Player (string name, Texture texture, Font font)
		{
			Name = new Text (font);
			Name.String = name;
			Name.CharacterSize = 12;

			Sprite = new FrameAnimator (texture);

			Sprite.Add ("Down");
			Sprite["Down"].Duration = TimeSpan.FromSeconds (1);
			Sprite["Down"].Add (new Rectangle (96, 0, 32, 32));
			Sprite["Down"].Add (new Rectangle (160, 0, 32, 32));

			Sprite.Add ("Up");
			Sprite["Up"].Duration = TimeSpan.FromSeconds (1);
			Sprite["Up"].Add (new Rectangle (96, 96, 32, 32));
			Sprite["Up"].Add (new Rectangle (160, 96, 32, 32));

			Sprite.Add ("Left");
			Sprite["Left"].Duration = TimeSpan.FromSeconds (1);
			Sprite["Left"].Add (new Rectangle (96, 32, 32, 32));
			Sprite["Left"].Add (new Rectangle (160, 32, 32, 32));

			Sprite.Add ("Right");
			Sprite["Right"].Duration = TimeSpan.FromSeconds (1);
			Sprite["Right"].Add (new Rectangle (96, 64, 32, 32));
			Sprite["Right"].Add (new Rectangle (160, 64, 32, 32));

			Sprite.SourceRect = new Rectangle (128, 0, 32, 32);
		}

		public void Move(Map map, Vector2 position)
		{
			var offset = Position - position;
			var newPos = Position + (offset * _elapsedTime);
			var rect = new Rectangle (
				(int)newPos.X - 12, (int)newPos.Y - 4, 24, 24);
			var canMove = true;

			foreach (var obj in map.Objects)
			{
				if (obj.Intersects(rect))
				{
					canMove = false;
					break;
				}
			}

			if (!map.Bounds.Intersects (rect)) canMove = false;
			if (canMove) Position = newPos;
		}

		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			Sprite.Draw (spriteBatch, effects);
			Name.Draw (spriteBatch, effects);
		}

		public void Update(GameTime gameTime)
		{
			_elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			Sprite.Update (gameTime);
		}
	}
}