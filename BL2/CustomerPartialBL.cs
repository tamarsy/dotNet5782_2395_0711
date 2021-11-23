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
    partial class BL
    {
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
                FromCustomer = ParcelsList().Where(parcel=> parcel.SenderId == customer.Id)
                    .Select(parcel => ConvertCustomerToCustomerDelivery(parcel, parcel.GetterId)).ToList(),
                ToCustomer = ParcelsList().Where(parcel => parcel.GetterId == customer.Id)
                    .Select(parcel => ConvertCustomerToCustomerDelivery(parcel, parcel.SenderId)).ToList()
            };
            return newCustomer;
        }
        private CustomerDelivery ConvertCustomerToCustomerDelivery(ParcelToList parcel, int id)
        {
            Customer geterCustomer = GetCustomer(id);
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
                    NumOfParcelsDefined = dalObject.ParcelList().Count(p => p.SenderId == customer.Id && p.PickedUp != default(DateTime)),
                    NumOfParcelsAscribed = dalObject.ParcelList().Count(p => p.SenderId == customer.Id && p.PickedUp == default(DateTime) && p.Delivered != default(DateTime)),
                    NumOfParcelsCollected = dalObject.ParcelList().Count(p => p.Getter == customer.Id && p.PickedUp != default(DateTime)),
                    NumOfParcelsSupplied = dalObject.ParcelList().Count(p => p.Getter == customer.Id && p.PickedUp == default(DateTime) && p.Delivered != default(DateTime))
                };
                customersList.Add(newCustomer);
            }

            return customersList;
        }
    }
}
