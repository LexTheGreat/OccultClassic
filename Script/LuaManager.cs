using OccultClassic.Script.Mods;
using OccultClassic.Script.LuaCore;
using NLua;
using NLua.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OccultClassic.Script
{
	public static class LuaManager
    {
        public static Lua LuaObject = new Lua();
        public static ModManager ModManager = new ModManager();
        public static Utilities Utility = new Utilities();
		public static Hooks Hook = new Hooks();

		public static void Init() { 
            ModManager.Mods.Add(0, new Mod("")); // Defalt error mod, returns 'Unknown Mod'

            LuaObject["util"] = Utility;
			LuaObject["hook"] = Hook;

            string[] Mods = Directory.GetDirectories(@"Content/mods/", "*", System.IO.SearchOption.TopDirectoryOnly);
            foreach (string f in Mods)
                ModManager.Add(f.Remove(0, 13));
        }

		public static void ExecuteFile(string file)
        {
			if (System.IO.File.Exists("Content/mods/" + file))
            {
                try
				{ LuaObject.DoFile("Content/mods/" + file); }
                catch (LuaException ex)
                { Utility.cPrint("LuaException:\n" + ex.ToString().Remove(0, 36), "Red"); }
            }
            else
            {
				Utility.cPrint("File not found:\n" + file, "Red");
            }
        }
    }
}
