using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Serialization;

namespace Airport.Classes
{
    public static class Tools
    {
        public const string DataPathKey = "DataPath";

        /// <summary>
        /// Получить путь к файлу с данными о рейсах из файла конфигурации
        /// </summary>
        public static string GetDataPath()
        {
            string path = ConfigurationManager.AppSettings[DataPathKey];

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new Exception($"Не указан путь к файлу в App.config (параметр {DataPathKey})");
            }

            return path;
        }

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

        /// <summary>
        /// Показать MessageBox с иконкой ошибки
        /// </summary>
        public static void ShowErrorMessage(string message, string title)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
