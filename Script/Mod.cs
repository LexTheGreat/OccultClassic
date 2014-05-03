using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OccultClassic.Script.Mods
{
	public class Mod
    {
		public string Name;

        public Mod(string name)
        {
            if (File.Exists("Content/mods/" + name + "/init.lua")) {
				Name = name; Init();
            } else 
				Name = "Unknown Mod";
        }

		public void Init()
        {        
			LuaManager.ExecuteFile(Name + "/init.lua");
        }

		public void Reload()
        {
			LuaManager.ExecuteFile(Name + "/init.lua");
        }
    }
}
