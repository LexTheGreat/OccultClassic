using System;
using Spooker.Core;
using Spooker.Content;
using OccultClassic.States;

namespace OccultClassic
{
	public class Main : GameWindow
	{
		public Main (GameSettings gameSettings) : base(gameSettings)
		{
			Content.AddLoaders (ContentProvider.Default);

			//Trigger main menu
			StateFactory.SetState (
				new Menu(
					this,
					"Content/textures/gui/skin.png",
					"Content/fonts/CONSOLA.ttf",
					15));
		}
	}
}