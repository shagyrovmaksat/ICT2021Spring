using System;

namespace Problem1108
{
    class Program
    {
        static string DefangIPaddr(string address) {
            string answer="";
            for(int i=0; i<address.Length; i++){
                if (address[i] == '.') answer+="[.]";
                else answer+=address[i];
            }
            return answer;
        }

        static void Main(string[] args)
        {
            string a = "1.1.1.1";
            Console.WriteLine(DefangIPaddr(a));
        }
    }
}
