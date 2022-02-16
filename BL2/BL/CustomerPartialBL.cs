using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;
using DalApi;
using System.Runtime.CompilerServices;

namespace BL
{
    /// <summary>
    /// Customer BL class
    /// </summary>
    partial class BL
    {
        /// <summary>
        /// AddCustomer
        /// Exception: ObjectAlreadyExistException
        /// creat a new customer with the details:
        /// </summary>
        /// <param name="id">id of the new customer</param>
        /// <param name="name">name of the new customer</param>
        /// <param name="phone">phone of the new customer</param>
        /// <param name="location">location of the new customer</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(int id, string name, string phone, Location location)
        {
            DO.Customer newCustomer = new DO.Customer()
            {
                Id = id,
                Name = name,
                Phone = phone,
                Longitude = location.Longitude,
                Lattitude = location.Latitude
            };
            try { lock (dalObject) { dalObject.AddCustomer(newCustomer); } }
            catch (DO.ObjectAlreadyExistException e) { throw new ObjectAlreadyExistException(e.Message); }
        }


        /// <summary>
        /// UpdateCusomer
        /// Exception: NoChangesToUpdateException
        /// update one or more details for specific customer
        /// </summary>
        /// <param name="id">the customer id</param>
        /// <param name="name">customer name for update name</param>
        /// <param name="phone">customer phone for update phone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCusomer(int id, string name = default(string), string phone = default(string))
        {
            lock (dalObject)
            {
                DO.Customer customer = dalObject.GetCustomer(id);
                if (name != default)
                    customer.Name = name;
                if (phone != default)
                    customer.Phone = phone;
                else if (name == default)
                {
                    throw new NoChangesToUpdateException("Name and phone not change");
                }
                dalObject.UpdateCustomer(customer);
            }
        }


        /// <summary>
        /// GetCustomer
        /// Exception: ObjectNotExistException
        /// return the requested customer from the dt 
        /// </summary>
        /// <param name="requestedId">the customer id</param>
        /// <returns>Customer</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int requestedId)
        {
            lock (dalObject)
            {
                DO.Customer customer;
                try
                {
                    customer = dalObject.GetCustomer(requestedId);
                }
                catch (DO.ObjectNotExistException e){ throw new ObjectNotExistException(e.Message); }
                if(customer.IsDelete) throw new ObjectNotExistException("customer deleted");
                Customer newCustomer = new Customer()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    CurrentLocation = new Location(customer.Lattitude, customer.Longitude),
                    FromCustomer = ParcelsList().Where(parcel => parcel.SenderId == customer.Id).Select(p => CustomerAndParcelToCustomerDelivery(p.Id, p.GetterId)).ToList(),
                    ToCustomer = ParcelsList().Where(parcel => parcel.GetterId == customer.Id).Select(parcel => CustomerAndParcelToCustomerDelivery(parcel.Id, parcel.SenderId)).ToList()
                };
                return newCustomer;
            }
        }


        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="id">customer id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int id) { lock (dalObject) { dalObject.DeleteCustomer(id); } }


        /// <summary>
        /// CustomersList
        /// getting all customers
        /// </summary>
        /// <returns>IEnumerable of CustomerToList</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> CustomersList()
        {
            List<CustomerToList> customersList = new List<CustomerToList>();
            lock (dalObject)
            {
                foreach (var customer in dalObject.CustomerList())
                {
                    CustomerToList newCustomer = new CustomerToList()
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        Phone = customer.Phone,
                        NumOfParcelsDefined = dalObject.ParcelList().Where(p => p.SenderId == customer.Id && p.PickedUp != default(DateTime)).Count(),
                        NumOfParcelsAscribed = dalObject.ParcelList().Where(p => p.SenderId == customer.Id && p.PickedUp == default(DateTime) && p.Delivered != default(DateTime)).Count(),
                        NumOfParcelsCollected = dalObject.ParcelList().Where(p => p.GetterId == customer.Id && p.PickedUp != default(DateTime)).Count(),
                        NumOfParcelsSupplied = dalObject.ParcelList().Where(p => p.GetterId == customer.Id && p.PickedUp == default(DateTime) && p.Delivered != default(DateTime)).Count()
                    };
                    customersList.Add(newCustomer);
                }

                return customersList;
            }
        }
    }
}
