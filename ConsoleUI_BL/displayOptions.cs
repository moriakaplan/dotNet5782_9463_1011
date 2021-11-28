using System;
using IBL.BO;
using IBL;

namespace ConsoleUI_BL
{
    public partial class ConsoleUI_BL
    {
        public void display(Ibl blObject)
        {
            string input;
            DisplayOptions displayChoise;
            int id;
            
            Console.WriteLine(@"Wich option you want?
1-Display base station
2-Display drone
3-Display customer
4-Display parcel
");
            input = Console.ReadLine();
            DisplayOptions.TryParse(input, out displayChoise);
            switch (displayChoise)
            {
                case DisplayOptions.Station://display a station
                    Console.WriteLine("Enter the station ID (4 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine(blObject.DisplayStation(id));
                    break;
                case DisplayOptions.Drone://display a drone
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine(blObject.DisplayDrone(id));
                    break;
                case DisplayOptions.Customer://display a customer
                    Console.WriteLine("Enter the customer ID (9 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine(blObject.DisplayCustomer(id));
                    break;
                case DisplayOptions.Parcel://display a paecel
                    Console.WriteLine("Enter the parcel ID (8 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine(blObject.DisplayParcel(id));
                    break;
                default:
                    Console.WriteLine("ERROR");
                    break;
            };
        }
    }
}
