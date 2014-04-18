using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hangman                                                 
{
    class Game
    {
        Random nahoda;
        ConsoleKeyInfo pressedKey;
        ConsoleColor color;

        string word = "";
        string title;
        string guessingUnderlines;
        string guessedLetter;
        string distinctWord;
        string usedLetters;

        bool isNotValid;
        bool win;
        bool validWord;
        bool spaceUnderline;

        string[] textCZ = new string[] { "Chcete použít jedno z přednastavených slov? Y/N", "Počet písmen tvého slova:", "Pro pokračování stiskni jakékoliv tlačítko.", "Prosím napište slovo pro uhodnutí: ", "Nemůžeš použít slovní spojení nebo číslice", "Napiš písmeno: ", "To nebylo písmeno", "Správně!", "Špatně", "Výhra! Chceš hrát znovu? Y/N", "Prohra! Chceš hrát znovu? Y/N", "Toto písmeno jsi už použil" };
        string[] textEN = new string[] { "Do you want to use a preset word? Y/N", "Lettercount of your word: ","Press any button to continue.", "Please enter your word: ","You can't have numbers or more than one word","Press a letter: ","You did not press a letter", "Correct!", "Wrong", "You Win! Do you want to play again? Y/N", "You Lose! Do you want to play again? Y/N","Toto písmeno jsi už použil" };
        string[] textES = new string[] { "¿Quieres a usar una palabra preestablecida? Y/N", "Cuántas letras tu palabra tiene: ", "Pulse cualquier tecla para continuar.", "Por favor introduzca su palabra: ", "No se puede tener los numeros o más de una palabra", "Escribe una letra: ", "Esto no es una letra", "¡Correcto!", "Incorrecto", "¡Usted gana! Quieres a jugar una tiempa más?", "Pierdas! Quieres a jugar una tiempa más?", "Ya ha usado esta letra!" };
        string[] text;

        string[] wordsES = new string[] { "coche", "gallina", "chimenea", "granada", "perro", "escuela", "casa", "argentina", "manzana", "lápiz" };
        string[] wordsCZ = new string[] { "stůl", "dům", "defibrilátor", "autoškola", "zmrzlina", "les", "louka", "obloha", "žirafa", "mravenec" };
        string[] wordsEN = new string[] { "candle", "tree", "forest", "pool", "washington", "hamburger", "basement", "chimney", "orange", "weapon" };
        string[] words;

        string wordMsgEN;
        string wordMsgES;
        string wordMsgCZ;
        string wordMsg;

        int count;
        int curIndex;
        int correctGuesses;

        byte lives;
        byte language;

        bool picked;
        
        void PlayGame()
        {
            correctGuesses = 0;
            Console.Clear();
            DrawHangmanTitle();
            DrawHangmanFrame();
            UnderlinesString((byte)word.Length);
            DrawGuessedLettersArea();
            DrawQuestion();
            while (correctGuesses<distinctWord.Length && lives > 0)
            TakeAGuess();
            if (lives <= 0)
            {
                DrawHangman();
                Console.SetCursorPosition(0, 21);
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, 22);
                ClearCurrentConsoleLine();
                Console.SetCursorPosition((Console.WindowWidth-wordMsg.Length)/2, 22);
                Console.Write(wordMsg);
                Console.SetCursorPosition(0, 23);
                ClearCurrentConsoleLine();
                Console.Write(CenterText(text[10]));
                win = false;
            }
            else if (correctGuesses == distinctWord.Length)
            {
                Console.SetCursorPosition(0, 21);
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, 22);
                ClearCurrentConsoleLine();
                Console.SetCursorPosition(0, 23);
                ClearCurrentConsoleLine();
                Console.Write(CenterText(text[9]));
                win = true;
            }
        }
        private void TakeAGuess()
        {
            guessedLetter = null;
            validWord = false;
            while (!validWord)
            {
                guessedLetter = Console.ReadKey().KeyChar.ToString().ToUpper();                
                if (!guessedLetter.All(Char.IsLetter))
                {
                    //zadaná hodnota není písmeno
                    DrawQuestion();
                    ClearCurrentConsoleLine();
                    Console.Write(CenterText(text[6]));
                    Console.SetCursorPosition(Console.WindowLeft / 2, 23);
                }
                else if (usedLetters.Contains(guessedLetter))
                {
                    //dané písmeno již bylo použito
                    DrawQuestion();
                    ClearCurrentConsoleLine();
                    Console.Write(CenterText(text[11]));
                    Console.SetCursorPosition(Console.WindowLeft / 2, 23);
                }
                else
                {
                    if (word.Contains(Char.Parse(guessedLetter)))
                    {
                        //zde je uhodnutí písmene
                        DrawQuestion();
                        ClearCurrentConsoleLine();
                        Console.Write(CenterText(text[7]));
                        UpdateGuessedString();
                        DrawGuessedLettersArea();
                        correctGuesses++;
                        Console.SetCursorPosition(Console.WindowLeft / 2, 23);
                        usedLetters += guessedLetter;
                        validWord = true;                        
                    }
                    else
                    {
                        //písmeno není obsaženo ve slově
                        DrawQuestion();
                        ClearCurrentConsoleLine();
                        Console.Write(CenterText(text[8]));
                        lives--;
                        DrawHangman();
                        Console.SetCursorPosition(Console.WindowLeft / 2, 23);                        
                        usedLetters += guessedLetter;
                        validWord = true;
                    }
                }
            }
        }        
        private void UpdateGuessedString()
        {
            //utváří string s uhodnutými písmenky - peklo
            count = word.Split(Char.Parse(guessedLetter)).Length - 1;
            if (count > 1)
            {
               curIndex = 0;
                if (word.IndexOf(Char.Parse(guessedLetter)) == 0 )
                {                       
                        guessingUnderlines = guessingUnderlines.Remove(0,1);
                        guessingUnderlines = guessingUnderlines.Insert(0, guessedLetter);
                        count--;
                }

                for (int i = 0; i < count; i++)
                {
                    curIndex = word.IndexOf(Char.Parse(guessedLetter), curIndex+1);
                        guessingUnderlines = guessingUnderlines.Remove(curIndex*2, 1);
                        guessingUnderlines = guessingUnderlines.Insert((curIndex * 2), guessedLetter);
                }
            }
            else
            {
                guessingUnderlines = guessingUnderlines.Remove(word.IndexOf(Char.Parse(guessedLetter))*2, 1);
                guessingUnderlines = guessingUnderlines.Insert(word.IndexOf(Char.Parse(guessedLetter))*2,guessedLetter);
            }
        }
        private void DrawGuessedLettersArea()
        {
            //draws underlines corresponding with the word being guessed
            Console.SetCursorPosition(0, 19);
            Console.Write(CenterText(guessingUnderlines));
        }
        private void UnderlinesString(byte letters)
        {
            if (guessingUnderlines == "")
            {
                spaceUnderline = true;
                for (int i = 0; i < (2 * letters); i++)
                {
                    if (spaceUnderline)
                    {
                        spaceUnderline = !spaceUnderline;
                        guessingUnderlines += "_";
                    }
                    else
                    {
                        spaceUnderline = !spaceUnderline;
                        guessingUnderlines += " ";
                    }
                }
            }
        }
        private void DrawTitle()
        {
            //smazání vrchního řádku
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.White;
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            //vykreslení nápisu jazyka
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = color;
            Console.Write(CenterText(title));
            Console.SetCursorPosition(0, 1);
            Console.ResetColor();
        }
        private void DrawHangmanTitle()
        {
            DrawTitle();
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Magenta;
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, 2);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, 3);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, 2);
            Console.WriteLine(CenterText("HANGMAN"));
        }
        private void DrawHangmanFrame()
        {
            //draw only the frame of playing area
            Console.ResetColor();
            Console.SetCursorPosition(0, 5);
            Console.WriteLine(CenterText("+----------------+"));
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine(CenterText("|                |"));
            }
            Console.WriteLine(CenterText("+----------------+"));
        }
        /// <summary>
        /// Builds the game class
        /// </summary>
        /// <param name="title">Language title</param>
        /// <param name="color">Color of the title</param>
        /// <param name="lang">Byte number of the language</param>
        public Game(string title, ConsoleColor color, byte lang)
        {
            Console.Clear();
            usedLetters = "";
            guessingUnderlines = "";
            word = "";
            lives = 8;
            curIndex = 0;
            nahoda = new Random();
            isNotValid = false;
            this.title = title;
            this.color = color;
            DrawTitle();
            //zařazení slovníku daného jazyka
            language = lang;
            if (lang == 1)
            {
                text = textEN;
                words = wordsEN;
            }
            if (lang == 2)
            {
                text = textCZ;
                words = wordsCZ;
            }
            if (lang == 3)
            {
                text = textES;
                words = wordsES;
            }
            GetWord();
        }
        public void DrawQuestion()
        {
            Console.SetCursorPosition(0, 21);
            Console.Write(CenterText(text[5]));
            Console.SetCursorPosition(Console.WindowLeft / 2, 23);
        }

        private void GetWord()
        {
            //pokud decidemode vrátí 1 vybere se náhodné slovo, pro 0 si zadává vlastní
            if (DecideMode())
            {
                word = words[nahoda.Next(0, 9)];
                word = word.ToUpper();
                Console.Clear();
                DrawTitle();
                DrawText(text[1] + word.Length);
                Console.WriteLine();
                DrawText(text[2]);
                Console.ReadKey();
                Console.Clear();
                DrawTitle();
            }
            else
            {
                Console.Clear();
                DrawTitle();
                validWord = false;
                while (!validWord)
                {
                    Console.Write(text[3]);
                    word = Console.ReadLine();
                    word = word.ToUpper();
                    //pokud znak není písmeno další pokus
                    isNotValid = !word.All(Char.IsLetter);
                    //pokud obsahuje mezeru nastává další pokus
                    if (word.Contains(" ") || isNotValid)
                    {
                        //cant use numerals and spaces
                        DrawText(text[4]);
                        Console.WriteLine();
                        //press any button
                        DrawText(text[2]);
                        Console.ReadKey();
                        Console.Clear();
                        DrawTitle();
                    }
                    else
                    {
                        DrawText(text[1] + word.Length);
                        Console.WriteLine();
                        DrawText(text[2]);
                        Console.ReadKey();
                        Console.Clear();
                        DrawTitle();
                        validWord = true;
                    }
                }
            }
            InitializeMsg();
            distinctWord = String.Join("", word.Distinct());
            PlayGame();
        }

        private bool DecideMode()
        {
            //rozhodne o pouzitem modu
            picked = false;
            DrawText(text[0]);
            //samotny vyber
            while (!picked)
            {
                pressedKey = Console.ReadKey();
                if (pressedKey.Key == ConsoleKey.Y)
                {
                    picked = true;
                    return true;
                }
                else if (pressedKey.Key == ConsoleKey.N)
                {
                    picked = true;
                    return false;
                }
                else
                {
                    Console.Clear();
                    DrawTitle();
                    Console.Write(text[0]);
                }
            }
            return false;
        }
        public bool DecideNewGame()
        {
            picked = false;
            while (!picked)
            {
                pressedKey = Console.ReadKey();
                if (pressedKey.Key == ConsoleKey.Y)
                {
                    picked = true;
                    return true;
                }
                else if (pressedKey.Key == ConsoleKey.N)
                {
                    picked = true;
                    return false;
                }
                else
                {
                    if (win)
                    {
                        Console.SetCursorPosition(0, 21);
                        ClearCurrentConsoleLine();
                        Console.SetCursorPosition(0, 22);
                        ClearCurrentConsoleLine();
                        Console.SetCursorPosition(0, 23);
                        ClearCurrentConsoleLine();
                        Console.Write(CenterText(text[9]));
                    }
                    else
                    {
                        Console.SetCursorPosition(0, 21);
                        ClearCurrentConsoleLine();
                        Console.SetCursorPosition(0, 22);
                        ClearCurrentConsoleLine();
                        Console.SetCursorPosition(0, 23);
                        ClearCurrentConsoleLine();
                        Console.Write(CenterText(text[10]));
                    }
                }
            }
           return false;
        }

        private void DrawHangman()
        {
            switch (lives) 
            {
                case 7:
                    Console.SetCursorPosition((Console.WindowWidth/2)-5, 14);
                    Console.Write(("__________"));
                    break;
                case 6:
                                        Console.SetCursorPosition((Console.WindowWidth / 2) - 2, 14);
                    Console.Write("/----\\");
                    break;
                case 5:
                    for (int i = 8; i < 16; i++)
                    {
                        Console.SetCursorPosition((Console.WindowWidth / 2) - 6, i-1);
                        Console.Write("|");                        
                    }
                    break;
                case 4:
                    Console.SetCursorPosition((Console.WindowWidth / 2) - 6, 6);
                    Console.Write(("_______"));
                    break;
                case 3:
                    Console.SetCursorPosition((Console.WindowWidth / 2) - 5, 7);
                    Console.Write("/");
                    break;
                case 2:
                    Console.SetCursorPosition(Console.WindowWidth / 2, 7);
                    Console.Write("|");
                    Console.SetCursorPosition(Console.WindowWidth / 2, 8);
                    Console.Write("O");
                    break;
                case 1:
                    Console.SetCursorPosition((Console.WindowWidth / 2) - 1, 9);
                    Console.Write("/");
                    Console.SetCursorPosition((Console.WindowWidth / 2) + 1, 9);
                    Console.Write("\\");
                    break;
                case 0:
                    Console.SetCursorPosition(Console.WindowWidth / 2, 9);
                    Console.Write("|");
                    Console.SetCursorPosition((Console.WindowWidth / 2) - 1, 10);
                    Console.Write("/");
                    Console.SetCursorPosition((Console.WindowWidth / 2) + 1, 10);
                    Console.Write("\\");
                    break;
            }
        }
        private void InitializeMsg()
        {
            wordMsgCZ = "Tvé slovo bylo " + word + ".";
            wordMsgEN = "Your word was " + word + ".";
            wordMsgES = "Tu palabra era " + word + ".";
            switch (language)
            {
                case 1:
                    wordMsg = wordMsgEN;
                    break;
                case 2:
                    wordMsg = wordMsgCZ;
                    break;
                case 3:
                    wordMsg = wordMsgES;
                    break;
            }
        }

        public static void DrawText(string _text)
        {
            //"animovane" vykresleni textu
            foreach (char c in _text)
            {
                Console.Write(c);
                Thread.Sleep(10);
            }
        }
        public static string CenterText(string input)
        {
            return input.PadLeft(((Console.WindowWidth - input.Length) / 2) + input.Length);
        }
        public static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
