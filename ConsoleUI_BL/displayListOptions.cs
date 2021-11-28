using System;
using IBL.BO;
using IBL;

namespace ConsoleUI_BL
{
    internal partial class ConsoleUI_BL
    {
        public void displayList(Ibl blObject)
        {
            string input;
            DisplayListOptions displayListChoise;

            Console.WriteLine(@"Wich option you want?
1-Display base stations list
2-Display drones list
3-Display customers list
4-Display parcels list
5-Display list of unassigned parcels
6-Display list of stations with available carging slots
");
            input = Console.ReadLine();
            DisplayListOptions.TryParse(input, out displayListChoise);
            switch (displayListChoise)
            {
                case DisplayListOptions.Stations://display a list of stations
                    Console.WriteLine("The stations are:");
                    foreach (StationToList item in blObject.DisplayListOfStations())
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case DisplayListOptions.Drones://display a list of drones
                    Console.WriteLine("The drones are:");
                    foreach (DroneToList item in blObject.DisplayListOfDrones())
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case DisplayListOptions.Customers://display a list of customers
                    Console.WriteLine("The customers are:");
                    foreach (CustomerToList item in blObject.DisplayListOfCustomers())
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case DisplayListOptions.Parcels://display a list of parcels
                    Console.WriteLine("The parcels are:");
                    foreach (ParcelToList item in blObject.DisplayListOfParcels())
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case DisplayListOptions.UnassignedParcels://display the list of the parcels that not assign to a drone.
                    Console.WriteLine("The parcels that not assigned to drone are:"); //לכתוב תיאור של מה מודפס
                    foreach (ParcelToList item in blObject.DisplayListOfUnassignedParcels())
                    {
                        Console.WriteLine(item);
                    }
                    break;
                case DisplayListOptions.StationsWithAvailableCargingSlots://display the list of the stations that have available charge slots
                    Console.WriteLine("The stations with available charge slots are:"); //לכתוב תיאור של מה מודפס
                    foreach (StationToList item in blObject.DisplayListOfStationsWithAvailableCargeSlots())
                    {
                        Console.WriteLine(item);
                    }
                    break;
                default:
                    Console.WriteLine("ERROR");
                    break;
            };
        }
    }
}
