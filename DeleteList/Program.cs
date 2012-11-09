using System;
using System.Threading;

namespace DeleteList
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ExpireDictionary<int, object> testdictionary = new ExpireDictionary<int, object>(10, new TimeSpan(0, 0, 0, 5));
            Console.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
            testdictionary.Add(0, "Ana ");
            Console.WriteLine(testdictionary.Count);
            Thread.Sleep(1);
            testdictionary.Add(1, "are ");
            Console.WriteLine(testdictionary.Count);
            Thread.Sleep(1);
            testdictionary.Add(2, "mere ");
            Console.WriteLine(testdictionary.Count);
            Thread.Sleep(1);
            testdictionary.Add(3, "smechere ");
            Console.WriteLine(testdictionary.Count);
            Thread.Sleep(1);
            testdictionary.Add(4, 34);
            Console.WriteLine(testdictionary.Count);
            Console.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff"));
            Thread.Sleep(1000);
            string myTime = DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss:ffff");
            Console.WriteLine(myTime);
            Console.Read();
        }
    }
}