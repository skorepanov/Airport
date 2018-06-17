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
        public Plane LastPlane { get; set; }
        
        public int PeopleNumberOfLastPlaneIn { get; set; }
        public int PeopleNumberOfLast24HoursIn { get; set; }
        public int PeopleNumberOfAllPlanesIn { get; set; }
        public int PeopleNumberOfLastPlaneOut { get; set; }
        public int PeopleNumberOfLast24HoursOut { get; set; }
        public int PeopleNumberOfAllPlanesOut { get; set; }

        private DateTime _CurrentTime;

        public string CurrentTimeString
        {
            get
            {
                return _CurrentTime.ToString("HH:mm:ss dd.MM.yyyy");
            }
        }

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

            // проставить случайное количество пассажиров
            Random random = new Random();
            foreach (Plane plane in Planes)
            {
                plane.NumberOfPassengers = random.Next((int)plane.Model);
            }

            // сохранить текущее время и скорость имитации
            _CurrentTime = DateTime.Now;
            ImitationSpeed = 1;
        }
    }
}
