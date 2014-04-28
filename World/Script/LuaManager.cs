using NLua;
using NLua.Exceptions;
using OccultClassic.World.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OccultClassic.World.Script
{
    class LuaManager
    {
        public static Lua LuaObject = new Lua();

        public static void ExecuteFile(String File)
        {
            if (System.IO.File.Exists("Content/mods/" + File))
            {
                try
                { LuaObject.DoFile("Content/mods/" + File); }
                catch (LuaException ex)
                { }
            }
            else
            {
                // log (File + " Not found");
            }
        }
    }
}
