               using System;

namespace ConsoleUI
{
    class ConsoleUI
    {
        enum Options{ Exit, Adding, Updating, Display, ListsDisplay };
        enum AddingOptions { Station=1, Drone, Customer, Parcel};
        enum UpdatingOptions { Assign=1, Pick, Deliver, SendToCharge, ReleaseFromCharge};
        enum DisplayOptions { Station=1, Drone, Customer, Parcel};
        enum DisplayListOptions { Stations = 1, Drones, Customers, Parcels, UnassignedParcels, StationsWithAvailableCargingSlots};

        static void Main()
        {
            string input;
            Options options;
            AddingOptions addChoise;
            UpdatingOptions updateChoise;
            do
            {
                Console.WriteLine(@"Hello! what do you want to do?
1-Adding options
2-Updating options
3-Display options
4-Lists display options
0-Exit
");
                input = Console.ReadLine();
                Options.TryParse(input, out options);
                switch (options)
                {
                    case Options.Adding:
                        Console.WriteLine(@"Wich option you want?
1-Add new base station
2-Add new drone
3-Add new customer
4-Add new parcel");
                        input = Console.ReadLine();
                        AddingOptions.TryParse(input, out addChoise);
                        switch (addChoise)
                        {
                            case AddingOptions.Base:

                                break;
                            case AddingOptions.Drone:

                                break;
                            case AddingOptions.Customer:

                                break;
                            case AddingOptions.Parcel:

                                break;
                        };
                        break;
                    case Options.Updating:
                        Console.WriteLine(@"Wich option you want?
1-Assign parcel to drone
2-Pick parcel by drone
3-Deliver parcel to customer
4-Send drone to charge
5-Release drone frome charge");
                        input = Console.ReadLine();
                        UpdatingOptions.TryParse(input, out updateChoise);
                        switch (updateChoise)
                        {
                            case UpdatingOptions.Assign:

                                break;
                            case UpdatingOptions.Pick:

                                break;
                            case UpdatingOptions.Deliver:

                                break;
                            case UpdatingOptions.SendToCharge:

                                break;
                            case UpdatingOptions.ReleaseFromCharge:

                                break;
                            default:
                                break;
                        };
                        break;
                    case Options.Display:
                        Console.WriteLine(@"Wich option you want?
1-Display base station
2-Display drone
3-Display customer
4-Display parcel");
                        input = Console.ReadLine();
                        AddingOptions.TryParse(input, out choise);
                        switch (choise)
                        {
                            case Options:

                                break;
                            case Options:

                                break;
                            case Options:

                                break;
                            case Options:

                                break;
                            default:
                                break;
                        };
                        break;
                    case Options.ListsDisplay:
                        Console.WriteLine(@"Wich option you want?
1-Display base stations list
2-Display drones list
3-Display customers list
4-Display parcels list
5-Display list of unassigned parcels
6-Display list of stations with available carging slots");
                        input = Console.ReadLine();
                        AddingOptions.TryParse(input, out choise);
                        switch (choise)
                        {
                            case Options:

                                break;
                            case Options:

                                break;
                            case Options:

                                break;
                            case Options:

                                break;
                            default:
                                break;
                        };
                        break;
                    case Options.Exit:
                        break;
                    default:

                        break;
                };
            }
            while (options != 0);
//Adding options:
//1-Add new base station
//2-Add new drone
//3-Add new customer
//4-Add new parcel

//Updating options:
//5-Assign parcel to drone
//6-Pick parcel by drone
//7-Deliver parcel to customer
//8-Send drone to charge
//9-Release drone frome charge

//Display options:
//10-Display base station
//11-Display drone
//12-Display customer
//13-Display parcel

//Lists display options:
//14-Display base stations list
//15-Display drones list
//16-Display customers list
//17-Display parcels list
//18-Display list of unassigned parcels
//19-Display list of stations with available carging slots

//0-exit
        
        }
    }
}
