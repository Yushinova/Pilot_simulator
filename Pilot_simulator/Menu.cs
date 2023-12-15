using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilot_simulator
{
    class Menu
    {
        public void printMenu<T>(List<T> masMenu, int punct, int X, int Y)
        {
            for (int i = 0; i < masMenu.Count; i++)
            {
                if (i == punct)
                {
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.SetCursorPosition(X, Y + i);
                Console.Write(masMenu[i]);
            }
            Console.ResetColor();
        }
        public int choiseMenu<T>(List<T> masMenu, int X, int Y)//выбор пунктов верх низ
        {
            ConsoleKey key;
            int punctNumber = 0;
            do
            {
                printMenu(masMenu, punctNumber, X, Y);
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (punctNumber > 0)
                        {
                            punctNumber--;
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (punctNumber < masMenu.Count - 1)
                        {
                            punctNumber++;
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (punctNumber == masMenu.Count)
                        {
                            return -1;
                        }
                        return punctNumber;
                        break;
                }
            } while (key != ConsoleKey.Escape);
            return -1;
        }
    }
}
