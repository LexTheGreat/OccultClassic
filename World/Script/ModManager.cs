using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace OccultClassic.World.Mods
{
    class ModManager
    {
        
        // Content/mods/
        public static int LocalIndex;
        public static Dictionary<int, Mod> Mods = new Dictionary<int, Mod>();

        public ModManager()
        {
            Mods.Add(0, new Mod(""));
        }

        public static void Update()
        {
            foreach (var Mod in Mods.Values)
                Mod.reload();
        }

        public static Mod Mod(int index)
        {
            if (Mods.ContainsKey(index))
                return Mods[index]; else return Mods[0];
        }

        public static Mod Mod(string name)
        {
            return Mods[0];
        }

        public static void Add(string name)
        {
            var mod = new Mod(name);
            mod._name = name;

        }
    }
}
