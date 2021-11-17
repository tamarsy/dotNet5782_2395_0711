﻿
namespace IDAL
{
    namespace DO
    {
        public struct Drone
        {
            public double Battery;

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
            internal global::ConsoleUI.DroneStatuses Status { get; public set; }

            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Model: " + Model + "\n"
                    + "MaxWeight: " + MaxWeight;
            }
        }
    }
}
