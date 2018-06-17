using Airport.Classes;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

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

            // задать источник данных для гистограммы
            ((ColumnSeries)ChartAccumulation.Series[0]).ItemsSource = Schedule.Accumulation;
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
