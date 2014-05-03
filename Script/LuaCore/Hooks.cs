using NLua;
using KeraLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OccultClassic.Script.LuaCore
{
	public class Hooks
    {
		private List<KeyValuePair<string, List<object>>> _table = new List<KeyValuePair<string, List<object>>>();

		public void Add(string hook, string name, LuaFunction lf)
        {
			_table.Add(new KeyValuePair<String, List<Object>>(hook, new List<Object>() { name, lf }));
        }

		public void Remove(string hook, string name)
        {
			foreach (var pair in _table)
				if (hook == pair.Key && (String)pair.Value[0] == name) { _table.Remove(pair); break; }
        }

		public void Call(string hook)
		{
			Call(hook, null);
		}

		public void Call(string hook, params object[] args)
        {
			foreach (KeyValuePair<String, List<Object>> pair in _table.ToList())
            {
				if (hook == pair.Key)
                {
                    LuaFunction func = (LuaFunction)pair.Value[1];
                    try { if (args != null) { func.Call(args); } else { func.Call(); } }
                    catch (Exception ex) { LuaManager.Utility.cPrint(ex.ToString()); }
                };
            }
        }
    }
}
