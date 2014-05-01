using NLua;
using KeraLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OccultClassic.World.Script.LuaCore
{
    class Hooks
    {
        private List<KeyValuePair<String, List<Object>>> Table = new List<KeyValuePair<String, List<Object>>>();
        public void Add(String Hook, String Name, LuaFunction lf)
        {
            Table.Add(new KeyValuePair<String, List<Object>>(Hook, new List<Object>() { Name, lf }));
        }

        public void Remove(String Hook, String Name)
        {
            foreach (var pair in Table)
                if (Hook == pair.Key && (String)pair.Value[0] == Name) { Table.Remove(pair); break; }
        }

        public void Call(String Hook, params Object[] args)
        {
            foreach (KeyValuePair<String, List<Object>> pair in Table.ToList())
            {
                if (Hook == pair.Key)
                {
                    LuaFunction func = (LuaFunction)pair.Value[1];
                    try { if (args != null) { func.Call(args); } else { func.Call(); } }
                    catch (Exception ex) { LuaManager.Utility.cPrint(ex.ToString()); }
                };
            }
        }

        public void Call(String Hook)
        {
            Call(Hook, null);
        }
    }
}
