using System;
using System.Xml.Serialization;

namespace Airport.Classes
{
    [Serializable()]
    public class Plane
    {
        [XmlElement("Model")]
        public PlaneType Model { get; set; }

        [XmlElement("Direction")]
        public PlaneDirection Direction { get; set; }

        [XmlElement("Time")]
        public string TimeStr { get; set; }

        [XmlElement("City")]
        public string City { get; set; }


        private DateTime? _Time = null;

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

        public int NumberOfPassengers { get; set; }

        public string ByString
        {
            get
            {
                {
                    string direction = Direction == PlaneDirection.In
                        ? "прилетел из города"
                        : "вылетел в город";
                    return $"{Model.ToString()} {direction} {City} " +
                        $"в {Time.Value.ToString("HH:mm:ss dd.MM.yyyy")} " +
                        $"с {NumberOfPassengers} пассажирами";
                }
            }
        }
    }

    [Serializable()]
    [XmlRoot("PlaneCollection")]
    public class PlaneCollection
    {
        [XmlArray("Planes")]
        [XmlArrayItem("Plane", typeof(Plane))]
        public Plane[] Planes { get; set; }
    }
}
