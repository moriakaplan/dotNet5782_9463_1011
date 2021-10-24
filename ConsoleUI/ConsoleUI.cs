using System;

namespace ConsoleUI
{
    class ConsoleUI
    {
        static void Main()
        {
            Console.WriteLine(@"Hello! what do you want to do?

Adding options:
1-Add new base station
2-Add new drone
3-Add new customer
4-Add new parcel

Updating options:
5-Assign parcel to drone
6-Pick parcel by drone
7-Deliver parcel to customer
8-Send drone to charge
9-Release drone frome charge

Display options:
10-Display base station
11-Display drone
12-Display customer
13-Display parcel

Display lists options:
14-Display base stations list
15-Display drones list
16-Display customers list
17-Display parcels list
18-Display list of unassigned parcels
19-Display list of stations with available carging slots

20-exit
");
        }
    }
}
