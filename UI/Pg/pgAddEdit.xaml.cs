using System;
using System.Collections.Generic;
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
using Приложение_Турагенства.Classes;
using Приложение_Турагенства.Data;


namespace Приложение_Турагенства.UI.Pg
{
    /// <summary>
    /// Логика взаимодействия для pgAddEdit.xaml
    /// </summary>
    public partial class pgAddEdit : Page
    {
        private Hotel _currentHotel = new Hotel();
        public pgAddEdit(Hotel selectedHotel)
        {
            InitializeComponent();

            if(selectedHotel != null)
            {
                _currentHotel = selectedHotel;
            }
            DataContext = _currentHotel;
            ComboCountries.ItemsSource = ToursBase_49_22Entities.GetContext().Country.ToList();
       }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentHotel.Name))
            {                
                errors.AppendLine("Укажите название отеля");
            }
            if (_currentHotel.CountOfStars < 1 || _currentHotel.CountOfStars > 5)
            {
                errors.AppendLine("Количество звезд - число от 1 до 5");
            }
            if (_currentHotel.Country == null)
            {
                errors.AppendLine("Выберите страну");
            }

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if(_currentHotel.Id == 0)
            {
                ToursBase_49_22Entities.GetContext().Hotel.Add(_currentHotel);
            }

            try
            {
                ToursBase_49_22Entities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

            clManager.MainFrame.Navigate(new pgHotels());
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            control.Style = (Style)Application.Current.FindResource("btnIsMouseMove");
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            control.Style = (Style)Application.Current.FindResource("btnBasic");
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            clManager.MainFrame.Navigate(new pgHotels());
        }
    }
}
