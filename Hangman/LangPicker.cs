using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class LangPicker
    {
        ConsoleColor barva;
        ConsoleKeyInfo langPick;

        Game ogame;

        public byte lang;
        bool picked;
        string text = "Press 1 for English, 2 for Czech, 3 for Spanish";
        string title;


        public void DrawStart()
        {
            picked = false;
            Console.Clear();
            Game.DrawText(text);
        }
        
        public void StartLangPicker()
        {
            DrawStart();
            while (!picked)
            {
                langPick = Console.ReadKey();
                switch (langPick.Key)
                { 
                    case ConsoleKey.NumPad1:
                        lang = 1;
                        picked = true;
                        break;

                    case ConsoleKey.NumPad2:
                        lang = 2;
                        picked = true;
                        break;
                    case ConsoleKey.NumPad3:
                        lang = 3;
                        picked = true;                        
                        break;
                    default: 
                        picked = false;
                        Console.Clear();
                        Console.WriteLine(text);
                        break;
                }
            }
            GoStartGame();
        }

        void GoStartGame()
        {
            switch (lang)
            {
                case 1:
                    title = "ENGLISH";
                    barva = ConsoleColor.Red;
                    break;
                case 2:
                    title = "ČESKY";
                    barva = ConsoleColor.Blue;
                    break;
                case 3:
                    title = "ESPAÑOL";
                    barva = ConsoleColor.DarkYellow;
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("You should not be seeing this, report to dev");
                    break;
            }
            picked = false;
            while (!picked)
            {
                ogame = new Game(title, barva, lang);
                if (!ogame.DecideNewGame())
                {
                    picked = true;
                }                
            }
        }
    }
}
