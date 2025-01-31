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
using Приложение_Турагенства.UI.Wnd;

namespace Приложение_Турагенства.UI.Pg
{
    /// <summary>
    /// Логика взаимодействия для pgHotels.xaml
    /// </summary>
    public partial class pgHotels : Page
    {
        public Tour selectedTour = new Tour();        
        public pgHotels()
        {
            InitializeComponent();
            var currentHotels = ToursBase_49_22Entities.GetContext().Hotel.ToList();


            var allCountries = ToursBase_49_22Entities.GetContext().Country.ToList();
            //List<object> someCountries = new List<object>();
            //for (int i = 0; i < currentHotels.Count; i++)
            //{
            //    if(someCountries.Count != 0)
            //    {
            //        for(int k = 0; i < someCountries.Count; k++)
            //        {
            //            if(someCountries[k] == currentHotels[i].CountryCode)
            //            {
            //                break;
            //            }                        
            //        }
            //        someCountries.Add(allCountries.Where(p => p.Code == currentHotels[i].CountryCode).ToList());
            //    }
            //    else
            //    {
            //        someCountries.Add(allCountries.Where(p => p.Code == currentHotels[i].CountryCode).ToList());
            //    }
            //}
            allCountries.Insert(0, new Data.Country { Name = "Все страны" });
            ComboCountry.ItemsSource = allCountries;
            ComboCountry.SelectedIndex = 0;


            
            if (selectedTour != null)
            {
                currentHotels = currentHotels.Where(p => p.Tour.Contains(selectedTour)).ToList();
                DGridHotels.ItemsSource = currentHotels;
            }
            else if (selectedTour == null)
            {
                DGridHotels.ItemsSource = ToursBase_49_22Entities.GetContext().Hotel.ToList();
            }

            //DGridHotels.ItemsSource = ToursBase_49_22Entities.GetContext().Hotel.ToList();

            if (wndTours.userName == null)
            {
                btnAdd.IsEnabled = false;
                btnAdd.Visibility = Visibility.Hidden;

                btnDelete.IsEnabled = false;
                btnDelete.Visibility = Visibility.Hidden;                
            }
            else 
            {
                List<string> userType = wndTours.userType;
                for(int i = 0; i < userType.Count; i++)
                {
                    if(userType[i] == "Администратор")
                    {
                        btnAdd.IsEnabled = true;
                        btnAdd.Visibility = Visibility.Visible;

                        btnDelete.IsEnabled = true;
                        btnDelete.Visibility = Visibility.Visible;
                        return;
                    }
                    else if (userType[i] == "Менеджер")
                    {
                        btnAdd.IsEnabled = false;
                        btnAdd.Visibility = Visibility.Hidden;

                        btnDelete.IsEnabled = false;
                        btnDelete.Visibility = Visibility.Hidden;

                        DGridHotels.Columns.Remove(DGridHotels.Columns[3]);                       
                    }
                }                               
            }

            UpdateTours();
        }

        private void UpdateTours()
        {
            var currentHotels = ToursBase_49_22Entities.GetContext().Hotel.ToList();

            if(ComboCountry.SelectedIndex > 0)
            {
                var selectedCountry = ComboCountry.SelectedItem as Data.Country;
                var selectedCountryCode = selectedCountry.Code;
                currentHotels = currentHotels.Where(p => p.Country.Code == selectedCountryCode).ToList();
            }
            currentHotels = currentHotels.Where(p => p.Name.ToLower().Contains(txtbxSearch.Text.ToLower())).ToList();
            DGridHotels.ItemsSource = currentHotels.ToList();            
        }

        private void txtbxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTours();
        }

        private void ComboCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTours();
        }



        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            clManager.MainFrame.Navigate(new pgAddEdit((sender as Button).DataContext as Hotel));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            clManager.MainFrame.Navigate(new pgAddEdit(null));
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var hotelsForRemoving = DGridHotels.SelectedItems.Cast<Hotel>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {hotelsForRemoving.Count()} элементов?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    if(hotelsForRemoving.Count != 0)
                    {
                        string errorHotels = "";
                        for (int i = 0; i < hotelsForRemoving.Count; i++)
                        {
                            var countRelatedTours = hotelsForRemoving[0].Tour.Count;
                            if (countRelatedTours != 0)
                            {
                                if (errorHotels != "")
                                {
                                    errorHotels = errorHotels + $", {hotelsForRemoving[i].Name} - {countRelatedTours} записей";
                                }
                                else
                                {
                                    errorHotels = $"{hotelsForRemoving[i].Name} - {countRelatedTours} записей";
                                }
                            }
                        }
                        if (errorHotels != "")
                        {
                            MessageBox.Show($"Нельзя удалить данные, т.к. с ними связано записей: {errorHotels}", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }                    
                    

                    ToursBase_49_22Entities.GetContext().Hotel.RemoveRange(hotelsForRemoving);
                    ToursBase_49_22Entities.GetContext().SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridHotels.ItemsSource = ToursBase_49_22Entities.GetContext().Hotel.ToList();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
       

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(Visibility == Visibility.Visible)
            {
                ToursBase_49_22Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridHotels.ItemsSource = ToursBase_49_22Entities.GetContext().Hotel.ToList();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            clManager.MainFrame.Navigate(new pgTours());
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

        private void btnEdit_Loaded(object sender, RoutedEventArgs e)
        {
            Control btnEdit = (Control)sender;
            if (wndTours.userName == null)
            {
                btnEdit.IsEnabled = false;
                btnEdit.Visibility = Visibility.Hidden;              
            }
            else
            {
                List<string> userType = wndTours.userType;
                for (int i = 0; i < userType.Count; i++)
                {
                    if (userType[i] == "Администратор")
                    {
                        btnEdit.IsEnabled = true;
                        btnEdit.Visibility = Visibility.Visible;
                        return;
                    }
                    else if(userType[i] == "Менеджер")
                    {
                        btnEdit.IsEnabled = false;
                        btnEdit.Visibility = Visibility.Hidden;
                    }
                }
            }
          
        }
    }
}
