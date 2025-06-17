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
using Приложение_Турагенства.UI.Pg;
using Приложение_Турагенства.Classes;
using Приложение_Турагенства.Data;
using System.ComponentModel;

namespace Приложение_Турагенства.UI.Wnd
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class wndTours : Window
    {
        public static List<string> userType;
        public static string userName;
        ToolTip toolTip = new ToolTip();

        private void OnWindowClosing(object sender, CancelEventArgs e)
        {
            // Завершаем работу контекста базы данных
            ToursBase_49_22Entities.DisposeContext();

        }

        public wndTours()
        {
            InitializeComponent();

            if (ToursBase_49_22Entities.GetContext() == null)
            {
                MessageBox.Show("Не удалось подключиться к базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }

            if (userName != null)
            {
                lblActivityUser.Visibility = Visibility.Visible;                
                lblUserName.Visibility = Visibility.Visible;
                lblUserRole.Visibility = Visibility.Visible;
                lblUserName.Content = $"Учетная запись: {userName}";

                string roles = "";
                if (userType.Count > 1)
                {
                    roles = "Роли: ";
                    for (int i = 0; i < userType.Count; i++)
                    {
                        roles += $"{userType[i]} ";
                    }
                }
                else
                {
                    roles = $"Роль: {userType[0]}";
                }
                lblUserRole.Content = roles;

            }
            else
            {
                lblActivityUser.Visibility = Visibility.Hidden;
                lblUserName.Visibility = Visibility.Hidden;
                lblUserRole.Visibility = Visibility.Hidden;
            }


            MainFrame.Navigate(new pgTours());
            clManager.MainFrame = MainFrame;                      
        }       

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            control.Style = (Style)Application.Current.FindResource("ButtonNoSizeIsMouseMove");

            if (userName != null)
            {
                string content = $"Учетная запись: {userName}\n";

                if (userType.Count > 1)
                {
                    content += "Роли: ";
                    for(int i = 0; i < userType.Count; i++)
                    {
                        content += $"{userType[i]} ";
                    }
                }
                else
                {
                    content += $"Роль: {userType[0]}";
                }
                toolTip.Content = content;
            }
            else
            {
                toolTip.Content = "Вы не вошли в учетную запись";
            }
            control.ToolTip = toolTip;
            toolTip.IsOpen = true;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            control.Style = (Style)Application.Current.FindResource("ButtonBasicNoSize");

            toolTip.IsOpen = false;

        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            clManager.MainFrame.GoBack();            
        }       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(userType != null)
            {
                 MessageBoxResult resultQuestion = MessageBox.Show("Вы хотите сменить пользователя?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if(resultQuestion == MessageBoxResult.No)
                {
                    MessageBox.Show($"Не получилось сменить пользователя.\nТекущая учетная запись: {userName}", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    wndAuthorization authorization = new wndAuthorization();
                    authorization.Show();
                    this.Close();
                }
            }
            else
            {
                wndAuthorization authorization = new wndAuthorization();
                authorization.Show();
                this.Close();
            }
        }
       
        private void Window_Activated(object sender, EventArgs e)
        {
            if (userName != null)
            {
                btnAuthorization.Content = "Вход выполнен";
            }
        }       
    }
}
