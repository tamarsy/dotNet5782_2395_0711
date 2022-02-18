using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using DalApi;
using DO;
using Dal;
using System.Runtime.CompilerServices;
using System.IO;

namespace DAL
{
    internal sealed partial class DalXML : IDal
    {

        public void AddCustomer(Customer newCustomer)
        {
            List<Customer> customers = XMLTools.LoadListFromXmlSerializer<Customer>(ConfigPath);
            customers.Add(newCustomer);
            XMLTools.SaveListToXmlSerializer(customers, customersPath);
        }


        public IEnumerable<Customer> CustomerList(Predicate<bool> selectList = null) =>             
          selectList == null ?
              XMLTools.LoadListFromXmlSerializer<Customer>(customersPath) :
              XMLTools.LoadListFromXmlSerializer<Customer>(customersPath).Where(p => selectList(true));

        public void DeleteCustomer(int id)
        {
            List<Customer> customers = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            Customer customer = customers.FirstOrDefault(customer => customer.Id == id);
            customers.Remove(customer);
            customer.IsDelete = true;
            customers.Add(customer);
            XMLTools.SaveListToXmlSerializer(customers, customersPath);
        }

        public Customer GetCustomer(int id)
        {
            try
            {
                Customer customer = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath).FirstOrDefault(item => item.Id == id);
                if (customer.Equals(default(Customer)))
                    throw new KeyNotFoundException("There isnt suitable parcel in the data!");
                return customer;
            }
            catch
            {
                throw new KeyNotFoundException();
            }
        }

        /// <summary>
        /// Exception: ArgumentNullException, ObjectNotExistException
        /// Update the Customer details
        /// </summary>
        /// <param name="customer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer customer)
        {
            if (customer.Equals(default(Customer)))
                throw new ArgumentNullException("Null argument");

            var customers = XMLTools.LoadListFromXmlSerializer<Customer>(customersPath);
            int i = customers.FindIndex(s => s.Id == customer.Id);
            if (i < 0)
                throw new ObjectNotExistException("not found a customer with id = " + customer.Id);
            customers[i] = customer;

            XMLTools.SaveListToXmlSerializer(customers, customersPath);
        }
    }
}