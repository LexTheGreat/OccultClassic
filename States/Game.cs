using System;
using System.Collections.Generic;
using SFML.Window;
using Spooker;
using Spooker.Core;
using Spooker.GameStates;
using Spooker.Graphics;
using Spooker.Graphics.Animations;
using Spooker.Graphics.Lights;
using Spooker.Graphics.Particles;
using Spooker.Graphics.TiledMap;
using Spooker.Time;
using Spooker.Content;
using OccultClassic.World;
using OccultClassic.Script;
using Gwen.Control;
using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;

namespace OccultClassic.States
{
	public class Game : StateGUI
	{
		Camera mapCamera;
		Font font;
		// For our Emitter test, we need both a ParticleEmitter and ParticleSystem
		List<ParticleEmitter> emitters = new List<ParticleEmitter> ();
		ParticleSystem emitterSystem;

		public Game(GameWindow game, string GuiImagePath, string FontName, int FontSize) 
			: base(game, GuiImagePath, FontName, FontSize)
		{
			MapManager.Maps.Add ("map", new Map ("Content/map.tmx"));
			MapManager.LocalIndex = "map";

			PlayerManager.Players.Add ("thomas", new Player ("Thomas", "characters"));
			PlayerManager.LocalIndex = "thomas";
			PlayerManager.Local.MoveSpeed = 50f;
			var shape = BodyFactory.CreateRectangle (MapManager.Local.Physics,
				ConvertUnits.ToSimUnits (16),
				ConvertUnits.ToSimUnits (16), 1f);
			shape.BodyType = BodyType.Dynamic;
			shape.LinearDamping = 1;
			PlayerManager.Local.CreateBody (shape);

			InitializeTileEngine ();
			InitializeInput ();

			mapCamera.Follow = PlayerManager.Local;

			LuaManager.Hook.Call("onGameLoad");
		}

		public override void Enter()
		{
			base.Enter ();
			PlayerManager.Local.Position = new Vector2 (
				GraphicsDevice.Size.X / 2f - 16f,
				GraphicsDevice.Size.Y / 2f - 16f);
		}
		
		public override void Draw (SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Begin (SpriteBlendMode.Alpha, SpriteSortMode.FrontToBack, mapCamera.Transform);
			spriteBatch.Draw (MapManager.Local);
			foreach (var obj in MapManager.Local.Objects)
				spriteBatch.Draw (obj);
			spriteBatch.End ();

			var prev = mapCamera.Rotation;
			mapCamera.Rotation = 0f;
			spriteBatch.Begin (SpriteBlendMode.Alpha, SpriteSortMode.FrontToBack, mapCamera.Transform);
			PlayerManager.Draw (spriteBatch, effects);
			spriteBatch.End ();
			mapCamera.Rotation = prev;

			base.Draw (spriteBatch, effects);
		}

		public override void LoadContent(ContentManager content)
		{
			font = content.Get<Font> ("OccultClassic");
			var lights = new LightEngine (mapCamera, content.Get<Texture> ("lightmask"));
			lights.Add (new Light (new Vector2 (GraphicsDevice.Size.X, GraphicsDevice.Size.Y) / 2, 1.5f, Color.White, false));

			emitterSystem = new ParticleSystem (Game, content.Get<ParticleSettings> ("Emitter"));
			Components.Add(emitterSystem);

			foreach(var obj in MapManager.Local.Objects)
			{
				if (obj.Properties.ContainsKey("Light"))
				{
					lights.Add (new Light (
						obj.Position + 16f,
						float.Parse (obj.Properties ["Ratio"]),
						Color.Gold));
					emitters.Add (new ParticleEmitter (
						emitterSystem,
						60,
						obj.Position - new Vector2(0, 6)));
				}
			}

			Components.Add (lights);
			PlayerManager.Local.LoadContent (content);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update (gameTime);
			MapManager.Local.Update (gameTime);
			PlayerManager.Update (gameTime);
			foreach (var emitter in emitters)
				emitter.Update (gameTime, emitter.Position, mapCamera);
            LuaManager.Hook.Call("onUpdate");
		}

		private void InitializeTileEngine()
		{
			mapCamera = new Camera (new Rectangle (
				(int)(GraphicsDevice.Size.X / 2f),
				(int)(GraphicsDevice.Size.Y / 2f),
				(int)GraphicsDevice.GetView ().Size.X,
				(int)GraphicsDevice.GetView ().Size.Y));
			Components.Add (mapCamera);
		}

		private void InitializeInput()
		{
			GameInput.Add("Down");
			GameInput["Down"].Add (Keyboard.Key.S);
			GameInput["Down"].Add (Keyboard.Key.Down);
			GameInput["Down"].OnHold += () => {
				PlayerManager.Local.Sprite.Play("Down");
				PlayerManager.Local.Direction = 1;
				PlayerManager.Local.Rotation = mapCamera.Rotation;
			};
			GameInput["Down"].OnRelease += () => {
				PlayerManager.Local.Sprite.Stop();
				PlayerManager.Local.Sprite.SourceRect = new Rectangle (128, 0, 32, 32);
				PlayerManager.Local.Direction = 0;
			};

			GameInput.Add("Up");
			GameInput["Up"].Add (Keyboard.Key.W);
			GameInput["Up"].Add (Keyboard.Key.Up);
			GameInput["Up"].OnHold += () => {
				PlayerManager.Local.Sprite.Play("Up");
				PlayerManager.Local.Direction = -1;
				PlayerManager.Local.Rotation = mapCamera.Rotation;
			};
			GameInput["Up"].OnRelease += () => {
				PlayerManager.Local.Sprite.Stop();
				PlayerManager.Local.Sprite.SourceRect = new Rectangle (128, 96, 32, 32);
				PlayerManager.Local.Direction = 0;
			};

			GameInput.Add("Left");
			GameInput["Left"].Add (Keyboard.Key.A);
			GameInput["Left"].Add (Keyboard.Key.Left);
			GameInput["Left"].OnHold += () => {
				mapCamera.Rotation += 0.2f;
			};

			GameInput.Add("Right");
			GameInput["Right"].Add (Keyboard.Key.D);
			GameInput["Right"].Add (Keyboard.Key.Right);
			GameInput["Right"].OnHold += () => {
				mapCamera.Rotation -= 0.2f;
			};

			GameInput.Add("ZoomIn");
			GameInput["ZoomIn"].Add (Keyboard.Key.Q);
			GameInput["ZoomIn"].OnPress += () => {
				mapCamera.Zoom += 0.5f;
			};

			GameInput.Add("ZoomOut");
			GameInput["ZoomOut"].Add (Keyboard.Key.E);
			GameInput["ZoomOut"].OnPress += () => {
				mapCamera.Zoom -= 0.5f;
			};
            LuaManager.LuaObject["GameInput"] = GameInput;
		}
	}
}