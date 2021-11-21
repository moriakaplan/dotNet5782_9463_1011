using System;
using IBL;
using IBL.BO;

namespace ConsoleUI_BL
{
    enum Options { Exit, Adding, Updating, Display, ListsDisplay, Distance };
    enum AddingOptions { Station = 1, Drone, Customer, Parcel };
    enum UpdatingOptions { DroneName = 1, station, Customer, SendDroneToCharge, ReleaseDroneFromCharge, AssignParcelToDrone, PickParcelByDrone, DeliverParcelByDrone };
    enum DisplayOptions { Station = 1, Drone, Customer, Parcel };
    enum DisplayListOptions { Stations = 1, Drones, Customers, Parcels, UnassignedParcels, StationsWithAvailableCargingSlots };

    public class ConsoleUI_BL
    {
        IBL.IBL BlObject = new BL.BL();
        static void Main(string[] args)
        {
            string input;
            Options options;
            UpdatingOptions updateChoise;
            DisplayOptions displayChoise;
            DisplayListOptions displayListChoise;
            int id, senderId, droneId;
            double longitude, lattitude;

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
                        AddingOptions addChoise;
                        Console.WriteLine(@"Wich option you want?
1-Add new base station
2-Add new drone
3-Add new customer
4-Add new parcel
");
                        input = Console.ReadLine();
                        AddingOptions.TryParse(input, out addChoise);
                        add(addChoise);
                        break;
                    case Options.Updating://if the user wants to update something
                        Console.WriteLine(@"Wich option you want?
1-Assign parcel to drone
2-Pick parcel by drone
3-Deliver parcel to customer
4-Send drone to charge
5-Release drone frome charge
");
                        input = Console.ReadLine();
                        UpdatingOptions.TryParse(input, out updateChoise);
                        update(updateChoise);
                        break;
                    case Options.Display:// if the user wants to display something
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
                                Console.WriteLine(BLObject.DisplayStation(id));
                                break;
                            case DisplayOptions.Drone://display a drone
                                Console.WriteLine("Enter the drone ID (6 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine(dalObject.DisplayDrone(id));
                                break;
                            case DisplayOptions.Customer://display a customer
                                Console.WriteLine("Enter the customer ID (9 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine(dalObject.DisplayCustomer(id));
                                break;
                            case DisplayOptions.Parcel://display a paecel
                                Console.WriteLine("Enter the parcel ID (8 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine(dalObject.DisplayParcel(id));
                                break;
                            default:
                                Console.WriteLine("ERROR");
                                break;
                        };
                        break;
                    case Options.ListsDisplay://if the user wants to display a list
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
                                foreach (Station item in dalObject.DisplayListOfStations())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.Drones://display a list of drones
                                Console.WriteLine("The drones are:");
                                foreach (Drone item in dalObject.DisplayListOfDrones())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.Customers://display a list of customers
                                Console.WriteLine("The customers are:");
                                foreach (Customer item in dalObject.DisplayListOfCustomers())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.Parcels://display a list of parcels
                                Console.WriteLine("The parcels are:");
                                foreach (Parcel item in dalObject.DisplayListOfParcels())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.UnassignedParcels://display the list of the parcels that not assign to a drone.
                                Console.WriteLine("The parcels that not assigned to drone are:"); //לכתוב תיאור של מה מודפס
                                foreach (Parcel item in dalObject.DisplayListOfUnassignedParcels())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.StationsWithAvailableCargingSlots://display the list of the stations that have available charge slots
                                Console.WriteLine("The stations with available charge slots are:"); //לכתוב תיאור של מה מודפס
                                foreach (Station item in dalObject.DisplayListOfStationsWithAvailableCargeSlots())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            default:
                                Console.WriteLine("ERROR");
                                break;
                        };
                        break;
                    case Options.Distance://if the user want to know the distance between two places
                        Console.WriteLine(@"Enter Coordinates of place in the world:
longitude- ");
                        input = Console.ReadLine();
                        double.TryParse(input, out longitude);
                        Console.WriteLine("lattitude- ");
                        input = Console.ReadLine();
                        double.TryParse(input, out lattitude);
                        Console.WriteLine("Do you want to know the distance to station or customer?");
                        input = Console.ReadLine();
                        double distance = 0;
                        if (input == "station" || input == "Station")//if the user want to know the distance between station and another place
                        {
                            Console.WriteLine("Enter the station ID: (4 digits) ");
                            input = Console.ReadLine();
                            int.TryParse(input, out id);
                            distance = dalObject.DistanceForStation(longitude, lattitude, id);
                        }
                        else if (input == "customers" || input == "Customers")//if the user want to know the distance between customer and another place
                        {
                            Console.WriteLine("Enter the customer ID: (9 digits) ");
                            input = Console.ReadLine();
                            int.TryParse(input, out id);
                            distance = dalObject.DistanceForCustomer(longitude, lattitude, id);
                        }
                        Console.WriteLine("the distance is: " + distance);
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


        static void add(AddingOptions choise)
        {
            string input, name, model, phone;
            int id, senderId, targetId, chargeSlots;
            double longitude, lattitude, battery;
            IBL.BO.DroneStatus status;
            IBL.BO.WeightCategories weight;
            IBL.BO.Priorities priority;
            switch (choise)
            {
                case AddingOptions.Station://if the user wants to add a station
                                           //Receives the details of the station from the user
                    Console.WriteLine(@"Please enter the following details:
Station ID (4 digits)-");
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
                    IBL.BO.Location location { Longitude = Longitude, Lattitude = Lattitude };
                    Console.WriteLine("Available charge slots-");
                    input = Console.ReadLine();
                    int.TryParse(input, out chargeSlots);
                    Station station = new IBL.BO.Station//Creates a new station
                    {
                        Id = id,
                        Name = name,
                        Location = location,
                        AvailableChargeSlots = chargeSlots
                    };
                    BLObject.AddStation(station);//Add the station to the list
                    break;
                case AddingOptions.Drone://if the user wants to add a drone
                                         //Receives the details of the drone from the user

                    Console.WriteLine(@"Please enter the following details:
Drone ID (6 digits)-");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Dron model-");
                    model = Console.ReadLine();
                    Console.WriteLine("Dron max weight (Easy/Medium/Heavy)-");
                    input = Console.ReadLine();
                    WeightCategories.TryParse(input, out weight);
                    Console.WriteLine("Drone status (Vacant/Maintenance/Sending)-");
                    input = Console.ReadLine();
                    IBL.BO.DroneStatus.TryParse(input, out status);
                    Console.WriteLine("Drone battery-");
                    input = Console.ReadLine();
                    double.TryParse(input, out battery);
                    Drone drone = new Drone//Creates a new drone
                    {
                        Id = id,
                        Model = model,
                        MaxWeight = weight,
                        Status = status,
                        Battery = battery
                    };
                    BLObject.AddDrone(drone);//Add the drone to the list
                    break;
                case AddingOptions.Customer://if the user wants to add a customer
                                            //Receives the details of the customer from the user
                    Console.WriteLine(@"Please enter the following details:
Customer ID (9 digits)-");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Customer name-");
                    name = Console.ReadLine();
                    Console.WriteLine("Customer phone-");
                    phone = Console.ReadLine();
                    Console.WriteLine("Customer longitude-");
                    input = Console.ReadLine();
                    double.TryParse(input, out longitude);
                    Console.WriteLine("Customer lattitude-");
                    input = Console.ReadLine();
                    double.TryParse(input, out lattitude);
                    Customer customer = new Customer// Creates a new customer
                    {
                        Id = id,
                        Name = name,
                        Phone = phone,
                        Location = location
                    };
                    BLObject.AddCustomer(customer);//Add the customer to the list
                    break;
                case AddingOptions.Parcel://if the user wants to add a parcel
                                          //Receives the details of the parcel from the user
                    Console.WriteLine(@"Please enter the following details:");
                    Console.WriteLine("Sender ID (9 digits)");
                    input = Console.ReadLine();
                    int.TryParse(input, out senderId);
                    Console.WriteLine("Target ID (9 digits)-");
                    input = Console.ReadLine();
                    int.TryParse(input, out targetId);
                    Console.WriteLine("Parcel weight (Easy/Medium/Heavy)-");
                    input = Console.ReadLine();
                    WeightCategories.TryParse(input, out weight);
                    Console.WriteLine("Parcel priority (Regular/Fast/Emergency)-");
                    input = Console.ReadLine();
                    Priorities.TryParse(input, out priority);
                    Parcel pa = new Parcel//Creates a new customer
                    {
                        Id = 0,
                        Sender = BLObject.DisplayCustomer(senderId),
                        Target = BLObject.DisplayCustomer(targetId),
                        Drone = null,
                        Weight = weight,
                        Priority = priority,
                        Requested = DateTime.Now,
                        Scheduled = DateTime.MinValue,
                        PickedUp = DateTime.MinValue,
                        Delivered = DateTime.MinValue
                    };
                    BLObject.AddParcel(pa);//Add the parcel to the list
                    break;
                default:
                    Console.WriteLine("ERROR");
                    break;
            };
        }

        static void update(UpdatingOptions choise)
        {
            string input, model, name, phone;
            int id, chargeSlots;
            DateTime time;
            switch (choise)
            {
                case UpdatingOptions.DroneName:
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Enter the new model of the drone:");
                    model = Console.ReadLine();
                    BLObject.UpdateDroneModel(id, model);
                    break;
                case UpdatingOptions.station: //לטפל במה שקורה כשמזינים רק חלק
                    Console.WriteLine("Enter the station ID (4 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Enter new name of the station (optional):");
                    name = Console.ReadLine();
                    Console.WriteLine("Enter new amount of of charge slots in the station (optional):");
                    input = Console.ReadLine();
                    if (input == "") chargeSlots = -1;
                    else int.TryParse(input, out chargeSlots);
                    break;
                    BLObject.UpdateStation(id, name, chargeSlots);
                case UpdatingOptions.Customer:
                    Console.WriteLine("Enter the customer ID (8 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Enter new name of the customer (optional):");
                    name = Console.ReadLine();
                    Console.WriteLine("Enter new phone of the customer (optional):");
                    phone = Console.ReadLine();
                    BLObject.UpdateCustomer(id, name, phone);
                    break;
                case UpdatingOptions.SendDroneToCharge://if the user wants to send the parcel to charge
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    BLObject.SendDroneToCharge(id);
                    break;
                case UpdatingOptions.ReleaseDroneFromCharge://if the user wants to rrlrase the drone from charge
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Enter the time that the drone was in charging:");
                    input = Console.ReadLine();
                    DateTime.TryParse(input, out time);
                    BLObject.ReleaseDroneFromeCharge(id, time);
                    break;
                case UpdatingOptions.AssignParcelToDrone://if the user wants to Assign a parcel to a drone
                                                         //Receives the details of the parcel and the drone frome the user
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    BLObject.AssignParcelToDrone(id);//assign the parcel to the drone
                    break;
                case UpdatingOptions.PickParcelByDrone:// if the user wants to pick up parcel by a drone
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    BLObject.PickParcelByDrone(id);
                    break;
                case UpdatingOptions.DeliverParcelByDrone://if the user wants to deliver the parcel to the customer
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    BLObject.DeliverParcelToCustomer(id);
                    break;
                default:
                    Console.WriteLine("ERROR");
                    break;
            };
        }
    }
}
