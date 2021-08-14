using System;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static object Kontrol = new object(); 
        static void X()
        {
            lock (Kontrol)
            {
                for (int i = 0; i < 50; i++)
                {
                    Console.WriteLine("X:" + i);
                }
            }
        }
        static void Y()
        {
            lock (Kontrol)
            {
                for (int i = 0; i < 50; i++)
                {
                    Console.WriteLine("Y:" + i);
                }
            }
        }
        static void Main(string[] args)
        {
            Thread thx = new Thread(new ThreadStart(X));
            Thread thy = new Thread(new ThreadStart(Y));
            thx.Start();
            thy.Start();
        }
    }
}
