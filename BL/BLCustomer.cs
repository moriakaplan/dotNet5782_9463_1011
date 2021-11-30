﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
{
    public partial class BL 
    {
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
            //try
            //{
               dl.AddCustomerToTheList(dCustomer);
            //add the new station to the list in the data level
            // }
            // catch(Exception ex)
            //{
            //    throw new ExistIdException(ex.Message, ex)
            // }
        }
        public void UpdateCustomer(int customerId, string name, string phone)
        {
            IDAL.DO.Customer dCustomer = dl.DisplayCustomer(customerId);
            dl.DeleteCustomer(customerId);
            if (name != null)//update the name
            {
                dCustomer.Name = name;
            }
            if (phone != null)//update the charge slots
            {
                dCustomer.Phone = phone;
            }
            dl.AddCustomerToTheList(dCustomer);

        }
        public Customer DisplayCustomer(int customerId)
        {
            IDAL.DO.Customer dCustomer = dl.DisplayCustomer(customerId);
            Customer bCustomer = new Customer();
            bCustomer.Id = dCustomer.Id;
            bCustomer.Name = dCustomer.Name;
            bCustomer.Phone = dCustomer.Phone;
            bCustomer.Location = new Location() { Latti = dCustomer.Lattitude, Longi = dCustomer.Lattitude };
            bCustomer.parcelFrom = getCustomerParcelFrom(customerId);
            bCustomer.parcelTo = getCustomerParcelTo(customerId);
            return bCustomer;
        }
        private IEnumerable<ParcelInCustomer> getCustomerParcelFrom( int customerId)
        {
            List<ParcelInCustomer> result = new List<ParcelInCustomer>();
            ParcelInCustomer parcel=new ParcelInCustomer();
            foreach (ParcelToList tempparcel in DisplayListOfParcels())
            {
                if(DisplayParcel(tempparcel.Id).Sender.Id==customerId)
                {
                    parcel.Id = tempparcel.Id;
                    parcel.Priority = tempparcel.Priority;
                    parcel.SenderOrTarget=new CustomerInParcel{ Id= DisplayParcel(tempparcel.Id).Target.Id, Name= DisplayParcel(tempparcel.Id).Target.Name };
                    parcel.Status = tempparcel.Status;
                    parcel.Weight = tempparcel.Weight;
                    result.Add(parcel);
                }
            }
            return result;
        }
        private IEnumerable<ParcelInCustomer> getCustomerParcelTo(int customerId)
        {
            List<ParcelInCustomer> result = new List<ParcelInCustomer>();
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
                    result.Add(parcel);
                }
            }
            return result;
        }
        private CustomerToList DisplayCustomersToList(int customerId)
        {
            IDAL.DO.Customer dcustomer = dl.DisplayCustomer(customerId);
            CustomerToList bCustomer = new CustomerToList
            {
                Id = dcustomer.Id,
                Name = dcustomer.Name,
                phone = dcustomer.Phone
            };
            //bCustomer.
            return bCustomer;
        }
        public IEnumerable<CustomerToList> DisplayListOfCustomers()
        {
            List<CustomerToList> result = new List<CustomerToList>(null);
            foreach (IDAL.DO.Customer dCustomer in dl.DisplayListOfCustomers())
            {
                result.Add(DisplayCustomersToList(dCustomer.Id));
            }
            return result;
        }
    }
}
