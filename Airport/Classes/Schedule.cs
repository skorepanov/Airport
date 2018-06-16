using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airport.Classes
{
    public class Schedule
    {
        public List<Plane> Planes { get; set; }
        public int Speed { get; set; }
        public Plane LastPlaneIn { get; set; }
        public Plane LastPlaneOut { get; set; }
        public DateTime CurrentTime { get; set; }
    }
}
