﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLApi;
using System.Runtime.CompilerServices;


namespace BL
{
    internal partial class BL
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(int id, string name, string phone, Location loc)
        {
            //creates a new customer in the data level
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
                lock (dl)
                {
                    dl.AddCustomer(dCustomer); //add the new customer to the list in the data level
                }
            }
            catch (DO.CustomerException ex)
            {
                throw new ExistIdException(ex.Message, "-customer");
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(int customerId, string name, string phone)
        {
            lock (dl)
            {
                DO.Customer dCustomer;

                try { dCustomer = dl.GetCustomer(customerId); }
                catch (DO.CustomerException ex)
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

                dl.AddCustomer(dCustomer);
            }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerId)
        {
            DO.Customer dCustomer;
            lock (dl)
            {
                try { dCustomer = dl.GetCustomer(customerId); }
                catch (DO.CustomerException ex)
                {
                    throw new NotExistIDException(ex.Message, "-customer");
                }
            }
            Customer bCustomer = new Customer();
            bCustomer.Id = dCustomer.Id;
            bCustomer.Name = dCustomer.Name;
            bCustomer.Phone = dCustomer.Phone;
            bCustomer.Location = new Location() { Latti = dCustomer.Lattitude, Longi = dCustomer.Longitude };
            bCustomer.ParcelsFrom = getParcelsFromCustomer(customerId);
            bCustomer.ParcelsTo = getParcelsToCustomer(customerId);
            return bCustomer;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetCustomersList()
        {
            lock (dl)
            {
                return from dCustomer in dl.GetCustomersList()
                       select GetCustomersToList(dCustomer);
            }
        }

        /// <summary>
        /// Returns a list of parcels in the customer - from the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private IEnumerable<ParcelInCustomer> getParcelsFromCustomer(int customerId)
        {
            lock (dl)
            {
                return from tempparcel in GetParcelsList()
                       where GetParcel(tempparcel.Id).Sender.Id == customerId//where the parcel sender is the requested customer
                       let target = GetParcel(tempparcel.Id).Target
                       select new ParcelInCustomer
                       {
                           Id = tempparcel.Id,
                           Priority = tempparcel.Priority,
                           SenderOrTarget = new CustomerInParcel { Id = target.Id, Name = target.Name },
                           Status = tempparcel.Status,
                           Weight = tempparcel.Weight,
                       };
            }
        }

        /// <summary>
        /// Returns a list of parcels in the customer - to the customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private IEnumerable<ParcelInCustomer> getParcelsToCustomer(int customerId)
        {
            lock (dl)
            {
                return from tempparcel in GetParcelsList()
                       where GetParcel(tempparcel.Id).Target.Id == customerId//where the parcel target is the requested customer
                       let sender = GetParcel(tempparcel.Id).Sender
                       select new ParcelInCustomer
                       {
                           Id = tempparcel.Id,
                           Priority = tempparcel.Priority,
                           SenderOrTarget = new CustomerInParcel { Id = sender.Id, Name = sender.Name },
                           Status = tempparcel.Status,
                           Weight = tempparcel.Weight,
                       };
            }
        }
        /// <summary>
        /// Returns the customer- in customer to list
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private CustomerToList GetCustomersToList(DO.Customer dcustomer)
        {
            CustomerToList bCustomer = new CustomerToList//creats new customer to list
            {
                Id = dcustomer.Id,
                Name = dcustomer.Name,
                phone = dcustomer.Phone,
                numOfParcelsDelivered = 0,
                numOfParcelsInTheWay = 0,
                numOfParcelsSentAndNotDelivered = 0,
                numOfParclReceived = 0
            };
            lock (dl)
            {
                foreach (ParcelToList parcelFromList in GetParcelsList()) //initialize the values that we dont have in regular customer
                {
                    Parcel parcel = GetParcel(parcelFromList.Id);
                    if ((parcel.Sender.Id == dcustomer.Id) && (parcel.DeliverTime != null))//initialize the number the parcel the customer sent and deliverd
                    {
                        bCustomer.numOfParcelsDelivered++;
                    }
                    if ((parcel.Sender.Id == dcustomer.Id) && ((parcel.DeliverTime == null) && ((parcel.CreateTime != null))))//initialize the number of parcel the customer sent but not deliverd
                    {
                        bCustomer.numOfParcelsSentAndNotDelivered++;
                    }
                    if ((parcel.Target.Id == dcustomer.Id) && (parcel.DeliverTime == null)) //initialize the number of parcel in the way to the customer
                    {
                        bCustomer.numOfParcelsInTheWay++;
                    }
                    if ((parcel.Target.Id == dcustomer.Id) && (parcel.DeliverTime != null))//initialize the number of parcel the customer recived
                    {
                        bCustomer.numOfParclReceived++;
                    }
                }
            }
            return bCustomer;
        }

    }
}
