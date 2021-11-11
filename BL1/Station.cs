using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL
{
    namespace BO
    {
        class Station
        {
            public Station(int id, int name, int chargeSlot, Location currentSiting)
            {
                Id = id;
                Name = name;
                Location CurrentSiting = currentSiting;
                ChargeSlot = chargeSlot;
                List<Drone> DronesInCharge = new List<Drone>();
            }
            public int Id { get; set; }
            public int Name { get; set; }
            public Siting CurrentSiting { get; set; }
            public int ChargeSlot { get; set; }
            public override string ToString()
            {
                return "Id: " + Id + "\n"
                    + "Name: " + Name + "\n"
                     + "CurrentSiting:" + CurrentSiting + "\n"
                     + "ChargeSlot: " + ChargeSlot;
            }
        }
    }
}
