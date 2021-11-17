using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
        public class Drone : Ilocatable
        {
<<<<<<< HEAD
            public int Id { get; set; }
            public int Model { get; set; }
            public WeightCategories MaxWeiht { get; set; }
            public double Battery { get; set; }
            public DroneStatuses Status { get; set; }
            public Location Location { get; set; }
           
           
            //public string ToString()
            //{
            //    return "cc";
            //}
=======
            public string ToString()
            {
                return "drone";
            }
>>>>>>> 38d0674abd17b0bc6c81d9a6b64a0614c75896bc
        }
    
}
