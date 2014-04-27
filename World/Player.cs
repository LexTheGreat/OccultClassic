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
		private Text _name;
		private AnimatedSprite _sprite;
		private Vector2 _position;
		private float _elapsedTime;
		private Camera _camera;

		public AnimatedSprite Sprite
		{
			get { return _sprite; }
		}

		public Vector2 Position
		{
			get { return _position; }
			set { 
				_position = value;
				_sprite.Position = _camera.Transform (value);
				_name.Position = new Vector2 (_sprite.Position.X + _sprite.SourceRect.Width / 2 - _name.Size.X / 2,
					_sprite.Position.Y - Sprite.SourceRect.Height);
			}
		}

		public Player (Camera camera, string name, Texture texture, Font font)
		{
			_camera = camera;
			_name = new Text (font);
			_name.DisplayedString = name;
			_name.CharacterSize = 12;

			_sprite = new AnimatedSprite (texture);

			_sprite.Add ("Down");
			_sprite["Down"].Duration = TimeSpan.FromSeconds (1);
			_sprite["Down"].Add (new Rectangle (96, 0, 32, 32));
			_sprite["Down"].Add (new Rectangle (160, 0, 32, 32));

			_sprite.Add ("Up");
			_sprite["Up"].Duration = TimeSpan.FromSeconds (1);
			_sprite["Up"].Add (new Rectangle (96, 96, 32, 32));
			_sprite["Up"].Add (new Rectangle (160, 96, 32, 32));

			_sprite.Add ("Left");
			_sprite["Left"].Duration = TimeSpan.FromSeconds (1);
			_sprite["Left"].Add (new Rectangle (96, 32, 32, 32));
			_sprite["Left"].Add (new Rectangle (160, 32, 32, 32));

			_sprite.Add ("Right");
			_sprite["Right"].Duration = TimeSpan.FromSeconds (1);
			_sprite["Right"].Add (new Rectangle (96, 64, 32, 32));
			_sprite["Right"].Add (new Rectangle (160, 64, 32, 32));

			_sprite.SourceRect = new Rectangle (128, 0, 32, 32);
		}

		public void Move(Map map, Vector2 offset)
		{
			var newPos = _position + offset;
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
			if (canMove) {
				Position = newPos;
				_camera.Position = newPos;
			}
		}

		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			_sprite.Draw (spriteBatch, effects);
			_name.Draw (spriteBatch, effects);
		}

		public void Update(GameTime gameTime)
		{
			_elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
			_sprite.Update (gameTime);
		}
	}
}