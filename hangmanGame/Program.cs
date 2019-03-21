using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace hangmanGame
{
    internal static class Program
    {
        internal static string CurrentWord { get; set; }
        internal static string GuessedWord { get; set; }
        internal static readonly List<int> GuessedCharsPositions = new List<int>();

        internal static readonly string[] Alphabet =
        {
            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u",
            "v", "w", "x", "y", "z"
        };

        private static int Attempts { get; set; }

        private static readonly string[] Hangman = //lol this actually works
        {
            "/",
            "/ \\",
            " |\n/ \\",
            " /|\n / \\",
            " /|\\\n / \\",
            "  O\n /|\\\n / \\",
            "  |\n  |\n  O\n /|\\\n / \\",
            "  /--\n  |\n  |\n  O\n /|\\\n / \\",
            "  /-------\\\n  |\n  |\n  O\n /|\\\n / \\",
            "  /-------\\_\n  |\t  |#|\n  |\t  |#|\n  O\n /|\\\n / \\",
            "  /-------\\_\n  |\t  |#|\n  |\t  |#|\n  O\t  |#|\n /|\\\t  |#|\n / \\",
            "  /-------\\_\n  |\t  |#|\n  |\t  |#|\n  O\t  |#|\n /|\\\t  |#|\n / \\\t  /|\\"
        };

        internal static void Main()
        {
            Console.WriteLine("Hangman!\n");

            string[] menuItems =
            {
                "1. Start a new game",
                "2. Exit"
            };

            foreach (var menuItem in menuItems)
            {
                Console.WriteLine(menuItem);
            }

            Console.Write("What will it be chief: ");
            var menuResponse = Console.ReadLine();

            while (menuResponse.IsNotInt() || Convert.ToInt32(menuResponse).IsNotInRange(2, 1))
            {
                Console.WriteLine("\nInvalid input chief");
                Console.Write("What will it be chief: ");
                menuResponse = Console.ReadLine();
            }

            switch (Convert.ToInt32(menuResponse))
            {
                case 1:
                    Console.Clear();
                    StartGame();
                    break;
                case 2:
                    Environment.Exit(1);
                    break;
            }
        }

        private static void StartGame()
        {
            Console.WriteLine("It's time to set the word");
            CurrentWord = TakeCurrentWordInput();

            GameInputProcessing();
        }

        private static void GameInputProcessing()
        {
            while (true)
            {
                DisplayMethod();

                Console.Write("\nEnter guess [# to complete]: ");
                var guess = Console.ReadLine()?.ToLower().Trim();

                while (guess != "#" && !Alphabet.Contains(guess) || GuessProcessing.IsAlreadyGuessed(guess))
                {
                    Console.WriteLine("\nThat ain't a correct guess chief");
                    Console.Write("Enter guess: ");
                    guess = Console.ReadLine()?.ToLower().Trim();
                }

                if (guess == "#")
                {
                    Console.Write("\nEnter guess to complete: ");
                    var completeGuess = Console.ReadLine()?.ToLower().Trim();

                    while (string.IsNullOrEmpty(completeGuess) || completeGuess.ContainsInt())
                    {
                        Console.WriteLine("\nThat ain't a correct guess chief");
                        Console.Write("Enter guess: ");
                        completeGuess = Console.ReadLine()?.ToLower().Trim();
                    }

                    if (completeGuess == CurrentWord)
                    {
                        WinMethod();
                    }

                    IncorrectGuess(completeGuess);
                    continue;
                }

                GuessedCharsPositions.Add(GuessProcessing.UpdateGuessedChars(guess));

                IReadOnlyCollection<int> indexes = CurrentWord.GetAllIndexes(guess).ToList().AsReadOnly();

                if (indexes.Count == 0) { IncorrectGuess(guess); continue;}

                GuessedWord = GuessProcessing.UpdateGuessedWord(indexes);

                if (GuessProcessing.CheckIfWon())
                {
                    WinMethod();
                }

                Console.Clear();
            }
        }

        private static void IncorrectGuess(string guess)
        {
            if (Attempts == Hangman.Length - 1) { LostMethod(); }
            Console.WriteLine($"\n{guess} is incorrect!\n");

            Console.WriteLine(Hangman[Attempts]);

            Console.WriteLine("\nPress any key to continue...");
            Attempts++;
            Console.ReadKey();
            Console.Clear();
        }

        private static string TakeCurrentWordInput()
        {
            Console.Write("Enter word: ");
            var currentWordInput = Console.ReadLine()?.ToLower().Trim();

            while (string.IsNullOrWhiteSpace(currentWordInput) || currentWordInput.IsInt() || currentWordInput.ContainsInt() || Regex.IsMatch(currentWordInput, @"[^a-z\s]+"))
            {
                Console.WriteLine("\nThat ain't a valid word chief");
                Console.Write("Enter word: ");
                currentWordInput = Console.ReadLine()?.ToLower().Trim();
            }

            Console.Clear();

            IReadOnlyCollection<int> spaceIndexes = currentWordInput.GetAllIndexes(" ").ToList().AsReadOnly();

            for (var i = 0; i < currentWordInput.Length; i++)
            {
                if (spaceIndexes.Contains(i))
                {
                    GuessedWord += "  ";
                }
                else
                {
                    GuessedWord += "_ ";
                }
            }

            return currentWordInput;
        }

        private static void DisplayMethod()
        {
            var wordLength = CurrentWord.Length - CurrentWord.Count(x => x == ' ');

            Console.WriteLine($"\n{GuessedWord} {wordLength} chars long | {Hangman.Length - Attempts} attempts left\n\n");

            for (var i = 0; i < Alphabet.Length; i++)
            {
                if (GuessedCharsPositions.Contains(i))
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write($"{Alphabet[i]}, ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write($"{Alphabet[i]}, ");
                }
            }
        }

        private static void WinMethod()
        {
            Console.Clear();
            Console.WriteLine($"{CurrentWord} is correct!");
            Console.WriteLine("u're winnr");

            Cleanup();
            Console.ReadKey();
            Console.Clear();
            Main();
        }

        private static void LostMethod()
        {
            Console.Clear();
            Console.WriteLine("u lost\n");
            Console.WriteLine(Hangman[Hangman.Length - 1]);

            Cleanup();
            Console.ReadKey();
            Console.Clear();
            Main();
        }

        private static void Cleanup()
        {
            GuessedCharsPositions.Clear();
            GuessedWord = "";
            Attempts = 0;
        }
    }
}
