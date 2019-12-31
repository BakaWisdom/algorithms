using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms1
{
    class Anagram
    {
        /// <summary>
        /// converts input stream to a string list
        /// </summary>
        /// <returns></returns>
        public static List<string> AcquireInputString()
        {
            string thisLine;
            List<string> output = new List<string>();

            //code modelled after given example code in kattis to ensure reading/writing is done properly.
            while ((thisLine = Console.ReadLine()) != null && thisLine != "")
            {
                output.Add(thisLine);
            }
            return output;

        }

        /// <summary>
        /// evaluates string input to determine number of unique words
        /// heart of the program
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int EvaluateInput(List<string> wordList)
        {
            HashSet<string> uniqueWords = new HashSet<string>();
            HashSet<string> repeatedWords = new HashSet<string>();
            int length, maxWords, wordCount;
            wordCount = 0;

            {
                string[] configurations = wordList[0].Split(' ', '\n');
                length = int.Parse(configurations[1]);
                maxWords = int.Parse(configurations[0]);
            }

            foreach (string word in wordList.Skip(1))
            {
                wordCount++;
                //checks if it is a valid word length
                if (word.Length != length)
                {
                    continue;
                }

                string code = Encoder(word);
                if (uniqueWords.Contains(code))
                {
                    repeatedWords.Add(code);
                    uniqueWords.Remove(code);
                }
                else
                {
                    if (!repeatedWords.Contains(code))
                    {
                        uniqueWords.Add(code);
                    }
                }

                //doesn't look at words after the maxWords has been reached
                if (wordCount >= maxWords)
                {
                    return uniqueWords.Count;
                }
            }

            return uniqueWords.Count;
        }

        /// <summary>
        /// encodes each word into a a number that uniquely represents the char found in a word of given length.
        /// this is done by
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private static string Encoder(string word)
        {
            char[] letters = word.ToCharArray();

            StringBuilder output = new StringBuilder();
            foreach (char letter in letters.OrderByDescending(letter => letter))
            {
                output.Append(letter);
            }
            return output.ToString();
        }
    }
}

