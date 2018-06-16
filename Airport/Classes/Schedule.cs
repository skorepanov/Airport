using System;
using System.IO;
using System.Xml.Serialization;

namespace Airport.Classes
{
    public class Schedule
    {
        public PlaneCollection Planes { get; set; }
        public int Speed { get; set; }
        public Plane LastPlaneIn { get; set; }
        public Plane LastPlaneOut { get; set; }
        public DateTime CurrentTime { get; set; }

        public Schedule(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlaneCollection));

            using(StreamReader reader = new StreamReader(path))
            {
                Planes = (PlaneCollection)serializer.Deserialize(reader);
            }
        }
    }
}
