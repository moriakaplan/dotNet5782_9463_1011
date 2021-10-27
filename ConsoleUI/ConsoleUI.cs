using IDAL.DO;
using DalObject;
using System;
//לבדוק האם צריך לשלוח לפונקציות ההוספה שדות או אובייקטים

namespace ConsoleUI
{
    enum Options { Exit, Adding, Updating, Display, ListsDisplay };
    enum AddingOptions { Station = 1, Drone, Customer, Parcel };
    enum UpdatingOptions { Assign = 1, Pick, Deliver, SendToCharge, ReleaseFromCharge };
    enum DisplayOptions { Station = 1, Drone, Customer, Parcel };
    enum DisplayListOptions { Stations = 1, Drones, Customers, Parcels, UnassignedParcels, StationsWithAvailableCargingSlots };
    
    class ConsoleUI
    {
        static void Main(string[] args)
        {
            string input;
            Options options;
            AddingOptions addChoise;
            UpdatingOptions updateChoise;
            DisplayOptions displayChoise;
            DisplayListOptions displayListChoise;

            int id, senderId, targetId, droneId, chargeSlots;
            double longitude, lattitude, battery;
            string name, phone, model;
            WeightCategories weight;
            DroneStatuses status;
            Priorities priority;
            DalObject.DataSource.Initialize();
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
                            case AddingOptions.Station:
                                Console.WriteLine(@"Please enter the following details:
Station ID-");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine("Station name-");
                                name = Console.ReadLine();
                                Console.WriteLine("Station longitude-");
                                input = Console.ReadLine();
                                double.TryParse(input, out longitude);
                                Console.WriteLine("Station lattitude-");
                                input = Console.ReadLine();
                                double.TryParse(input, out lattitude);
                                Console.WriteLine("Available charge slots-");
                                input = Console.ReadLine();
                                int.TryParse(input, out chargeSlots);
                                Station station = new Station {
                                    Id = id,
                                    Name = name,
                                    Longitude = longitude,
                                    Lattitude = lattitude,
                                    ChargeSlots = chargeSlots };
                                DalObject.DalObject.AddStationToTheList(station);
                                break;
                            case AddingOptions.Drone:
                                Console.WriteLine(@"Please enter the following details:
Drone ID-");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine("Dron model-");
                                model = Console.ReadLine();
                                Console.WriteLine("Dron max weight (Easy/Medium/Heavy)-");
                                input = Console.ReadLine();
                                WeightCategories.TryParse(input, out weight);
                                Console.WriteLine("Drone status (Vacant/Maintenance/Sending)-");
                                input = Console.ReadLine();
                                DroneStatuses.TryParse(input, out status);
                                Console.WriteLine("Drone battery-");
                                input = Console.ReadLine();
                                double.TryParse(input, out battery);
                                Drone drone = new Drone {
                                    Id = id,
                                    Model = model,
                                    MaxWeight = weight,
                                    Status = status,
                                    Battery = battery };
                                DalObject.DalObject.AddDroneToTheList(drone);
                                break;
                            case AddingOptions.Customer:
                                Console.WriteLine(@"Please enter the following details:
Customer ID-");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine("Customer name-");
                                name = Console.ReadLine();
                                Console.WriteLine("Customer phone-");
                                phone = Console.ReadLine();
                                Console.WriteLine("Station longitude-");
                                input = Console.ReadLine();
                                double.TryParse(input, out longitude);
                                Console.WriteLine("Station lattitude-");
                                input = Console.ReadLine();
                                double.TryParse(input, out lattitude);
                                Customer customer = new Customer {
                                    Id = id,
                                    Name = name,
                                    Phone = phone,
                                    Longitude = longitude,
                                    Lattitude = lattitude
                                };
                                DalObject.DalObject.AddCustomerToTheList(customer);
                                break;
                            case AddingOptions.Parcel:
                                Console.WriteLine(@"Please enter the following details:");
                                Console.WriteLine("Sender ID");
                                input = Console.ReadLine();
                                int.TryParse(input, out senderId);
                                Console.WriteLine("Target ID-");
                                input = Console.ReadLine();
                                int.TryParse(input, out targetId);
                                Console.WriteLine("Drone ID-");
                                input = Console.ReadLine();
                                int.TryParse(input, out droneId);
                                Console.WriteLine("Parcel weight (Easy/Medium/Heavy)-");
                                input = Console.ReadLine();
                                WeightCategories.TryParse(input, out weight);
                                Console.WriteLine("Parcel priority (Regular/Fast/Emergency)-");
                                input = Console.ReadLine();
                                Priorities.TryParse(input, out priority);
                                Parcel pa = new Parcel
                                {
                                    Id = 0,
                                    Senderld = senderId,
                                    TargetId = targetId,
                                    Droneld = droneId,
                                    Weight = weight,
                                    Priority = priority,
                                    Requested = DateTime.Now,
                                    Scheduled = DateTime.MinValue,
                                    PickedUp = DateTime.MinValue,
                                    Delivered = DateTime.MinValue
                                };
                                DalObject.DalObject.AddParcelToTheList(pa);
                                break;
                            default:
                                Console.WriteLine("ERROR"); 
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
                                Console.WriteLine("Enter the parcel ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine("Enter the drone ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out senderId);
                                DalObject.DalObject.AssignParcelToDrone(id, senderId);
                                break;
                            case UpdatingOptions.Pick:
                                Console.WriteLine("Enter the parcel ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                DalObject.DalObject.PickParcelByDrone(id);
                                break;
                            case UpdatingOptions.Deliver:
                                Console.WriteLine("Enter the parcel ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                DalObject.DalObject.DeliverParcelToCustomer(id);
                                break;
                            case UpdatingOptions.SendToCharge:
                                Console.WriteLine("Enter the drone ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out droneId);
                                Console.WriteLine("Enter the station ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                DalObject.DalObject.SendDroneToCharge(droneId, id);
                                break;
                            case UpdatingOptions.ReleaseFromCharge:
                                Console.WriteLine("Enter the drone ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out droneId);
                                DalObject.DalObject.ReleaseDroneFromeCharge(droneId);
                                break;
                            default:
                                Console.WriteLine("ERROR");
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
                        DisplayOptions.TryParse(input, out displayChoise);
                        switch (displayChoise)
                        {
                            case DisplayOptions.Station:
                                Console.WriteLine("Enter the station ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine(DalObject.DalObject.DisplayStation(id));
                                break;
                            case DisplayOptions.Drone:
                                Console.WriteLine("Enter the drone ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine(DalObject.DalObject.DisplayDrone(id));
                                break;
                            case DisplayOptions.Customer:
                                Console.WriteLine("Enter the customer ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine(DalObject.DalObject.DisplayCustomer(id));
                                break;
                            case DisplayOptions.Parcel:
                                Console.WriteLine("Enter the parcel ID:");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine(DalObject.DalObject.DisplayParcel(id));
                                break;
                            default:
                                Console.WriteLine("ERROR");
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
                        DisplayListOptions.TryParse(input, out displayListChoise);
                        switch (displayListChoise)
                        {
                            case DisplayListOptions.Stations:
                                Console.WriteLine("The stations are:");
                                foreach(Station item in DalObject.DalObject.DisplayListOfStations())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.Drones:
                                Console.WriteLine("The drones are:");
                                foreach (Drone item in DalObject.DalObject.DisplayListOfDrones())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.Customers:
                                Console.WriteLine("The customers are:");
                                foreach (Customer item in DalObject.DalObject.DisplayListOfCustomers())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.Parcels:
                                Console.WriteLine("The parcels are:");
                                foreach (Parcel item in DalObject.DalObject.DisplayListOfParcels())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.UnassignedParcels:
                                Console.WriteLine("The parcels that not assigned to drone are:"); //לכתוב תיאור של מה מודפס
                                foreach (Parcel item in DalObject.DalObject.DisplayListOfUnassignedParcels())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.StationsWithAvailableCargingSlots:
                                Console.WriteLine("The stations with available charge slots are:"); //לכתוב תיאור של מה מודפס
                                foreach (Station item in DalObject.DalObject.DisplayListOfStationsWithAvailableCargeSlots())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            default:
                                Console.WriteLine("ERROR");
                                break;
                        };
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
