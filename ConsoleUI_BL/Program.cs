using System;
using IBL.BO;
using IBL;

namespace ConsoleUI_BL
{
    public partial class ConsoleUI_BL
    {
        static void Main(string[] args)
        {
            IBL.Ibl blObject = new BL.BL();
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
//                    case Options.Distance://if the user want to know the distance between two places
//                        Console.WriteLine(@"Enter Coordinates of place in the world:
//longitude- ");
//                        input = Console.ReadLine();
//                        double.TryParse(input, out longitude);
//                        Console.WriteLine("lattitude- ");
//                        input = Console.ReadLine();
//                        double.TryParse(input, out lattitude);
//                        Console.WriteLine("Do you want to know the distance to station or customer?");
//                        input = Console.ReadLine();
//                        double distance = 0;
//                        if (input == "station" || input == "Station")//if the user want to know the distance between station and another place
//                        {
//                            Console.WriteLine("Enter the station ID: (4 digits) ");
//                            input = Console.ReadLine();
//                            int.TryParse(input, out id);
//                            distance = dalObject.DistanceForStation(longitude, lattitude, id);
//                        }
//                        else if (input == "customers" || input == "Customers")//if the user want to know the distance between customer and another place
//                        {
//                            Console.WriteLine("Enter the customer ID: (9 digits) ");
//                            input = Console.ReadLine();
//                            int.TryParse(input, out id);
//                            distance = dalObject.DistanceForCustomer(longitude, lattitude, id);
//                        }
//                        Console.WriteLine("the distance is: " + distance);
//                        break;
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
