using DO;
using DalApi;
using System;

namespace ConsoleUI
{
    enum Options { Exit, Adding, Updating, Display, ListsDisplay, Distance };
    enum AddingOptions { Station = 1, Drone, Customer, Parcel };
    enum UpdatingOptions { Assign = 1, Pick, Deliver, SendToCharge, ReleaseFromCharge };
    enum DisplayOptions { Station = 1, Drone, Customer, Parcel };
    enum DisplayListOptions { Stations = 1, Drones, Customers, Parcels, UnassignedParcels, StationsWithAvailableCargingSlots };

    class ConsoleUI
    {
        static void Main(string[] args)
        {
            IDal dalObject = DalFactory.GetDal();
            string input;
            Options options;
            AddingOptions addChoise;
            UpdatingOptions updateChoise;
            DisplayOptions displayChoise;
            DisplayListOptions displayListChoise;
            int id, senderId, targetId, droneId, chargeSlots;
            double longitude, lattitude;
            string name, phone, model;
            WeightCategories weight;
            Priorities priority;
         
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

                        Console.WriteLine(@"Wich option you want?
1-Add new base station
2-Add new drone
3-Add new customer
4-Add new parcel
");
                        input = Console.ReadLine();
                        AddingOptions.TryParse(input, out addChoise);
                        switch (addChoise)
                        {
                            case AddingOptions.Station://if the user wants to add a station
                                                       //Receives the details of the station from the user
                                Console.WriteLine("Please enter the following details:\nStation ID (4 digits)-");
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
                                Station station = new Station//Creates a new station
                                {
                                    Id = id,
                                    Name = name,
                                    Longitude = longitude,
                                    Lattitude = lattitude,
                                    ChargeSlots = chargeSlots
                                };
                                dalObject.AddStation(station);//Add the station to the list
                                break;
                            case AddingOptions.Drone://if the user wants to add a drone
                                                     //Receives the details of the drone from the user

                                Console.WriteLine("Please enter the following details:\nDrone ID (6 digits)-");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine("Dron model-");
                                model = Console.ReadLine();
                                Console.WriteLine("Dron max weight (Easy/Medium/Heavy)-");
                                input = Console.ReadLine();
                                WeightCategories.TryParse(input, out weight);         
                                Drone drone = new Drone//Creates a new drone
                                {
                                    Id = id,
                                    Model = model,
                                    MaxWeight = weight,
                                };
                                dalObject.AddDrone(drone);//Add the drone to the list
                                break;
                            case AddingOptions.Customer://if the user wants to add a customer
                                                        //Receives the details of the customer from the user
                                Console.WriteLine("Please enter the following details: \nCustomer ID (9 digits)-");
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
                                    Longitude = longitude,
                                    Lattitude = lattitude
                                };
                                dalObject.AddCustomer(customer);//Add the customer to the list
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
                                    Senderld = senderId,
                                    TargetId = targetId,
                                    Droneld = 0, 
                                    Weight = weight,
                                    Priority = priority,
                                    CreateTime = DateTime.Now,
                                    AssociateTime = null,
                                    PickUpTime = null,
                                    DeliverTime = null
                                };
                                dalObject.AddParcel(pa);//Add the parcel to the list
                                break;
                            default:
                                Console.WriteLine("ERROR");
                                break;
                        };
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
                        switch (updateChoise)
                        {
                            case UpdatingOptions.Assign://if the user wants to Assign a parcel to a drone
                                                        //Receives the details of the parcel and the drone frome the user
                                Console.WriteLine("Enter the parcel ID (8 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine("Enter the drone ID (6 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out senderId);
                                dalObject.AssignParcelToDrone(id, senderId);//assign the parcel to the drone
                                break;
                            case UpdatingOptions.Pick:// if the user wants to pick up parcel by a drone
                                Console.WriteLine("Enter the parcel ID (8 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                dalObject.PickParcelByDrone(id);
                                break;
                            case UpdatingOptions.Deliver://if the user wants to deliver the parcel to the customer
                                Console.WriteLine("Enter the parcel ID (8 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                dalObject.DeliverParcelToCustomer(id);
                                break;
                            case UpdatingOptions.SendToCharge://if the user wants to send the parcel to charge
                                Console.WriteLine("Enter the drone ID (6 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out droneId);
                                Console.WriteLine("Enter the station ID (4 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                dalObject.SendDroneToCharge(droneId, id);
                                break;
                            case UpdatingOptions.ReleaseFromCharge://if the user wants to rrlrase the drone from charge
                                Console.WriteLine("Enter the drone ID (6 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out droneId);
                                dalObject.ReleaseDroneFromeCharge(droneId);
                                break;
                            default:
                                Console.WriteLine("ERROR");
                                break;
                        };
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
                                Console.WriteLine(dalObject.GetStation(id));
                                break;
                            case DisplayOptions.Drone://display a drone
                                Console.WriteLine("Enter the drone ID (6 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine(dalObject.GetDrone(id));
                                break;
                            case DisplayOptions.Customer://display a customer
                                Console.WriteLine("Enter the customer ID (9 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine(dalObject.GetCustomer(id));
                                break;
                            case DisplayOptions.Parcel://display a paecel
                                Console.WriteLine("Enter the parcel ID (8 digits):");
                                input = Console.ReadLine();
                                int.TryParse(input, out id);
                                Console.WriteLine(dalObject.GetParcel(id));
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
                                foreach (Station item in dalObject.GetStationsList())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.Drones://display a list of drones
                                Console.WriteLine("The drones are:");
                                foreach (Drone item in dalObject.GetDronesList())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.Customers://display a list of customers
                                Console.WriteLine("The customers are:");
                                foreach (Customer item in dalObject.GetCustomersList())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.Parcels://display a list of parcels
                                Console.WriteLine("The parcels are:");
                                foreach (Parcel item in dalObject.GetParcelsList())
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.UnassignedParcels://display the list of the parcels that not assign to a drone.
                                Console.WriteLine("The parcels that not assigned to drone are:"); 
                                foreach (Parcel item in dalObject.GetParcelsList(x=>x.AssociateTime==null))
                                {
                                    Console.WriteLine(item);
                                }
                                break;
                            case DisplayListOptions.StationsWithAvailableCargingSlots://display the list of the stations that have available charge slots
                                Console.WriteLine("The stations with available charge slots are:"); 
                                foreach (Station item in dalObject.GetStationsList(x=>x.ChargeSlots>0))
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
                            Station st = dalObject.GetStation(id);
                            distance = dalObject.Distance(longitude, lattitude, st.Longitude, st.Lattitude);
                        }
                        else if (input == "customers" || input == "Customers")//if the user want to know the distance between customer and another place
                        {
                            Console.WriteLine("Enter the customer ID: (9 digits) ");
                            input = Console.ReadLine();
                            int.TryParse(input, out id);
                            Customer cu = dalObject.GetCustomer(id);
                            distance = dalObject.Distance(longitude, lattitude, cu.Longitude, cu.Lattitude);
                        }
                        Console.WriteLine("the distance is: "+distance);
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
