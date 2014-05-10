using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML.Window;
namespace OccultClassic.Script.LuaCore
{
	public class Utilities
    {
        public void cPrint(String Message)
        {
            String TimeStamp = DateTime.Now.ToString("HH:mm:ss");
            Console.Write(TimeStamp + ": ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(Message);
            Console.ResetColor();
        }
        public void cPrint(String Message, Object Color)
        {
            String[] colorNames = ConsoleColor.GetNames(typeof(ConsoleColor));
            String TimeStamp = DateTime.Now.ToString("HH:mm:ss");

            try { Message.ToString(); }
            catch (Exception) { Console.WriteLine(""); }
            foreach (string colorName in colorNames)
            {
                ConsoleColor colorNameType = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorName);
                ConsoleColor colorType = ConsoleColor.Gray;
                try { colorType = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), (String)Color, true); }
                catch { }
                if (colorNameType == colorType)
                {
                    Console.Write(TimeStamp + ": ");
                    Console.ForegroundColor = colorNameType;
                    Console.WriteLine(Message);
                    Console.ResetColor();
                    return;
                }
            }
        }
        public Keyboard.Key Key(int keyId)
        {
            return (Keyboard.Key)keyId;
        }
    }
}
