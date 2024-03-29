﻿using System;
using BO;
using BLApi;

namespace ConsoleUI_BL
{
    public partial class ConsoleUI_BL
    {
        public static void display(IBL blObject)
        {
            try
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
                        try { Console.WriteLine(blObject.GetStation(id)); }
                        catch (NotExistIDException) { Console.WriteLine("this station not exist, try again"); }
                        break;
                    case DisplayOptions.Drone://display a drone
                        Console.WriteLine("Enter the drone ID (6 digits):");
                        input = Console.ReadLine();
                        int.TryParse(input, out id);
                        try { Console.WriteLine(blObject.GetDrone(id)); }
                        catch (NotExistIDException) { Console.WriteLine("this drone not exist, try again"); }
                        break;
                    case DisplayOptions.Customer://display a customer
                        Console.WriteLine("Enter the customer ID (9 digits):");
                        input = Console.ReadLine();
                        int.TryParse(input, out id);
                        try { Console.WriteLine(blObject.GetCustomer(id)); }
                        catch (NotExistIDException) { Console.WriteLine("this customer not exist, try again"); }
                        break;
                    case DisplayOptions.Parcel://display a paecel
                        Console.WriteLine("Enter the parcel ID (8 digits):");
                        input = Console.ReadLine();
                        int.TryParse(input, out id);
                        try { Console.WriteLine(blObject.GetParcel(id)); }
                        catch (NotExistIDException) { Console.WriteLine("this parcel not exist, try again"); }
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message + ", please try again");
            }
        }
    }
}
