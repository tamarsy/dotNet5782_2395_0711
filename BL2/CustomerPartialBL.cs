using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL;
using DalObject;
using IBL.BO;

namespace IBL
{
    /// <summary>
    /// all the function in BL class that conction to customer
    /// </summary>
    partial class BL
    {
        /// <summary>
        /// creat a new customer with the details:
        /// </summary>
        /// <param name="id">id of the new customer</param>
        /// <param name="name">name of the new customer</param>
        /// <param name="phone">phone of the new customer</param>
        /// <param name="location">location of the new customer</param>
        public void AddCustomer(int id, string name, string phone, Location location)
        {
            IDAL.DO.Customer newCustomer = new IDAL.DO.Customer()
            {
                Id = id,
                Name = name,
                Phone = phone,
                Longitude = location.Longitude,
                Lattitude = location.Latitude
            };
            try
            {
                dalObject.AddCustomer(newCustomer);
            }
            catch (DalObject.ObjectAlreadyExistException e)
            {
                throw new ObjectAlreadyExistException(e.Message);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }



        /// <summary>
        /// update one or more details for specific customer
        /// </summary>
        /// <param name="id">the customer id</param>
        /// <param name="name">customer name for update name</param>
        /// <param name="phone">customer phone for update phone</param>
        public void UpdateCusomer(int id, string name = default(string), string phone = default(string))
        {
            IDAL.DO.Customer customer = dalObject.GetCustomer(id);
            if (name != default)
                customer.Name = name;
            if (phone != default)
                customer.Phone = phone;
            else if (name == default)
            {
                throw new NoChangesToUpdateException();
            }

        }



        /// <summary>
        /// return the requested customer from the dt 
        /// </summary>
        /// <param name="requestedId">the customer id</param>
        /// <returns>Customer</returns>
        public Customer GetCustomer(int requestedId)
        {
            IDAL.DO.Customer customer;

            try
            {
                customer = dalObject.GetCustomer(requestedId);
            }
            catch (DalObject.ObjectNotExistException e)
            {
                throw new ObjectNotExistException(e.Message);
            }
            catch (Exception)
            {
                throw new Exception();
            }


            Customer newCustomer = new Customer()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                CurrentLocation = new Location(customer.Lattitude, customer.Longitude),
                FromCustomer = ParcelsList().Where(parcel => parcel.SenderId == customer.Id).Select(p => CustomerAndParcelToCustomerDelivery(p, p.GetterId)).ToList(),
                ToCustomer = ParcelsList().Where(parcel => parcel.GetterId == customer.Id).Select(parcel => CustomerAndParcelToCustomerDelivery(parcel, parcel.SenderId)).ToList()
        };
            return newCustomer;
        }



        /// <summary>
        /// creat a new CustomerDelivery by the customer and parcel 
        /// </summary>
        /// <param name="parcel">the parcel</param>
        /// <param name="id">the customer id</param>
        /// <returns>CustomerDelivery</returns>
        private CustomerDelivery CustomerAndParcelToCustomerDelivery(ParcelToList parcel, int id)
        {
            IDAL.DO.Customer geterCustomer = dalObject.GetCustomer(id);
            return new CustomerDelivery()
            {
                Id = parcel.Id,
                Weight = parcel.Weight,
                Priority = parcel.Priority,
                Status = parcel.ParcelStatuses,
                Customer = new DeliveryCustomer()
                {
                    Id = geterCustomer.Id,
                    Name = geterCustomer.Name
                }
            };
        }



        /// <summary>
        /// getting all customers
        /// </summary>
        /// <returns>IEnumerable of CustomerToList</returns>
        public IEnumerable<CustomerToList> CustomersList()
        {
            List<CustomerToList> customersList = new List<CustomerToList>();

            foreach (var customer in dalObject.CustomerList())
            {
                CustomerToList newCustomer = new CustomerToList()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    NumOfParcelsDefined = dalObject.ParcelList().Where(p => p.SenderId == customer.Id && p.PickedUp != default(DateTime)).Count(),
                    NumOfParcelsAscribed = dalObject.ParcelList().Where(p => p.SenderId == customer.Id && p.PickedUp == default(DateTime) && p.Delivered != default(DateTime)).Count(),
                    NumOfParcelsCollected = dalObject.ParcelList().Where(p => p.Getter == customer.Id && p.PickedUp != default(DateTime)).Count(),
                    NumOfParcelsSupplied = dalObject.ParcelList().Where(p => p.Getter == customer.Id && p.PickedUp == default(DateTime) && p.Delivered != default(DateTime)).Count()
                };
                customersList.Add(newCustomer);
            }

            return customersList;
        }
    }
}
