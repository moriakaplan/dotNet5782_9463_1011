using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL
    {
        /// <summary>
        /// add customer to the kist of the customers
        /// </summary>
        /// <param name="customer"></param>
        public void AddCustomer(Customer customer)
        {
            //creates a new station in the data level
            IDAL.DO.Customer dCustomer = new IDAL.DO.Customer
            {
                Id = customer.Id,
                Name = customer.Name,
                Lattitude = customer.Location.Latti,
                Longitude = customer.Location.Longi,
                Phone = customer.Phone
            };
            try
            {
            dl.AddCustomerToTheList(dCustomer);
            //add the new customer to the list in the data level
             }
             catch(IDAL.DO.CustomerException ex)
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
            IDAL.DO.Customer dCustomer;
            try { dCustomer = dl.DisplayCustomer(customerId); }
            catch(IDAL.DO.CustomerException ex)
            {
                throw new NotExistIDExeption(ex.Message, "-customer");
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
            IDAL.DO.Customer dCustomer;
            try { dCustomer = dl.DisplayCustomer(customerId); }
            catch (IDAL.DO.CustomerException ex)
            {
                throw new NotExistIDExeption(ex.Message, "-customer");
            }
            Customer bCustomer = new Customer();
            bCustomer.Id = dCustomer.Id;
            bCustomer.Name = dCustomer.Name;
            bCustomer.Phone = dCustomer.Phone;
            bCustomer.Location = new Location() { Latti = dCustomer.Lattitude, Longi = dCustomer.Lattitude };
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
                numOfParclRecived=0
            };
            foreach (ParcelToList parcelFromList in DisplayListOfParcels())
            {
                Parcel parcel = DisplayParcel(parcelFromList.Id);
                if ((parcel.Sender.Id == customerId) && (parcel.DeliverTime != DateTime.MinValue))
                {
                    bCustomer.numOfParcelsDelivered++;
                }
                if ((parcel.Sender.Id == customerId) && ((parcel.DeliverTime == DateTime.MinValue) && ((parcel.CreateTime != DateTime.MinValue))))
                {
                    bCustomer.numOfParcelsSentAndNotDelivered++;
                }
                if ((parcel.Target.Id == customerId) && (parcel.DeliverTime == DateTime.MinValue))
                {
                    bCustomer.numOfParcelsInTheWay++;
                }
                if ((parcel.Target.Id == customerId) && (parcel.DeliverTime != DateTime.MinValue))
                {
                    bCustomer.numOfParclRecived++;
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
            foreach (IDAL.DO.Customer dCustomer in dl.DisplayListOfCustomers())
            {
                yield return DisplayCustomersToList(dCustomer.Id);
            }
        }
    }
}
