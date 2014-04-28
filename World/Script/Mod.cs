using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace OccultClassic.World.Mods
{
    class Mod
    {
        public String _name;
        public File _init;

        public Mod(string name)
        {
            if (File.Exists("Content/mods/" + name + "/init.lua")) {
                this._name = name; init();
            } else this._name = "Unknown Mod";
        }

        public void init()
        {

        }

        public void reload()
        {

        }
    }
}
