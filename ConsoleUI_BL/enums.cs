namespace ConsoleUI_BL
{
    enum Options { 
        Exit, 
        Adding, 
        Updating, 
        Display, 
        ListsDisplay, 
        Distance 
    };
    enum AddingOptions { 
        Station = 1, 
        Drone, 
        Customer, 
        Parcel 
    };
    enum UpdatingOptions
    {
        DroneName = 1,
        station,
        customer,
        SendDroneToCharge,
        ReleaseDroneFromCharge,
        AssignParcelToDrone,
        PickParcelByDrone,
        DeliverParcelByDrone
    };
    enum DisplayOptions { 
        Station = 1, 
        Drone, 
        Customer, 
        Parcel 
    };
    enum DisplayListOptions { 
        Stations = 1, 
        Drones, 
        Customers, 
        Parcels, 
        UnassignedParcels, 
        StationsWithAvailableCargingSlots 
    };

}
