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
        
        public int PeopleNumberOfLastPlaneIn { get; private set; }
        public int PeopleNumberOfLast24HoursIn { get; private set; }
        public int PeopleNumberOfAllPlanesIn { get; private set; }
        public int PeopleNumberOfLastPlaneOut { get; private set; }
        public int PeopleNumberOfLast24HoursOut { get; private set; }
        public int PeopleNumberOfAllPlanesOut { get; private set; }

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

            CountPassengers();
        }

        /// <summary>
        /// Рассчитать количество пассажиров
        /// </summary>
        public void CountPassengers()
        {
            // все прошедшие рейсы
            var pastPlanes = Planes.Where(p => p.Time <= _CurrentTime);

            if (pastPlanes.Count() == 0)
            {
                LastPlane = null;
                PeopleNumberOfLastPlaneIn = 0;
                PeopleNumberOfLast24HoursIn = 0;
                PeopleNumberOfAllPlanesIn = 0;
                PeopleNumberOfLastPlaneOut = 0;
                PeopleNumberOfLast24HoursOut = 0;
                PeopleNumberOfAllPlanesOut = 0;
                return;
            }


            // последний рейс
            LastPlane = pastPlanes
                .OrderByDescending(p => p.Time.Value)
                .First();


            // количество пассажиров (прилёт)
            var pastPlanesIn = pastPlanes.Where(p => p.Direction == PlaneDirection.In);
            DateTime yesterday = _CurrentTime.AddHours(-24);

            if (pastPlanesIn.Count() == 0)
            {
                PeopleNumberOfLastPlaneIn = 0;
                PeopleNumberOfLast24HoursIn = 0;
                PeopleNumberOfAllPlanesIn = 0;
            }
            else
            {
                PeopleNumberOfLastPlaneIn = pastPlanesIn
                    .OrderByDescending(p => p.Time.Value)
                    .First()
                    .NumberOfPassengers;

                PeopleNumberOfLast24HoursIn = pastPlanesIn
                    .Where(p => p.Time >= yesterday)
                    .Sum(p => p.NumberOfPassengers);

                PeopleNumberOfAllPlanesIn = pastPlanesIn
                    .Sum(p => p.NumberOfPassengers);
            }


            // количество пассажиров (вылет)
            var pastPlanesOut = pastPlanes.Where(p => p.Direction == PlaneDirection.Out);
            
            if (pastPlanesOut.Count() == 0)
            {
                PeopleNumberOfLastPlaneOut = 0;
                PeopleNumberOfLast24HoursOut = 0;
                PeopleNumberOfAllPlanesOut = 0;
            }
            else
            {
                PeopleNumberOfLastPlaneOut = pastPlanesOut
                    .OrderByDescending(p => p.Time.Value)
                    .First()
                    .NumberOfPassengers;

                PeopleNumberOfLast24HoursOut = pastPlanesOut
                    .Where(p => p.Time >= yesterday)
                    .Sum(p => p.NumberOfPassengers);

                PeopleNumberOfAllPlanesOut = pastPlanesOut
                    .Sum(p => p.NumberOfPassengers);
            }
        }
    }
}
