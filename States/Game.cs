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
using OccultClassic.Script;
using Gwen.Control;

namespace OccultClassic.States
{
	public class Game : StateGUI
	{
		Camera mapCamera;
		LightEngine lights;
		Texture lightTexture, character;
		Font font;
		float cameraRotation;
		float dt;

		public Game(GameWindow game, string GuiImagePath, string FontName, int FontSize) 
			: base(game, GuiImagePath, FontName, FontSize)
		{
			InitializeTileEngine ();

			InitializeInput ();

			MapManager.Maps.Add ("map", new Map (mapCamera, "Content/map.tmx"));
			MapManager.LocalIndex = "map";

			InitializeLights ();

			PlayerManager.Players.Add ("thomas", new Player (MapManager.Local, "Thomas", character, font));
			PlayerManager.LocalIndex = "thomas";

			PlayerManager.Local.Position = new Vector2 (
				GraphicsDevice.Size.X / 2f - 16f,
				GraphicsDevice.Size.Y / 2f - 16f);
			PlayerManager.Local.MoveSpeed = 160f;

			mapCamera.Follow = PlayerManager.Local;

			LuaManager.Hook.Call("onGameLoad");
		}
		
		public override void Draw (SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			base.Draw (spriteBatch, effects);

			spriteBatch.Begin (SpriteBlendMode.Alpha, SpriteSortMode.FrontToBack, mapCamera.Transform);

			spriteBatch.Draw (MapManager.Local);

			foreach (var obj in MapManager.Local.Objects)
				spriteBatch.Draw (obj);

			spriteBatch.End ();

			mapCamera.Rotation = 0f;
			spriteBatch.Begin (SpriteBlendMode.Alpha, SpriteSortMode.FrontToBack, mapCamera.Transform);
			PlayerManager.Draw (spriteBatch, effects);
			spriteBatch.End ();
			mapCamera.Rotation = cameraRotation;

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
			cameraRotation = mapCamera.Rotation;
			MapManager.Local.Update (gameTime);
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
				if (obj.Properties.ContainsKey("Light")) lights.Add (new Light (
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
				PlayerManager.Local.Direction = new Vector2(
					(float)Math.Sin(MathHelper.ToRadians(cameraRotation)),
					(float)Math.Cos(MathHelper.ToRadians(cameraRotation)));
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
				PlayerManager.Local.Direction = new Vector2(
					-(float)Math.Sin(MathHelper.ToRadians(cameraRotation)),
					-(float)Math.Cos(MathHelper.ToRadians(cameraRotation)));
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
		}
	}
}