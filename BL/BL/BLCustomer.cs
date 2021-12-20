using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;

namespace BL
{
    public partial class BL
    {
        /// <summary>
        /// add customer to the kist of the customers
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(int id, string name, string phone, Location loc)
        {
            //creates a new station in the data level
            DO.Customer dCustomer = new DO.Customer
            {
                Id = id,
                Name = name,
                Lattitude = loc.Latti,
                Longitude = loc.Longi,
                Phone = phone
            };
            try
            { 
                dl.AddCustomerToTheList(dCustomer); //add the new customer to the list in the data level
            }
             catch(DO.CustomerException ex)
            {
                throw new ExistIdException(ex.Message, "-customer");
            }
        }
        /// <summary>
        /// Updates customer details(name, phone)
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustomer(int customerId, string name, string phone)
        {
            DO.Customer dCustomer;
            try { dCustomer = dl.DisplayCustomer(customerId); }
            catch(DO.CustomerException ex)
            {
                throw new NotExistIDException(ex.Message, "-customer");
            }
            dl.DeleteCustomer(customerId);
            if (name != null)//update the name
            {
                dCustomer.Name = name;
            }
            if (phone != null)//update the phone
            {
                dCustomer.Phone = phone;
            }
            dl.AddCustomerToTheList(dCustomer);

        }
        /// <summary>
        /// Returns the customer with the requested ID
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public Customer DisplayCustomer(int customerId)
        {
            DO.Customer dCustomer;
            try { dCustomer = dl.DisplayCustomer(customerId); }
            catch (DO.CustomerException ex)
            {
                throw new NotExistIDException(ex.Message, "-customer");
            }
            Customer bCustomer = new Customer();
            bCustomer.Id = dCustomer.Id;
            bCustomer.Name = dCustomer.Name;
            bCustomer.Phone = dCustomer.Phone;
            bCustomer.Location = new Location() { Latti = dCustomer.Lattitude, Longi = dCustomer.Longitude };
            bCustomer.parcelFrom = getCustomerParcelFrom(customerId);
            bCustomer.parcelTo = getCustomerParcelTo(customerId);
            return bCustomer;
        }
        /// <summary>
        /// Returns a list of parcels in the customer - from the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private IEnumerable<ParcelInCustomer> getCustomerParcelFrom(int customerId)
        {
            ParcelInCustomer parcel = new ParcelInCustomer();
            foreach (ParcelToList tempparcel in DisplayListOfParcels())
            {
                if (DisplayParcel(tempparcel.Id).Sender.Id == customerId)
                {
                    parcel.Id = tempparcel.Id;
                    parcel.Priority = tempparcel.Priority;
                    parcel.SenderOrTarget = new CustomerInParcel { Id = DisplayParcel(tempparcel.Id).Target.Id, Name = DisplayParcel(tempparcel.Id).Target.Name };
                    parcel.Status = tempparcel.Status;
                    parcel.Weight = tempparcel.Weight;
                    yield return parcel;
                }
            }
        }
        /// <summary>
        /// Returns a list of parcels in the customer - to the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private IEnumerable<ParcelInCustomer> getCustomerParcelTo(int customerId)
        {
            ParcelInCustomer parcel = new ParcelInCustomer();
            foreach (ParcelToList tempparcel in DisplayListOfParcels())
            {
                if (DisplayParcel(tempparcel.Id).Target.Id == customerId)
                {
                    parcel.Id = tempparcel.Id;
                    parcel.Priority = tempparcel.Priority;
                    parcel.SenderOrTarget = new CustomerInParcel { Id = DisplayParcel(tempparcel.Id).Sender.Id, Name = DisplayParcel(tempparcel.Id).Sender.Name };
                    parcel.Status = tempparcel.Status;
                    parcel.Weight = tempparcel.Weight;
                    yield return parcel;
                }
            }
        }
        /// <summary>
        /// Returns the customer with the requested ID
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private CustomerToList DisplayCustomersToList(int customerId)
        {
            Customer dcustomer = DisplayCustomer(customerId);
            CustomerToList bCustomer = new CustomerToList
            {
                Id = dcustomer.Id,
                Name = dcustomer.Name,
                phone = dcustomer.Phone, 
                numOfParcelsDelivered=0, 
                numOfParcelsInTheWay=0, 
                numOfParcelsSentAndNotDelivered=0, 
                numOfParclReceived=0
            };
            foreach (ParcelToList parcelFromList in DisplayListOfParcels())
            {
                Parcel parcel = DisplayParcel(parcelFromList.Id);
                if ((parcel.Sender.Id == customerId) && (parcel.DeliverTime != null))
                {
                    bCustomer.numOfParcelsDelivered++;
                }
                if ((parcel.Sender.Id == customerId) && ((parcel.DeliverTime == null) && ((parcel.CreateTime != null))))
                {
                    bCustomer.numOfParcelsSentAndNotDelivered++;
                }
                if ((parcel.Target.Id == customerId) && (parcel.DeliverTime == null))
                {
                    bCustomer.numOfParcelsInTheWay++;
                }
                if ((parcel.Target.Id == customerId) && (parcel.DeliverTime != null))
                {
                    bCustomer.numOfParclReceived++;
                }
            }
            return bCustomer;
        }
        /// <summary>
        /// returns the list of the customers
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CustomerToList> DisplayListOfCustomers()
        {
            foreach (DO.Customer dCustomer in dl.DisplayListOfCustomers())
            {
                yield return DisplayCustomersToList(dCustomer.Id);
            }
        }
    }
}
