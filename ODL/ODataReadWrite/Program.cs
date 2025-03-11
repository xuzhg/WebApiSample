using ODataReadWrite.Read;
using ODataReadWrite.Write;
using System;

namespace ODataReadWrite
{
    class Program
    {
        static void Main(string[] args)
        {
            ODataReadTest.TestReadResource();

            Console.WriteLine("Hello World!");

            ODataWriteTest.TestWriting();
        }
    }
}
