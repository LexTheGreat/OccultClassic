using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OccultClassic.World.Script;

namespace OccultClassic.World.Mods
{
    class Mod
    {
        public String _name;
        public Mod(string name)
        {
            if (File.Exists("Content/mods/" + name + "/init.lua")) {
                this._name = name; init();
            } else this._name = "Unknown Mod";
        }

        public void init()
        {        
            LuaManager.ExecuteFile(_name + "/init.lua");
        }

        public void reload()
        {
            LuaManager.ExecuteFile(_name + "/init.lua");
        }
    }
}
