using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    public enum WeightCategories
    {
        easy, medium, heavy
    }
    public enum Priorities
    {
        regular, fast, emergancy
    }
    public enum DroneStatuses
    {
        vacant, maintanance, sending
    }
    public enum ParcelStatuses
    {
        defined, ascribed, collected, supplied
    }

    public enum BatteryUsage
    {
        Available, Light, Medium, Heavy, Charging 
    }
}
