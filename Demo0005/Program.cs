using System;
using System.Collections.Generic;

namespace Demo0005
{
    
    class Program
    {
        static void Main (string[] args)
        {
            System.Console.WriteLine(Test.GetCount());
        }
    }

    public class Test
    {
        private static Dictionary<string, string> dic = new Dictionary<string, string> ();
        static Test ()
        {
             dic.Add("a","a");
        }

        public static int GetCount ()
        {
            return dic.Count;
        }
    }
}