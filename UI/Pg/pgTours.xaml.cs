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
using Приложение_Турагенства.Data;
using Приложение_Турагенства.Classes;
using Приложение_Турагенства.UI.Pg;
using Приложение_Турагенства.UI.Wnd;

namespace Приложение_Турагенства.UI.Pg
{
    /// <summary>
    /// Логика взаимодействия для pgTours.xaml
    /// </summary>
    public partial class pgTours : Page
    {
        public pgTours()
        {
            InitializeComponent();            
            
            var allTypes = ToursBase_49_22Entities.GetContext().Type.ToList();
            allTypes.Insert(0, new Data.Type { Name = "Все типы" });
            ComboType.ItemsSource = allTypes;

            chbActual.IsChecked = true;
            ComboType.SelectedIndex = 0;

            var currentTours = ToursBase_49_22Entities.GetContext().Tour.ToList();
            lvwTours.ItemsSource = currentTours;

            if (wndTours.userName == null)
            {
                btnOpenListHotels.IsEnabled = false;
                btnOpenListHotels.Visibility = Visibility.Hidden;
            }
            else
            {
                btnOpenListHotels.IsEnabled = true;
                btnOpenListHotels.Visibility = Visibility.Visible;

            }



            UpdateTours();
        }

        private void UpdateTours()
        {
            var currentTours = ToursBase_49_22Entities.GetContext().Tour.ToList();

            if (ComboType.SelectedIndex > 0)
            {
                currentTours = currentTours.Where(p => p.Type.Contains(ComboType.SelectedItem as Data.Type)).ToList();
            }
            currentTours = currentTours.Where(p => p.Name.ToLower().Contains(txtbxSearch.Text.ToLower())).ToList();

            if (chbActual.IsChecked.Value)
            {
                currentTours = currentTours.Where(p => p.IsActual).ToList();
            }
            lvwTours.ItemsSource = currentTours.OrderBy(p => p.TicketCount).ToList();

            if (currentTours.Count != 0)
            {                
                lblCountData.Content = $"Найдено записей по вашему запросу: {currentTours.Count}";
                lblCountData.Visibility = Visibility.Visible;
            }
            else
            {
                lblCountData.Content = $"По вашему запросу ничего не найдено";
                lblCountData.Visibility = Visibility.Visible;
            }           
        }


        private void txtbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTours();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTours();
        }

        private void chbActual_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTours();
        }

        private void lvwTours_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //pgHotels pageHotels = new pgHotels();
            //pageHotels.selectedTour = lvwTours.SelectedItems[0] as Tour;
            //clManager.MainFrame.Navigate(new pgHotels());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
