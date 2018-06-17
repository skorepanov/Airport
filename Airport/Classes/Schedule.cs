using System;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Airport.Classes
{
    public class Schedule : INotifyPropertyChanged
    {
        private int _ImitationSpeed = 1;
        private DateTime _CurrentTime;
        private Plane _LastPlane = null;

        /// <summary>
        /// Массив всех рейсов
        /// </summary>
        public Plane[] Planes { get; set; }

        /// <summary>
        /// Накопление кол-ва пассажиров по часам
        /// </summary>
        public Dictionary<string, int> Accumulation { get; private set; }

        /// <summary>
        /// Скорость имитации
        /// </summary>
        public int ImitationSpeed
        {
            get
            {
                return _ImitationSpeed;
            }
            set
            {
                _ImitationSpeed = value;
                OnPropertyChanged("ImitationSpeed");
            }
        }

        /// <summary>
        /// Текущее время имитации
        /// </summary>
        public DateTime CurrentTime
        {
            get
            {
                return _CurrentTime;
            }
            set
            {
                _CurrentTime = value;
                OnPropertyChanged("CurrentTime");
                OnPropertyChanged("CurrentTimeString");
            }
        }

        /// <summary>
        /// Текущее время имитации строкой
        /// </summary>
        public string CurrentTimeString
        {
            get
            {
                return _CurrentTime.ToString("HH:mm:ss dd.MM.yyyy");
            }
        }

        /// <summary>
        /// Последний рейс на текущий момент
        /// </summary>
        public Plane LastPlane
        {
            get
            {
                return _LastPlane;
            }
            set
            {
                _LastPlane = value;
                OnPropertyChanged("LastPlane");
            }
        }

        #region Поля-счетчики пассажиров
        private int _PeopleNumberOfLastPlaneIn = 0;
        private int _PeopleNumberOfLast24HoursIn = 0;
        private int _PeopleNumberOfAllPlanesIn = 0;
        private int _PeopleNumberOfLastPlaneOut = 0;
        private int _PeopleNumberOfLast24HoursOut = 0;
        private int _PeopleNumberOfAllPlanesOut = 0;
        
        /// <summary>
        /// Кол-во пассажиров последнего рейса (прилёт)
        /// </summary>
        public int PeopleNumberOfLastPlaneIn
        {
            get
            {
                return _PeopleNumberOfLastPlaneIn;
            }
            set
            {
                _PeopleNumberOfLastPlaneIn = value;
                OnPropertyChanged("PeopleNumberOfLastPlaneIn");
            }
        }

        /// <summary>
        /// Кол-во пассажиров за последние 24 часа (прилёт)
        /// </summary>
        public int PeopleNumberOfLast24HoursIn
        {
            get
            {
                return _PeopleNumberOfLast24HoursIn;
            }
            set
            {
                _PeopleNumberOfLast24HoursIn = value;
                OnPropertyChanged("PeopleNumberOfLast24HoursIn");
            }
        }

        /// <summary>
        /// Кол-во пассажиров за всё время (прилёт)
        /// </summary>
        public int PeopleNumberOfAllPlanesIn
        {
            get
            {
                return _PeopleNumberOfAllPlanesIn;
            }
            set
            {
                _PeopleNumberOfAllPlanesIn = value;
                OnPropertyChanged("PeopleNumberOfAllPlanesIn");
            }
        }

        /// <summary>
        /// Кол-во пассажиров последнего рейса (вылет)
        /// </summary>
        public int PeopleNumberOfLastPlaneOut
        {
            get
            {
                return _PeopleNumberOfLastPlaneOut;
            }
            set
            {
                _PeopleNumberOfLastPlaneOut = value;
                OnPropertyChanged("PeopleNumberOfLastPlaneOut");
            }
        }

        /// <summary>
        /// Кол-во пассажиров последнего рейса (вылет)
        /// </summary>
        public int PeopleNumberOfLast24HoursOut
        {
            get
            {
                return _PeopleNumberOfLast24HoursOut;
            }
            set
            {
                _PeopleNumberOfLast24HoursOut = value;
                OnPropertyChanged("PeopleNumberOfLast24HoursOut");
            }
        }

        /// <summary>
        /// Кол-во пассажиров последнего рейса (вылет)
        /// </summary>
        public int PeopleNumberOfAllPlanesOut
        {
            get
            {
                return _PeopleNumberOfAllPlanesOut;
            }
            set
            {
                _PeopleNumberOfAllPlanesOut = value;
                OnPropertyChanged("PeopleNumberOfAllPlanesOut");
            }
        }
        #endregion Поля-счетчики пассажиров

        public Schedule(string path)
        {
            // вычитать данные
            XmlSerializer serializer = new XmlSerializer(typeof(PlaneCollection));

            using (StreamReader reader = new StreamReader(path))
            {
                PlaneCollection collection = (PlaneCollection)serializer.Deserialize(reader);
                Planes = collection.Planes.ToArray();
            }
            
            // проставить случайное количество пассажиров
            Random random = new Random();
            foreach (Plane plane in Planes)
            {
                plane.NumberOfPassengers = random.Next((int)plane.Model);
            }

            // сохранить текущее время и скорость имитации
            CurrentTime = DateTime.Now;
            ImitationSpeed = 1;

            CountPassengers();

            // запустить таймер
            CreateTimer();
        }

        public void CreateTimer()
        {
            Task.Factory.StartNew(() =>
            {
                System.Timers.Timer timer = new System.Timers.Timer();
                timer.Elapsed += ImitationTick;
                timer.Interval = ImitationSpeed * 1000;
                timer.Start();
            });
        }

        public void ImitationTick(object sender, EventArgs e)
        {
            CurrentTime = CurrentTime.AddMilliseconds(ImitationSpeed * 1000);
            CountPassengers();
        }

        /// <summary>
        /// Рассчитать количество пассажиров
        /// </summary>
        public void CountPassengers()
        {
            // определить все прошедшие рейсы
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


            // найти последний рейс
            LastPlane = pastPlanes
                .OrderByDescending(p => p.Time)
                .First();


            // рассчитать количество пассажиров (прилёт)
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
                    .OrderByDescending(p => p.Time)
                    .First()
                    .NumberOfPassengers;

                PeopleNumberOfLast24HoursIn = pastPlanesIn
                    .Where(p => p.Time >= yesterday)
                    .Sum(p => p.NumberOfPassengers);

                PeopleNumberOfAllPlanesIn = pastPlanesIn
                    .Sum(p => p.NumberOfPassengers);
            }


            // рассчитать количество пассажиров (вылет)
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
                    .OrderByDescending(p => p.Time)
                    .First()
                    .NumberOfPassengers;

                PeopleNumberOfLast24HoursOut = pastPlanesOut
                    .Where(p => p.Time >= yesterday)
                    .Sum(p => p.NumberOfPassengers);

                PeopleNumberOfAllPlanesOut = pastPlanesOut
                    .Sum(p => p.NumberOfPassengers);
            }


            // собрать данные для гистограммы - кол-во пассажиров по каждому часу за последние 24 часа
            DateTime time = yesterday;
            Accumulation = new Dictionary<string, int>();

            for (int i = 0; i < 24; i++)
            {
                string range = $"{time.Hour}:00-{time.AddHours(1).Hour}:00";

                int number =
                    (from Plane p in pastPlanes
                     where p.Time.Year == time.Year &&
                         p.Time.Month == time.Month &&
                         p.Time.Day == time.Day &&
                         p.Time.Hour == time.Hour
                     select p.NumberOfPassengers).Sum();
                
                Accumulation.Add(range, number);

                time = time.AddHours(1);
            }
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string info)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion Property changed handler
    }
}
