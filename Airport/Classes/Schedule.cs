using System;
using System.Linq;
using System.IO;
using System.Xml.Serialization;

namespace Airport.Classes
{
    public class Schedule
    {
        public Plane[] Planes { get; set; }
        public int ImitationSpeed { get; set; }
        public Plane LastPlaneIn { get; set; }
        public Plane LastPlaneOut { get; set; }
        public DateTime CurrentTime { get; set; }

        public Schedule(string path)
        {
            // вычитать данные
            PlaneCollection planes = null;

            XmlSerializer serializer = new XmlSerializer(typeof(PlaneCollection));

            using (StreamReader reader = new StreamReader(path))
            {
                planes = (PlaneCollection)serializer.Deserialize(reader);
            }

            // отсортировать рейсы по дате
            if (planes != null)
            {
                Planes = planes.Planes.OrderBy(p => p.Time).ToArray();
            }

            // сохранить текущее время и скорость имитации
            CurrentTime = DateTime.Now;
            ImitationSpeed = 1;
        }
    }
}
