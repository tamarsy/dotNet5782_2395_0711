using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalObject
{
    public partial class DAL
    {

        /// <summary>
        /// Exception: ObjectAlreadyExistException, ArgumentNullException
        /// the function add a new customer to the arry
        /// </summary>
        /// <param name="customer">the new customer to add</param>
        public void AddCustomer(Customer newCustomer)
        {
            if (newCustomer.Equals(default(Customer)))
                throw new ArgumentNullException("Null argument");
            if (DataSource.CustomerArr.Exists(drone => drone.Id == newCustomer.Id))
                throw new ObjectAlreadyExistException("Can't add, There is already a customer with this ID");
            DataSource.CustomerArr.Add(newCustomer);
        }



        /// <summary>
        /// Exception: ObjectNotExistException
        /// view the choosen customer
        /// </summary>
        /// <param name="id">the customer id</param>
        /// <returns></returns>
        public Customer GetCustomer(int id)
        {
            int i = DataSource.CustomerArr.FindIndex(c => c.Id == id);
            if (i < 0)
                throw new ObjectNotExistException("not found a customer with id = " + id);
            return DataSource.CustomerArr[i];
        }

        /// <summary>
        /// return Customers List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Customer> CustomerList() => DataSource.CustomerArr;



        /// <summary>
        /// Exception: ArgumentNullException, ObjectNotExistException
        /// Update the Customer details
        /// </summary>
        /// <param name="customer"></param>
        public void UpdateCustomer(Customer customer)
        {
            if(customer.Equals(default(Customer)))
                throw new ArgumentNullException("Null argument");
            int i = DataSource.CustomerArr.FindIndex(s => s.Id == customer.Id);
            if(i < 0)
                throw new ObjectNotExistException("not found a customer with id = " + customer.Id);
            DataSource.CustomerArr[i] = customer;
        }




        /// <summary>
        /// Exception: ObjectNotExistException
        /// Delete Customer
        /// </summary>
        /// <param name="customer id"></param>
        public void DeleteCustomer(int Id)
        {
            int i = DataSource.CustomerArr.FindIndex(s => s.Id == Id);
            if (i < 0)
                throw new ObjectNotExistException("not found a customer with id = " + Id);
            DataSource.CustomerArr[i] = new Customer()
            {
                Id = DataSource.CustomerArr[i].Id,
                Name = DataSource.CustomerArr[i].Name,
                Lattitude = DataSource.CustomerArr[i].Lattitude,
                Longitude = DataSource.CustomerArr[i].Longitude,
                Phone = DataSource.CustomerArr[i].Phone,
                IsDelete = true
            };
        }
    }
}
