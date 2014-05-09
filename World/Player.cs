using System;
using System.Collections.Generic;
using Spooker;
using Spooker.Graphics;
using Spooker.Graphics.Animations;
using Spooker.Graphics.TiledMap;
using Spooker.Time;
using Spooker.RPG.World;
using Spooker.Content;

namespace OccultClassic.World
{
	public class Player : Creature, IDrawable, ITargetable
	{
		private Text _name;
		private AnimatedSprite _sprite;

		public float Direction;
		public float MoveSpeed;

		public AnimatedSprite Sprite
		{
			get { return _sprite; }
		}

		public Player (string name, string asset) : base(name)
		{
			Asset = asset;
			Vitals.Add ("HP", 100);
			Vitals.Add ("MP", 100);
		}

		public override void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			_sprite.Draw (spriteBatch, effects);
			_name.Draw (spriteBatch, effects);
		}

		public override void LoadContent(ContentManager content)
		{
			_name = new Text (content.Get<Font> ("OccultClassic"));
			_name.DisplayedString = Name;
			_name.CharacterSize = 12;
			_name.Origin = _name.Size / 2;

			_sprite = new AnimatedSprite (content.Get<Texture> (Asset));

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
			_sprite.Origin = new Vector2 (
				_sprite.SourceRect.Size.X / 2,
				_sprite.SourceRect.Size.Y - 4);
		}

		public override void Update(GameTime gameTime)
		{
			_sprite.Update (gameTime);
			_sprite.Position = Position;
			_name.Position = new Vector2 (
				_sprite.Position.X,
				_sprite.Position.Y - Sprite.SourceRect.Height - 6);

			var speed = MoveSpeed * (float)gameTime.ElapsedGameTime.Seconds;
			var direction = Direction * new Vector2(
				(float)Math.Sin(MathHelper.ToRadians(Rotation)),
				(float)Math.Cos(MathHelper.ToRadians(Rotation)));
			Move (speed * direction);
		}
	}
}