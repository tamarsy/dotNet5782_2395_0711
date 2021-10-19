using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            /// <summary>
            /// Drone class
            /// </summary>
            /// <param name="Id">Id of the dron</param>
            /// <param name="Model">The drones model</param>
            /// <param name="MaxWeight">The max weight that the drone can bag</param>
            /// <param name="Status">The drone status</param>
            /// <param name="Battery">The amount of the energy</param>
            public Drone(int id, string model, WeightCategories maxWeight, DroneStatuses status, double battery)
            {
                Id = id;
                Model = model;
                MaxWeight = maxWeight;
                Status = status;
                Battery = battery;
            }
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public DroneStatuses Status { get; set; }
            public double Battery { get; set; }
            public override string ToString()
            {
                return "Id: " + Id;
            }
        }
    }
}
