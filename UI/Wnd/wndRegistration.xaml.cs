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
using System.Windows.Shapes;

namespace Приложение_Турагенства.UI.Wnd
{
    /// <summary>
    /// Логика взаимодействия для wndRegistration.xaml
    /// </summary>
    public partial class wndRegistration : Window
    {
        public wndRegistration()
        {
            InitializeComponent();

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            wndAuthorization authorization = new wndAuthorization();
            authorization.Show();
            this.Close();
        }

        private void btnLogInUser_Click(object sender, RoutedEventArgs e)
        {
            //wndTours.userName = CurrentUser.UserName;
            //wndTours.userType = roles;
            wndTours tours = new wndTours();
            MessageBox.Show("Вход в пользователя произведен успешно!", "", MessageBoxButton.OK, MessageBoxImage.Information);
            tours.Show();
            this.Close();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            control.Style = (Style)Application.Current.FindResource("ButtonNoSizeIsMouseMove");
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            control.Style = (Style)Application.Current.FindResource("ButtonBasicNoSize");
        }
    }
}
