using Airport.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace Airport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // вычитать список рейсов
            PlaneCollection planes = null;

            try
            {
                string path = "../../Data/Planes.xml";

                XmlSerializer serializer = new XmlSerializer(typeof(PlaneCollection));
                StreamReader reader = new StreamReader(path);
                planes = (PlaneCollection)serializer.Deserialize(reader);
                reader.Close();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Ошибка при чтении файла");
                Application.Current.Shutdown();
            }

            InitializeComponent();
        }
    }
}
