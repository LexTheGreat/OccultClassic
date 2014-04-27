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
using OccultClassic.World;

namespace OccultClassic.States
{
	public class Game : StateGUI
	{
		Map mapRenderer;
		Camera mapCamera;
		LightEngine lights;

		// speed with what is character moving
		const float MoveSpeed = 1f;

		public Game(GameWindow game, string GuiImagePath, string FontName, int FontSize) 
			: base(game, GuiImagePath, FontName, FontSize)
		{
			InitializeTileEngine ();

			InitializeLights ();

			InitializeInput ();

			PlayerManager.Players.Add (1, new Player (mapCamera, "Thomas", Content.Get<Texture> ("characters"), Content.Get<Font> ("CONSOLA")));
			PlayerManager.LocalIndex = 1;

			PlayerManager.LocalPlayer.Position = new Vector2 (
				GraphicsDevice.Size.X / 2f - 16f,
				GraphicsDevice.Size.Y / 2f - 16f);
		}

		public override void Draw (SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			base.Draw (spriteBatch, effects);

			spriteBatch.Begin ();

			foreach (var obj in mapRenderer.Objects)
				spriteBatch.Draw (obj);

			PlayerManager.Draw (spriteBatch, effects);

			spriteBatch.Draw (
				Content.Get<Font> ("CONSOLA"),
				"Local player position: " + PlayerManager.LocalPlayer.Position + " Camera position: " + mapCamera.Position,
				12, new Vector2 (10, 10),
				Color.White,
				Vector2.One,
				Vector2.Zero,
				0f,
				Text.Styles.Regular);

			spriteBatch.End ();

			// GraphicsDevice.Draw (lights);
		}

		public override void Update(GameTime gameTime)
		{
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
			mapCamera.Smoothness = 0.005f;
			mapCamera.Smooth = true;
			Components.Add (mapCamera);

			mapRenderer = new Map(mapCamera, "Content/map.tmx");
			Components.Add (mapRenderer);
		}

		private void InitializeLights()
		{
			lights = new LightEngine (mapCamera, Content.Get<Texture> ("lightmask"));
			lights.Add (new Light (new Vector2 (GraphicsDevice.Size.X, GraphicsDevice.Size.Y) / 2, 1.5f, Color.White, false));

			foreach(var obj in mapRenderer.Objects)
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
				PlayerManager.LocalPlayer.Sprite.Play("Down");
				PlayerManager.LocalPlayer.Move(mapRenderer, new Vector2(0, MoveSpeed));
			};
			GameInput["Down"].OnRelease += () => {
				PlayerManager.LocalPlayer.Sprite.Stop();
				PlayerManager.LocalPlayer.Sprite.SourceRect = new Rectangle (128, 0, 32, 32);
			};

			GameInput.Add("Up");
			GameInput["Up"].Add (Keyboard.Key.W);
			GameInput["Up"].Add (Keyboard.Key.Up);
			GameInput["Up"].OnHold += () => {
				PlayerManager.LocalPlayer.Sprite.Play("Up");
				PlayerManager.LocalPlayer.Move(mapRenderer, new Vector2(0, -MoveSpeed));
			};
			GameInput["Up"].OnRelease += () => {
				PlayerManager.LocalPlayer.Sprite.Stop();
				PlayerManager.LocalPlayer.Sprite.SourceRect = new Rectangle (128, 96, 32, 32);
			};

			GameInput.Add("Left");
			GameInput["Left"].Add (Keyboard.Key.A);
			GameInput["Left"].Add (Keyboard.Key.Left);
			GameInput["Left"].OnHold += () => {
				PlayerManager.LocalPlayer.Sprite.Play("Left");
				PlayerManager.LocalPlayer.Move(mapRenderer, new Vector2(-MoveSpeed, 0));
			};
			GameInput["Left"].OnRelease += () => {
				PlayerManager.LocalPlayer.Sprite.Stop();
				PlayerManager.LocalPlayer.Sprite.SourceRect = new Rectangle (128, 32, 32, 32);
			};

			GameInput.Add("Right");
			GameInput["Right"].Add (Keyboard.Key.D);
			GameInput["Right"].Add (Keyboard.Key.Right);
			GameInput["Right"].OnHold += () => {
				PlayerManager.LocalPlayer.Sprite.Play("Right");
				PlayerManager.LocalPlayer.Move(mapRenderer, new Vector2(MoveSpeed,0));
			};
			GameInput["Right"].OnRelease += () => {
				PlayerManager.LocalPlayer.Sprite.Stop();
				PlayerManager.LocalPlayer.Sprite.SourceRect = new Rectangle (128, 64, 32, 32);
			};
		}
	}
}