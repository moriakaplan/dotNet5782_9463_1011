using System;
using IBL.BO;
using IBL;
using System.Collections.Generic;

namespace ConsoleUI_BL
{
    public partial class ConsoleUI_BL
    {
        public void add(Ibl blObject)
        {
            string input;
            AddingOptions addChoise;
            int id, senderId, targetId, stationId, chargeSlots;
            double longitude, lattitude;
            string name, phone, model;
            WeightCategories weight;
            Priorities priority;
            Location location;

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
                    Console.WriteLine(@"Please enter the following details:
Station ID (4 digits)-");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Station name-");
                    name = Console.ReadLine();
                    Console.WriteLine(@"Station location:
longitude-");
                    input = Console.ReadLine();
                    double.TryParse(input, out longitude);
                    Console.WriteLine("lattitude-");
                    input = Console.ReadLine();
                    double.TryParse(input, out lattitude);
                    location = new Location { Longi = longitude, Latti = lattitude };
                    Console.WriteLine("Available charge slots-");
                    input = Console.ReadLine();
                    int.TryParse(input, out chargeSlots);
                    Station station = new Station//Creates a new station
                    {
                        Id = id,
                        Name = name,
                        Location = location,
                        AvailableChargeSlots = chargeSlots
                    };
                    blObject.AddStation(station);//Add the station to the list
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
                    Console.WriteLine("Enter ID of station for first charging of the drone-");
                    input = Console.ReadLine();
                    int.TryParse(input, out stationId);
                    location = blObject.DisplayStation(stationId).Location;
                    Drone drone = new Drone//Creates a new drone
                    {
                        Id = id,
                        Model = model,
                        MaxWeight = weight, 
                        CurrentLocation = location
                    };
                    blObject.AddDrone(drone);//Add the drone to the list
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
                    Console.WriteLine(@"customer location:
longitude-");
                    input = Console.ReadLine();
                    double.TryParse(input, out longitude);
                    Console.WriteLine("lattitude-");
                    input = Console.ReadLine();
                    double.TryParse(input, out lattitude);
                    location = new Location { Longi = longitude, Latti = lattitude };
                    Customer customer = new Customer// Creates a new customer
                    {
                        Id = id,
                        Name = name,
                        Phone = phone, 
                        Location=location
                    };
                    blObject.AddCustomer(customer);//Add the customer to the list
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
                    CustomerInParcel sender = new CustomerInParcel { 
                        Id = senderId, 
                        Name = blObject.DisplayCustomer(senderId).Name };
                    CustomerInParcel target = new CustomerInParcel
                    {
                        Id = targetId,
                        Name = blObject.DisplayCustomer(targetId).Name
                    };
                    Parcel pa = new Parcel//Creates a new customer
                    {
                        Sender = sender,
                        Target = target,
                        Weight = weight,
                        Priority = priority,
                        Requested = DateTime.Now,
                        Scheduled = DateTime.MinValue,
                        PickedUp = DateTime.MinValue,
                        Delivered = DateTime.MinValue
                    };
                    blObject.AddParcelToDelivery(pa);//Add the parcel to the list
                    break;
                default:
                    Console.WriteLine("ERROR");
                    break;
            };
        }
    }
}
