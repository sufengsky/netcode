using System;

//类的继承中虚方法重写示例
class Program
{
    public static void Main (string[] args)
    {
        B1 b1 = new B1 ();
        B2 b2 = new B2 ();
        b1.Print (); //B1
        b2.Print (); //B2

        A ab1 = new B1 ();
        A ab2 = new B2 ();
        ab1.Print (); //B1
        ab2.Print (); //A

        A a = new A ();
        a.Print (); //A
    }
}

public class A
{
    public virtual void Print ()
    {
        Console.WriteLine ("A");
    }
}

public class B1 : A
{
    public override void Print ()
    {
        Console.WriteLine ("B1");
    }
}
public class B2 : A
{
    public new void Print ()
    {
        Console.WriteLine ("B2");
    }
}