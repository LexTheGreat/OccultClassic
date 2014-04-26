using System;
using Gwen.Control;
using Spooker;
using Spooker.GameStates;
using Spooker.Time;
using Spooker.Graphics;
using Spooker.Core;
using SFML.Window;

namespace OccultClassic.States
{
	public class Menu : StateUi
	{
		private Sprite background;
		private Sprite logo;
		private Sprite overlay;

		public Menu(GameWindow game, string GuiImagePath, string FontName, int FontSize) 
			: base(game, GuiImagePath, FontName, FontSize)
		{
			background = new Sprite(Content.Get<Texture> ("gui/background"));
			logo = new Sprite(Content.Get<Texture> ("gui/logo"));
			logo.Position = (new Vector2 (GraphicsDevice.Size.X, GraphicsDevice.Size.Y) / 2) - (logo.Texture.Size / 2);
			overlay = new Sprite(Content.Get<Texture> ("gui/overlay"));
		}

		public override void Enter()
		{
			Audio.PlayMusic ("Content/music/amanita.it.ogg");
		}

		public override void LoadGui(Canvas gameGui)
		{
			var lblElapsed = new Label (gameGui);
			lblElapsed.Text = "waiting...";
			lblElapsed.SetPosition (10, 10);
			Gwen.GuiManager.Set<Label>("Elapsed", lblElapsed);

			var btnCreate = new Button(gameGui);
			btnCreate.Text = "Create game";
			btnCreate.SetPosition(195, 550);
			btnCreate.SetSize(200, 40);
			btnCreate.SetImage("Content/textures/gui/buttons/create.png");
			btnCreate.Clicked += btnCreate_Clicked;

			var btnJoin = new Button(gameGui);
			btnJoin.Text = "Join game";
			btnJoin.SetPosition(btnCreate.X + btnCreate.Width + 10, btnCreate.Y);
			btnJoin.SetSize(btnCreate.Width, btnCreate.Height);
			btnJoin.SetImage("Content/textures/gui/buttons/join.png");
		}

		public override void Draw(SpriteBatch spriteBatch, SpriteEffects effects = SpriteEffects.None)
		{
			spriteBatch.Begin();
			spriteBatch.Draw(background);
			spriteBatch.Draw(logo);
			spriteBatch.Draw(overlay);
			spriteBatch.End();

			base.Draw (spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{
			Gwen.GuiManager.Get<Label> ("Elapsed").Text = "Elapsed: " + gameTime.ElapsedGameTime.TotalSeconds + "s";
		}

		private void btnCreate_Clicked(Base control, EventArgs args)
		{
			StateFactory.SetState(
				new Game(
					Game,
					"Content/textures/gui/skin.png",
					"Content/fonts/CONSOLA.ttf",
					15));
		}
	}
}