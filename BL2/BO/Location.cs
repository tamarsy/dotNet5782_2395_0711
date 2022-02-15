using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BO
{
    /// <summary>
    /// Location:
    /// Latitude and Longitude
    /// </summary>
    public class Location : Ilocatable
    {
        public Location(double latitude = 0, double longitude = 0)
        {
            Latitude = latitude;
            Longitude = longitude;
            CurrentLocation = this;
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public Location CurrentLocation { get; set; }

        public override string ToString()
        {
            return $" ({LocationString(Latitude, 'N', 'S')} , {LocationString(Longitude, 'E', 'W')}) ";


            string LocationString(double n, char a, char b)
            {
                double l = n;
                char ch = a;
                if (l < 0)
                {
                    ch = b;
                    l = -l;
                }
                int d = (int)l;
                int m = (int)(60 * (l - d));
                double s = (l - d) * 3600 - m * 60;
                return $"{d}°{m}′{s:0.0}″{ch}";
            }
        }
    }
}
