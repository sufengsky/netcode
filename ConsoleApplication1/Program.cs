using System;
using System.Collections;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("aa", "abc");

            foreach (var key in hashtable.Keys)
            {
                Console.WriteLine(hashtable[key]);
                Console.WriteLine("ts");
            }

        }
    }
}
