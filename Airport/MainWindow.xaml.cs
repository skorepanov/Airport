using Airport.Classes;
using System;
using System.Configuration;
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
        public Schedule Schedule { get; set; }

        public MainWindow()
        {
            bool dataLoaded = LoadData();

            if (!dataLoaded)
                return;

            InitializeComponent();
        }

        private bool LoadData()
        {
            // получить путь к файлу с данными из app.config
            string settingName = "DataPath";
            string path = ConfigurationManager.AppSettings[settingName];

            if (string.IsNullOrWhiteSpace(path))
            {
                MessageBox.Show($"Не указан путь к файлу в app.config (параметр {settingName})",
                    "Данные не найдены", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return false;
            }

            // вычитать список рейсов
            try
            {
                Schedule = new Schedule(path);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Ошибка при чтении файла",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
                return false;
            }

            return true;
        }
    }
}
