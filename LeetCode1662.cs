using System;

namespace Problem1662
{
    class Program
    {
        static bool ArrayStringsAreEqual(string[] word1, string[] word2)
        {
            string first = "", second = "";
            for (int i = 0; i < word1.Length; i++)
            {
                first += word1[i];
            }
            for (int i = 0; i < word2.Length; i++)
            {
                second += word2[i];
            }
            return first == second;
        }

        static void Main(string[] args)
        {
            string[] arr1 = new string[] { "a", "bc", "de" };
            string[] arr2 = new string[] { "ab", "cd", "e" };

            if (ArrayStringsAreEqual(arr1, arr2))
            {
                Console.WriteLine("true");
            }
            else
            {
                Console.WriteLine("false");
            }
        }
    }
}
