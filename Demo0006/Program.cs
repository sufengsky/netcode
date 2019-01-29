using System;
using System.Threading.Tasks;

namespace Demo0006
{
    // https://blog.csdn.net/tianmuxia/article/details/17675681
    // async和await关键字的使用简单示例
    class Program
    {
        static void Main (string[] args)
        {
            var c = new MyClass ();
        }
    }

    public class MyClass
    {
        public MyClass ()
        {
            DisplayValue (); //这里不会阻塞
            System.Diagnostics.Debug.WriteLine ("MyClass() End.");
        }

        public async void DisplayValue ()
        {
            double result = await GetValueAsync (1234.5, 1.01); //此处会开新线程处理GetValueAsync任务，然后方法马上返回
            //这之后的所有代码都会被封装成委托，在GetValueAsync任务完成时调用
            System.Diagnostics.Debug.WriteLine ("Value is : " + result);
        }

        public Task<double> GetValueAsync (double num1, double num2)
        {
            return Task.Run (() =>
            {
                for (int i = 0; i < 1000000; i++)
                {
                    num1 = num1 / num2;
                }
                return num1;
            });
        }

    }
}