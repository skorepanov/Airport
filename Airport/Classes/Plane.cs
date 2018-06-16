using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Classes
{
    public class Plane
    {
        public PlaneType Model { get; set; }
        public PlaneDirection Direction { get; set; }
        public DateTime Time { get; set; }
        public string City { get; set; }
        public int NumberOfPassengers { get; set; }
    }
}
