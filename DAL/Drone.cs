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
            public Drone(int id, string model, WeightCategories maxWeight)
            {
                Id = id;
                Model = model;
                MaxWeight = maxWeight;
            }
            public int Id { get; set; }
            public string Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Model: " + Model + "\n"
                    + "MaxWeight: " + MaxWeight;
            }
        }
    }
}
