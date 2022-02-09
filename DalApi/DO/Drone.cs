
namespace DO
{
    /// <summary>
    /// Id: Id of the dron
    /// Model: The drones model
    /// MaxWeight: The max weight that the drone can bag
    /// IsDelete: true if the customer is delete
    /// </summary>
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public bool IsDelete { get; set; }
        public override string ToString()
        {
            return "Id: " + Id + "\n"
                + "Model: " + Model + "\n"
                + "MaxWeight: " + MaxWeight;
        }
    }
}

