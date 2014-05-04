using System;
using Gwen.Control;
using Spooker;
using Spooker.GameStates;
using Spooker.Time;
using Spooker.Graphics;
using Spooker.Core;
using Spooker.Content;
using Spooker.Network;
using SFML.Window;
using OccultClassic.Script;

namespace OccultClassic.States
{
	public class Menu : StateGUI
	{
		private Sprite background;
		private Sprite logo;
		private Sprite overlay;

		public Menu(GameWindow game, string GuiImagePath, string FontName, int FontSize) 
			: base(game, GuiImagePath, FontName, FontSize)
		{
			LuaManager.Hook.Call("onMenuLoad");
		}

		public override void Enter()
		{
			Audio.PlayMusic ("Content/music/amanita.it.ogg");
		}

		public override void LoadGUI(Canvas gameGUI)
		{
			var lblElapsed = new Label (gameGUI);
			lblElapsed.Text = "waiting...";
			lblElapsed.SetPosition (10, 10);
			Gwen.GuiManager.Set<Label>("Elapsed", lblElapsed);

			var btnCreate = new Button(gameGUI);
			btnCreate.Text = "Create game";
			btnCreate.SetPosition(195, 550);
			btnCreate.SetSize(200, 40);
			btnCreate.SetImage("Content/textures/gui/buttons/create.png");
			btnCreate.Clicked += btnCreate_Clicked;

			var btnJoin = new Button(gameGUI);
			btnJoin.Text = "Join game";
			btnJoin.SetPosition(btnCreate.X + btnCreate.Width + 10, btnCreate.Y);
			btnJoin.SetSize(btnCreate.Width, btnCreate.Height);
			btnJoin.SetImage("Content/textures/gui/buttons/join.png");
		}

		public override void LoadContent(ContentManager content)
		{
			background = new Sprite(content.Get<Texture> ("gui/background"));
			logo = new Sprite(content.Get<Texture> ("gui/logo"));
			logo.Position = (new Vector2 (GraphicsDevice.Size.X, GraphicsDevice.Size.Y) / 2) - (logo.Texture.Size / 2);
			overlay = new Sprite(content.Get<Texture> ("gui/overlay"));
		}

		public override void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			base.Draw (spriteBatch, effects);

			spriteBatch.Begin ();
			spriteBatch.Draw(background);
			spriteBatch.Draw(logo);
			spriteBatch.Draw(overlay);
			spriteBatch.End();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update (gameTime);
			Gwen.GuiManager.Get<Label> ("Elapsed").Text = "Elapsed: " + gameTime.ElapsedGameTime.Seconds + "s";
		}

		private void btnCreate_Clicked(Base control, EventArgs args)
		{
			StateFactory.SetState (new Game (
				Game,
				"Content/textures/gui/skin.png",
				"Content/fonts/OccultClassic.ttf",
				15));
		}
	}
}