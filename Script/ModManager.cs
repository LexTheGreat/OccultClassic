using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace OccultClassic.Script.Mods
{
	public class ModManager
    {
        public Dictionary<int, Mod> Mods = new Dictionary<int, Mod>();

        public void Update()
        {
            foreach (var Mod in Mods.Values)
				Mod.Reload();
        }

        public Mod Mod(int index)
        {
            if (Mods.ContainsKey(index))
                return Mods[index]; else return Mods[0];
        }

        public Mod Mod(string name)
        {
            foreach (var Mod in Mods.Values)
				if (Mod.Name == name) return Mod;

            return Mods[0];
        }

        public void Add(string name)
        {
            var mod = new Mod(name);
            Mods.Add(Mods.ToArray().Length + 1, mod);
        }
    }
}
