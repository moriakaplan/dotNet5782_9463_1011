using System;

namespace ConsoleUI_BL
{
    public class ConsoleUI_BL
    {
        static void Main(string[] args)
        {
            IBL.Ibl blObject = new BL.BL();
            Console.WriteLine("Hello World!");
        }
    }
}
