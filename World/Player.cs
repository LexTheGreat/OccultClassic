using System;
using System.Collections.Generic;
using Spooker;
using Spooker.Graphics;
using Spooker.Graphics.Animations;
using Spooker.Graphics.TiledMap;
using Spooker.Time;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace OccultClassic.World
{
	public class Player : IDrawable, IUpdateable, Camera.IFollowable
	{
		private Text _name;
		private AnimatedSprite _sprite;
		private Map _map;
		private Body _shape;

		public Vector2 Direction;
		public float MoveSpeed;

		public Vector2 Position
		{
			get { 
				var vec = ConvertUnits.ToDisplayUnits (_shape.Position);
				return new Vector2 (vec.X, vec.Y);
			}
			set { 
				var vec = new Vector2 (ConvertUnits.ToSimUnits (value.X), ConvertUnits.ToSimUnits (value.Y));
				_shape.Position = new Microsoft.Xna.Framework.Vector2 (vec.X, vec.Y);
			}
		}

		public AnimatedSprite Sprite
		{
			get { return _sprite; }
		}

		public Vector2 FollowPosition()
		{
			return Position;
		}

		public Player (Map map, string name, Texture texture, Font font)
		{
			_map = map;
			_name = new Text (font);
			_name.DisplayedString = name;
			_name.CharacterSize = 12;

			_sprite = new AnimatedSprite (texture);

			_sprite.Add ("Down", AnimType.Loop);
			_sprite["Down"].Duration = GameSpan.FromMilliseconds (600);
			_sprite["Down"].Add (new Rectangle (96, 0, 32, 32));
			_sprite["Down"].Add (new Rectangle (160, 0, 32, 32));

			_sprite.Add ("Up", AnimType.Loop);
			_sprite["Up"].Duration = GameSpan.FromMilliseconds (600);
			_sprite["Up"].Add (new Rectangle (96, 96, 32, 32));
			_sprite["Up"].Add (new Rectangle (160, 96, 32, 32));

			_sprite.Add ("Left", AnimType.Loop);
			_sprite["Left"].Duration = GameSpan.FromMilliseconds (600);
			_sprite["Left"].Add (new Rectangle (96, 32, 32, 32));
			_sprite["Left"].Add (new Rectangle (160, 32, 32, 32));

			_sprite.Add ("Right", AnimType.Loop);
			_sprite["Right"].Duration = GameSpan.FromMilliseconds (600);
			_sprite["Right"].Add (new Rectangle (96, 64, 32, 32));
			_sprite["Right"].Add (new Rectangle (160, 64, 32, 32));

			_sprite.SourceRect = new Rectangle (128, 0, 32, 32);
			_shape = BodyFactory.CreateRectangle (map.Physics,
				ConvertUnits.ToSimUnits (_sprite.SourceRect.Width),
				ConvertUnits.ToSimUnits (_sprite.SourceRect.Height), 1f);
			_shape.BodyType = BodyType.Dynamic;
			_shape.LinearDamping = 1;
		}

		public void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			_sprite.Draw (spriteBatch, effects);
			_name.Draw (spriteBatch, effects);
		}

		public void Update(GameTime gameTime)
		{
			_sprite.Update (gameTime);
			_sprite.Position = Position;
			_sprite.Origin = _sprite.SourceRect.Size / 2;
			_name.Position = new Vector2 (
				_sprite.Position.X,
				_sprite.Position.Y - Sprite.SourceRect.Height);
			_name.Origin = _name.Size / 2;

			var speed = MoveSpeed * (float)gameTime.ElapsedGameTime.Seconds;
			var newPos = speed * Direction;

			_shape.ApplyForce (ConvertUnits.ToSimUnits(new Microsoft.Xna.Framework.Vector2 (newPos.X, newPos.Y)));
		}
	}
}