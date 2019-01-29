using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo0007
{
    // 用于多线程编程的Parallel类的使用示例
    class Program
    {
        static void Main (string[] args)
        {
            ParallelLoopResult result = Parallel.For (
                0, 10, i =>
                {
                    Console.WriteLine ("{0},task:{1},thread:{2}", i,
                        Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep (10);
                }
            );

            System.Console.WriteLine ("Is completed:{0}", result.IsCompleted);
        }
    }
}