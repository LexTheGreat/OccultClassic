using OccultClassic.World.Mods;
using OccultClassic.World.Script;
using OccultClassic.World.Script.LuaCore;
using NLua;
using NLua.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OccultClassic.World.Script
{
    class LuaManager
    {
        public static Lua LuaObject = new Lua();
        public static ModManager ModManager = new ModManager();
        public static Utilities Utility = new Utilities();
        public static Hooks hook = new Hooks();

        public static void init() { 
            ModManager.Mods.Add(0, new Mod(""));

            LuaObject["util"] = Utility;
            LuaObject["hook"] = hook;
            ModManager.Add("FirstMod");
        }
        public static void ExecuteFile(String File)
        {
            if (System.IO.File.Exists("Content/mods/" + File))
            {
                try
                    { LuaObject.DoFile("Content/mods/" + File); }
                catch (LuaException ex)
                { Utility.cPrint("LuaException:\n" + ex.ToString().Remove(0, 36), "Red"); }
            }
            else
            {
                Utility.cPrint("File not found:\n" + File, "Red");
            }
        }

        /*
                Public Sub addFunction(ByVal luafunction As String, Optional ByVal vbfunction As String = Nothing)
            If String.IsNullOrEmpty(vbfunction) Then
                vbfunction = luafunction
            End If
            Try : LuaObject.RegisterFunction(luafunction, Commands, Commands.GetType().GetMethod(vbfunction)) : Catch ex As LuaException : Server.Writeline(ex, ConsoleColor.Red) : End Try
            Server.Writeline("Added LuaFunction |" & luafunction + "| to .NET function |" + vbfunction + "|", ConsoleColor.Green)
        End Sub

        Public Sub executeFunction(ByVal luafunction As String, ByVal ParamArray arg() As Object)
            If arg.Length >= 1 Then
                Try : LuaObject.GetFunction(luafunction).Call(arg) : Catch ex As LuaException : Server.Writeline(ex, ConsoleColor.Red) : Catch ex As Exception : End Try
            Else
                Try : LuaObject.GetFunction(luafunction).Call() : Catch ex As LuaException : Server.Writeline(ex, ConsoleColor.Red) : Catch ex As Exception : End Try
            End If
        End Sub*/
    }
}
