using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Airport.Classes
{
    public static class Tools
    {
        /// <summary>
        /// Вычитать данные
        /// </summary>
        public static Plane[] LoadPlanes(string path)
        {
            Plane[] planes = null;
            XmlSerializer serializer = new XmlSerializer(typeof(PlaneCollection));

            using (StreamReader reader = new StreamReader(path))
            {
                PlaneCollection collection = (PlaneCollection)serializer.Deserialize(reader);
                planes = collection.Planes.ToArray();
            }

            return planes;
        }
    }
}
