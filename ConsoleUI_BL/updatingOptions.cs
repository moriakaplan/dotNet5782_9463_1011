using System;
using IBL.BO;
using IBL;

namespace ConsoleUI_BL
{
    internal partial class ConsoleUI_BL
    {
        public void update(Ibl blObject)
        {
            string input;
            UpdatingOptions updateChoise;
            int id, chargeSlots;
            string name, phone, model;
            DateTime timeInCharge;

            Console.WriteLine(@"Wich option you want?
1-update drone name
2-update station data
3-update customer data
4-send drone to charge
5-release drone from charge
6-assign darcel to drone
7-pick parcel by drone
8-deliver parcel by drone
");
            input = Console.ReadLine();
            UpdatingOptions.TryParse(input, out updateChoise);
            switch (updateChoise)
            {
                case UpdatingOptions.DroneName:
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Enter the new model of the drone:");
                    model = Console.ReadLine();
                    blObject.UpdateDroneModel(id, model);
                    break;
                case UpdatingOptions.station:
                    Console.WriteLine("Enter the station ID (4 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Enter new name for the station: (optional)");
                    name = Console.ReadLine();
                    if (name == "\n") name = null;
                    Console.WriteLine("Enter the new amount of charging slots in the station (optional):");
                    input = Console.ReadLine();
                    if (!int.TryParse(input, out chargeSlots)) chargeSlots = -1;
                    blObject.UpdateStation(id, name, chargeSlots);
                    break;
                case UpdatingOptions.customer:
                    Console.WriteLine("Enter the customer ID (8 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Enter new name for the customer: (optional)");
                    name = Console.ReadLine();
                    if (name == "\n") name = null;
                    Console.WriteLine("Enter the new phone of the customer (optional):");
                    phone = Console.ReadLine();
                    if (phone == "\n") phone = null;
                    if (name != null || phone != null) blObject.UpdateCustomer(id, name, phone);
                    break;
                case UpdatingOptions.SendDroneToCharge://if the user wants to send the parcel to charge
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    blObject.SendDroneToCharge(id);
                    break;
                case UpdatingOptions.ReleaseDroneFromCharge://if the user wants to rrlrase the drone from charge
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    Console.WriteLine("Enter the time that the drone was in charge:");
                    input = Console.ReadLine();
                    DateTime.TryParse(input, out timeInCharge);
                    blObject.ReleaseDroneFromeCharge(id, timeInCharge);
                    break;
                case UpdatingOptions.AssignParcelToDrone://if the user wants to Assign a parcel to a drone
                                            //Receives the details of the parcel and the drone frome the user
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    blObject.AssignParcelToDrone(id);//assign the parcel to the drone
                    break;
                case UpdatingOptions.PickParcelByDrone:// if the user wants to pick up parcel by a drone
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    blObject.PickParcelByDrone(id);
                    break;
                case UpdatingOptions.DeliverParcelByDrone://if the user wants to deliver the parcel to the customer
                    Console.WriteLine("Enter the drone ID (6 digits):");
                    input = Console.ReadLine();
                    int.TryParse(input, out id);
                    blObject.DeliverParcelByDrone(id);
                    break;
                default:
                    Console.WriteLine("ERROR");
                    break;
            };
        }
    }
}
