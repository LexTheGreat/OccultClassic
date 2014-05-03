using System;
using Spooker.Core;
using Spooker.Content;
using OccultClassic.States;
using OccultClassic.Script;

namespace OccultClassic
{
	public class Main : GameWindow
	{
		public Main (GameSettings gameSettings) : base(gameSettings)
		{
			LuaManager.Init();

			//Trigger main menu
			StateFactory.SetState (
				new Menu(
					this,
					"Content/textures/gui/skin.png",
					"Content/fonts/OccultClassic.ttf",
					15));
		}

		public override void LoadContent(ContentManager content)
		{
			content.AddLoaders (ContentProvider.Default);
		}
	}
}