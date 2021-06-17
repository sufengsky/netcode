using System;

namespace Demo0001 {
    public class Student {
        public string Name { get; set; }
        public int Age { get; set; }

        public override string ToString () {
            return Name + Age.ToString ();
        }
    }
}