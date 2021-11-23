using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDAL.DO;

namespace DalObject
{
    public partial class DalObject
    {

        /// <summary>
        /// the function add a new customer to the arry
        /// </summary>
        /// <param name="customer">the new customer to add</param>
        public void AddCustomer(Customer newCustomer)
        {
            if (DataSource.CustomerArr.Exists(drone => drone.Id == newCustomer.Id))
            {
                throw new ObjectAlreadyExistException("Can't add, There is already a customer with this ID");
            }
            DataSource.CustomerArr.Add(newCustomer);
        }



        /// <summary>
        /// view the choosen customer
        /// </summary>
        /// <param name="id">the customer id</param>
        /// <returns></returns>
        public Customer GetCustomer(int id)
        {
            foreach (var item in DataSource.CustomerArr)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            throw new ArgumentException("not found a customer with id = " + id);
        }


        public IEnumerable<Customer> CustomerList()
        {
            List<Customer> CustomerList = new List<Customer>();
            foreach (var item in DataSource.CustomerArr)
            {
                CustomerList.Add(item);
            }
            return CustomerList;
        }
    }
}
