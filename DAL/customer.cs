﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public Customer(int id, double lattitude, double longitude, string name, string phone)
            {
                Id = id;
                Lattitude = lattitude;
                Longitude = longitude;
                Name = name;
                Phone = phone;
            }
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Name: " + Name + "\n"
                    + "Phone: " + Phone + "\n"
                     + "Longitude: " + Longitude + "\n"
                     + "Lattitude: " + Lattitude;
            }
        }


    }
}
