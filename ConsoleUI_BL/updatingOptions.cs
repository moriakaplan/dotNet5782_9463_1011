using System;
using IBL.BO;
using IBL;

namespace ConsoleUI_BL
{
    public partial class ConsoleUI_BL
    {
        public static void update(Ibl blObject)
        {
            try
            {
                string input;
                UpdatingOptions updateChoise;
                int id, chargeSlots;
                string name, phone, model;
                TimeSpan timeInCharge;

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
                        try { blObject.UpdateDroneModel(id, model); }
                        catch (IBL.NotExistIDException)
                        {
                            Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                        }
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
                        try { blObject.UpdateStation(id, name, chargeSlots); }
                        catch (IBL.NotExistIDException)
                        {
                            Console.WriteLine("this id not exist, please check again what is the id of the station that you want to change and try again\n");
                        }
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
                        if (name != null || phone != null)
                        {
                            try { blObject.UpdateCustomer(id, name, phone); }
                            catch (IBL.NotExistIDException)
                            {
                                Console.WriteLine("this id not exist, please check again what is the id of the customer that you want to change and try again\n");
                            }
                        }
                        break;
                    case UpdatingOptions.SendDroneToCharge://if the user wants to send the parcel to charge
                        Console.WriteLine("Enter the drone ID (6 digits):");
                        input = Console.ReadLine();
                        int.TryParse(input, out id);
                        try { blObject.SendDroneToCharge(id); }
                        catch (IBL.NotExistIDException)
                        {
                            Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                        }
                        catch (IBL.DroneCantGoToChargeException ex) { Console.WriteLine(ex.Message); }
                        break;
                    case UpdatingOptions.ReleaseDroneFromCharge://if the user wants to rrlrase the drone from charge
                        Console.WriteLine("Enter the drone ID (6 digits):");
                        input = Console.ReadLine();
                        int.TryParse(input, out id);
                        Console.WriteLine("Enter the time that the drone was in charge:");
                        input = Console.ReadLine();
                        TimeSpan.TryParse(input, out timeInCharge);
                        try { blObject.ReleaseDroneFromeCharge(id, timeInCharge); }
                        catch (IBL.NotExistIDException ex)
                        {
                            //Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                            Console.WriteLine(ex.Message);
                        }
                        catch (IBL.DroneCantReleaseFromChargeException ex) { Console.WriteLine(ex.Message); }
                        break;
                    case UpdatingOptions.AssignParcelToDrone://if the user wants to Assign a parcel to a drone
                                                             //Receives the details of the parcel and the drone frome the user
                        Console.WriteLine("Enter the drone ID (6 digits):");
                        input = Console.ReadLine();
                        int.TryParse(input, out id);
                        try { blObject.AssignParcelToDrone(id); }//assign the parcel to the drone
                        catch (IBL.NotExistIDException)
                        {
                            Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                        }
                        catch (IBL.DroneCantTakeParcelException ex) { Console.WriteLine(ex.Message); }
                        break;
                    case UpdatingOptions.PickParcelByDrone:// if the user wants to pick up parcel by a drone
                        Console.WriteLine("Enter the drone ID (6 digits):");
                        input = Console.ReadLine();
                        int.TryParse(input, out id);
                        try { blObject.PickParcelByDrone(id); }
                        catch (IBL.NotExistIDException)
                        {
                            Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                        }
                        catch (IBL.DroneCantTakeParcelException ex) { Console.WriteLine(ex.Message); }
                        break;
                    case UpdatingOptions.DeliverParcelByDrone://if the user wants to deliver the parcel to the customer
                        Console.WriteLine("Enter the drone ID (6 digits):");
                        input = Console.ReadLine();
                        int.TryParse(input, out id);
                        try { blObject.DeliverParcelByDrone(id); }
                        catch (IBL.NotExistIDException)
                        {
                            Console.WriteLine("this id not exist, please check again what is the id of the drone that you want to change and try again\n");
                        }
                        catch (IBL.DroneCantTakeParcelException ex) { Console.WriteLine(ex.Message); }
                        break;
                    default:
                        Console.WriteLine("ERROR");
                        break;
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ", please try again");
            }
        }
    }
}
