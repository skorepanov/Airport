using System;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Serialization;

namespace Airport.Classes
{
    /// <summary>
    /// Самолёт
    /// </summary>
    [Serializable()]
    public class Plane
    {
        /// <summary>
        /// Модель
        /// </summary>
        [XmlElement("Model")]
        public PlaneType Model { get; set; }

        /// <summary>
        /// Направление
        /// </summary>
        [XmlElement("Direction")]
        public PlaneDirection Direction { get; set; }

        /// <summary>
        /// Время вылета/прилёта строкой
        /// </summary>
        [XmlElement("Time")]
        public string TimeStr { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        [XmlElement("City")]
        public string City { get; set; }
        

        private DateTime? _Time = null;

        /// <summary>
        /// Время вылета/прилёта
        /// </summary>
        public DateTime? Time
        {
            get
            {
                if (_Time != null)
                    return _Time;

                if (!string.IsNullOrWhiteSpace(TimeStr))
                {
                    try
                    {
                        _Time = DateTime.Parse(TimeStr);
                    }
                    catch { }
                }

                return _Time;
            }
        }

        /// <summary>
        /// Количество пассажиров
        /// </summary>
        public int NumberOfPassengers { get; set; }

        /// <summary>
        /// Данные о самолёте
        /// </summary>
        public string ByString
        {
            get
            {
                {
                    Type type = typeof(PlaneType);
                    MemberInfo[] memInfo = type.GetMember(Model.ToString());
                    object[] attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    string modelStr = ((DescriptionAttribute)attributes[0]).Description;
                    
                    string direction = Direction == PlaneDirection.In
                        ? "прилетел из города"
                        : "вылетел в город";

                    return $"{modelStr} {direction} {City} " +
                        $"в {Time.Value.ToString("HH:mm:ss dd.MM.yyyy")} " +
                        $"с {NumberOfPassengers} пассажирами";
                }
            }
        }
    }

    /// <summary>
    /// Коллекция самолётов
    /// </summary>
    [Serializable()]
    [XmlRoot("PlaneCollection")]
    public class PlaneCollection
    {
        /// <summary>
        /// Самолёты
        /// </summary>
        [XmlArray("Planes")]
        [XmlArrayItem("Plane", typeof(Plane))]
        public Plane[] Planes { get; set; }
    }
}
