using System;
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
using OccultClassic.World.Script;
using Gwen.Control;

namespace OccultClassic.States
{
	public class Game : StateGUI
	{
		Camera mapCamera;
		LightEngine lights;
		Texture lightTexture, character;
		Font font;
		float dt;

		public Game(GameWindow game, string GuiImagePath, string FontName, int FontSize) 
			: base(game, GuiImagePath, FontName, FontSize)
		{
			InitializeTileEngine ();

			InitializeInput ();

			MapManager.Maps.Add ("map", new Map (mapCamera, "Content/map.tmx"));
			MapManager.LocalIndex = "map";

			InitializeLights ();

			PlayerManager.Players.Add (1, new Player (MapManager.Local, "Thomas", character, font));
			PlayerManager.LocalIndex = 1;

			PlayerManager.Local.Position = new Vector2 (
				GraphicsDevice.Size.X / 2f - 16f,
				GraphicsDevice.Size.Y / 2f - 16f);
			PlayerManager.Local.MoveSpeed = 5f;
			PlayerManager.Local.Direction = Vector2.Zero;

			mapCamera.Follow = PlayerManager.Local;

			LuaManager.hook.Call("onGameLoad");
		}
		
		public override void Draw (SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			base.Draw (spriteBatch, effects);

			// Tiled Map renderer begins and ends spriteBatch internally
			spriteBatch.Draw (MapManager.Local);

			spriteBatch.Begin (SpriteBlendMode.Alpha, SpriteSortMode.FrontToBack, mapCamera.Transform);

			foreach (var obj in MapManager.Local.Objects)
				spriteBatch.Draw (obj);

			PlayerManager.Draw (spriteBatch, effects);

			spriteBatch.End ();

			GraphicsDevice.Draw (lights);
		}

		public override void LoadContent(ContentManager content)
		{
			lightTexture = content.Get<Texture> ("lightmask");
			font = content.Get<Font> ("OccultClassic");
			character = content.Get<Texture> ("characters");
		}

		public override void Update(GameTime gameTime)
		{
			dt = (float)gameTime.ElapsedGameTime.Milliseconds;
			base.Update (gameTime);
			PlayerManager.Update (gameTime);
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

		private void InitializeLights()
		{
			lights = new LightEngine (mapCamera, lightTexture);
			lights.Add (new Light (new Vector2 (GraphicsDevice.Size.X, GraphicsDevice.Size.Y) / 2, 1.5f, Color.White, false));

			foreach(var obj in MapManager.Local.Objects)
			{
				if (obj.Type == "Light") lights.Add (new Light (
					obj.Position + 16f,
					float.Parse(obj.Properties ["Ratio"]),
					Color.Gold));
			}
		}

		private void InitializeInput()
		{
			GameInput.Add("Down");
			GameInput["Down"].Add (Keyboard.Key.S);
			GameInput["Down"].Add (Keyboard.Key.Down);
			GameInput["Down"].OnHold += () => {
				PlayerManager.Local.Sprite.Play("Down");
				PlayerManager.Local.Direction = new Vector2(0, 1);
			};
			GameInput["Down"].OnRelease += () => {
				PlayerManager.Local.Sprite.Stop();
				PlayerManager.Local.Sprite.SourceRect = new Rectangle (128, 0, 32, 32);
				PlayerManager.Local.Direction = Vector2.Zero;
			};

			GameInput.Add("Up");
			GameInput["Up"].Add (Keyboard.Key.W);
			GameInput["Up"].Add (Keyboard.Key.Up);
			GameInput["Up"].OnHold += () => {
				PlayerManager.Local.Sprite.Play("Up");
				PlayerManager.Local.Direction = new Vector2(0, -1);
			};
			GameInput["Up"].OnRelease += () => {
				PlayerManager.Local.Sprite.Stop();
				PlayerManager.Local.Sprite.SourceRect = new Rectangle (128, 96, 32, 32);
				PlayerManager.Local.Direction = Vector2.Zero;
			};

			GameInput.Add("Left");
			GameInput["Left"].Add (Keyboard.Key.A);
			GameInput["Left"].Add (Keyboard.Key.Left);
			GameInput["Left"].OnHold += () => {
				PlayerManager.Local.Sprite.Play("Left");
				PlayerManager.Local.Direction = new Vector2(-1, 0);
			};
			GameInput["Left"].OnRelease += () => {
				PlayerManager.Local.Sprite.Stop();
				PlayerManager.Local.Sprite.SourceRect = new Rectangle (128, 32, 32, 32);
				PlayerManager.Local.Direction = Vector2.Zero;
			};

			GameInput.Add("Right");
			GameInput["Right"].Add (Keyboard.Key.D);
			GameInput["Right"].Add (Keyboard.Key.Right);
			GameInput["Right"].OnHold += () => {
				PlayerManager.Local.Sprite.Play("Right");
				PlayerManager.Local.Direction = new Vector2(1, 0);
			};
			GameInput["Right"].OnRelease += () => {
				PlayerManager.Local.Sprite.Stop();
				PlayerManager.Local.Sprite.SourceRect = new Rectangle (128, 64, 32, 32);
				PlayerManager.Local.Direction = Vector2.Zero;
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

			GameInput.Add("RotateRight");
			GameInput["RotateRight"].Add (Keyboard.Key.R);
			GameInput["RotateRight"].OnHold += () => {
				mapCamera.Rotation += 0.2f;
			};

			GameInput.Add("RotateLeft");
			GameInput["RotateLeft"].Add (Keyboard.Key.T);
			GameInput["RotateLeft"].OnHold += () => {
				mapCamera.Rotation -= 0.2f;
			};
		}
	}
}