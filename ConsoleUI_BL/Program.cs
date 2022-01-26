using System;
using BLApi;

namespace ConsoleUI_BL
{
    public partial class ConsoleUI_BL
    {
        static void Main(string[] args)
        {
            IBL blObject = BLFactory.GetBl(); //need to be the interface Ibl
            string input;
            Options options;

            Console.Write("Hello! ");
            do//Asks and does what the user wants to do
            {
                Console.WriteLine(@"what do you want to do?
1-Adding options
2-Updating options
3-Display options
4-Lists display options
5-Distance between places
0-Exit
");
                input = Console.ReadLine();
                Options.TryParse(input, out options);
                switch (options)
                {
                    case Options.Adding://if the user wants to add something
                        add(blObject);
                        break;
                    case Options.Updating://if the user wants to update something
                        update(blObject);
                        break;
                    case Options.Display:// if the user wants to display something
                        display(blObject);
                        break;
                    case Options.ListsDisplay://if the user wants to display a list
                        displayList(blObject);
                        break;
                    case Options.Exit:
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                };
            }
            while (options != 0);
        }
    }
}
