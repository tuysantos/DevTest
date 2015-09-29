using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Configuration;

namespace DevTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             For Test 1 change the actionType in App.config to 1
             For Test 2 change the actionType in App.config to 2
             */

            var actionType = Convert.ToInt32(ConfigurationManager.AppSettings["ActionType"]);
            var words = File.ReadAllLines(@"fileinput.txt");

            if (actionType == 2)
            {
                ConcatenateWords(words);
            }
            else
            {
                CountWords(words);
            }

            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.Read();
        }

        static string RemoveSpecialChars(string line)
        {
            var arraySpecial = (",*;*'*?*!*:*/*\\*^*.*(*)*`").Split('*');

            if (line.Trim().Length > 0)
            {
                var iTotal = arraySpecial.Length;
                for (var x = 0; x < iTotal; x++)
                {
                    line = line.Replace(arraySpecial[x], "");
                }
            }

            return line;
        }

        static void FindOccurence(string line, IDictionary<string, string> wordsFound)
        {
            var allWordsInLine = line.Split(' ');

            foreach (string t in allWordsInLine)
            {
                var ffound = false;
                if (wordsFound.Any(word => t == word.Key))
                {
                    ffound = true;
                    var count = Convert.ToInt32(wordsFound[t]);
                    wordsFound[t] = (count + 1).ToString();
                }

                if (!ffound)
                {
                    wordsFound.Add(t, "1");
                }

                ffound = false;
            }
        }

        static void CountWords(IList<string> words)
        {
            try
            {
                var myWords = new SortedDictionary<string, string>();

                for (var i = 0; i < words.Count; i++)
                {
                    //remove all special characters
                    words[i] = RemoveSpecialChars(words[i]);
                    if (!string.IsNullOrEmpty(words[i]))
                    {
                        //Find occurenceand build dictionary
                        FindOccurence(words[i], myWords);
                    }
                }

                //print out the result
                Console.WriteLine("Word" + "\t \t \t Count");
                foreach (var myWord in myWords)
                {
                    if (myWord.Key.Length <= 7)
                        Console.WriteLine(myWord.Key + "\t \t \t" + myWord.Value);
                    else
                        Console.WriteLine(myWord.Key + "\t \t" + myWord.Value);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception : " + e.Message);
            }
        }

        static void ConcatenateWords(IList<string> words)
        {
            var myWords = new List<string>();
            var temp = "";

            //remove all special characters
            for (var i = 0; i < words.Count; i++)
            {
                words[i] = RemoveSpecialChars(words[i]);

                if (!String.IsNullOrEmpty(words[i]))
                {
                    if (String.IsNullOrEmpty(temp))
                    {
                        temp = words[i];
                    }
                    else
                    {
                        temp = temp + "," + words[i];
                    }
                }
            }

            var wordsToVerify = temp.Split(',');
            var result = FindWords(wordsToVerify, myWords);

            //print out
            Console.WriteLine(result);
        }

        static string FindWords(IEnumerable<string> line, ICollection<string> wordsFound)
        {
            var result = "";
            foreach (var tempLine in line)
            {
                var lineArray = tempLine.Split(' ');
                for (var i = 0; i < lineArray.Length; i++)
                {
                    foreach (var singleLine in lineArray)
                    {
                        if (lineArray[i] != singleLine)
                        {
                            if (singleLine.ToLower().StartsWith(lineArray[i].ToLower()) && singleLine.Length == 6)
                            {
                                if (wordsFound.All(t => t != singleLine))
                                {
                                    wordsFound.Add(singleLine);
                                }
                            }


                        }
                    }
                }

            }

            foreach (var word in wordsFound)
            {
                if (string.IsNullOrEmpty(result))
                    result = word;
                else
                {
                    result = result + ", " + word;
                }
            }

            return result;
        }
    }
}

