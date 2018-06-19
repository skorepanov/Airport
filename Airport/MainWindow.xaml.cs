using System;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using Airport.Classes;

namespace Airport
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Расписание самолётов
        /// </summary>
        public Schedule Schedule { get; set; }

        public MainWindow()
        {
            // загрузить данные
            bool dataLoaded = LoadData();

            if (!dataLoaded)
                return;

            InitializeComponent();

            // задать источник данных для гистограммы
            ((ColumnSeries)ChartAccumulation.Series[0]).ItemsSource = Schedule.Accumulation;
        }

        /// <summary>
        /// Загрузить данные
        /// </summary>
        /// <returns>True в случае успешной загрузки, иначе False</returns>
        private bool LoadData()
        {
            // получить путь к файлу с данными
            string path = string.Empty;

            try
            {
                path = Tools.GetDataPath();
            }
            catch (Exception exc)
            {
                Tools.ShowErrorMessage(exc.Message, "Данные не найдены");
                Close();
                return false;
            }

            // вычитать список рейсов и сформировать расписание
            try
            {
                Schedule = new Schedule(path);
            }
            catch (Exception exc)
            {
                Tools.ShowErrorMessage(exc.Message, "Ошибка при чтении файла");
                Close();
                return false;
            }

            // запустить имитацию
            Schedule.StartImitation();

            return true;
        }
    }
}
