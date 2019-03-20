using System;
using System.Collections.Generic;
using System.Text;

namespace hangmanGame
{
    internal static class GuessProcessing
    {
        internal static bool IsAlreadyGuessed(string guess)
        {
            var index = Array.IndexOf(Program.Alphabet, guess);

            return Program.GuessedCharsPositions.Contains(index);
        }

        internal static int UpdateGuessedChars(string guess)
        {
            return Array.IndexOf(Program.Alphabet, guess);
        }

        internal static string UpdateGuessedWord(IReadOnlyCollection<int> indexes)
        {
            StringBuilder sb = new StringBuilder(Program.GuessedWord);

            foreach (var index in indexes)
            {
                sb[index * 2] = Program.CurrentWord[index];
            }

            return sb.ToString();
        }

        internal static bool CheckIfWon()
        {
            return !Program.GuessedWord.Contains("_");
        }

        internal static IEnumerable<int> GetAllIndexes(this string word, string value)
        {
            return IndexesIterator();
            IEnumerable<int> IndexesIterator()
            {
                for (var index = 0;; index += value.Length)
                {
                    index = word.IndexOf(value, index, StringComparison.Ordinal);
                    if (index == -1)
                    {
                        break;
                    }

                    yield return index;
                }
            }
        }
    }
}
