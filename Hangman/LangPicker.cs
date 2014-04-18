using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman
{
    class LangPicker
    {
        ConsoleKeyInfo langPick;

        Game ogame;

        public byte lang;
        bool picked;
        string text = "Press 1 for English, 2 for Czech, 3 for Spanish";

        public void DrawStart()
        {
            picked = false;
            Console.Clear();
            Game.DrawText(Game.CenterText(text));
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
                        Console.WriteLine(Game.CenterText(text));
                        break;
                }
            }
            GoStartGame();
        }

        void GoStartGame()
        {
           
            picked = false;
            while (!picked)
            {
                ogame = new Game(lang);
                if (!ogame.DecideNewGame())
                {
                    picked = true;
                }                
            }
        }
    }
}
