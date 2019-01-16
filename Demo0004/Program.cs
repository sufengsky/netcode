using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo0004
{
    class Program
    {
        static void Main (string[] args)
        {
            var list = new List<string>
            {
                "a",
                "b",
                "c"
            };

            // var result = list.Where (x => x == "a").ToList ();
            // foreach (var item in result)
            // {
            //     System.Console.WriteLine (item);
            // }
            var result = list.Select (x => x == "a").ToList();
            foreach (var item in result)
            {
                System.Console.WriteLine (item);
            }
        }
    }
}