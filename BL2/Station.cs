using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        public class Station : Ilocatable
        {
            public Station(int id, string name, int chargeSlot, Location currentLocation)
            {
                Id = id;
                Name = name;
                Location CurrentLocation = currentLocation;
                ChargeSlot = chargeSlot;
                List<Drone> DronesInCharge = new List<Drone>();
            }
            public int Id { get; set; }
            public string Name { get; set; }
            public Location CurrentLocation { get; set; }
            public int ChargeSlot { get; set; }
            public List<Drone> DronesInCharge { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Name: " + Name + "\n"
                    + "CurrentSiting:" + CurrentLocation + "\n"
                    + "ChargeSlot: " + ChargeSlot;
            }
        }
    }
}
