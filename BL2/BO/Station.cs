using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    public class Station : Ilocatable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location CurrentLocation { get; set; }
        public int ChargeSlot { get; set; }
        public List<Drone> DronesInCharge { get; set; }
        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "Name: " + Name + "\n"
                + "CurrentLocation:" + CurrentLocation + "\n"
                + "ChargeSlot: " + ChargeSlot;
        }
    }
}
