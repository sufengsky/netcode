using System;

namespace Demo0003
{
    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person();
            Console.WriteLine("Age=" + person.Age + ",Name:" + person.Name);
            var student=new Student();

            System.Console.WriteLine(student.Name);
        }
    }
    //自动属性的默认值设置
    public class Person
    {
        public int Age { get; set; } = 20;
        public string Name { get; set; } = "default";
    }
}
