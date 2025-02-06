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
using System.Windows.Threading;
using System.Timers;
using Приложение_Турагенства.Classes;
using Приложение_Турагенства.Data;

namespace Приложение_Турагенства.UI.Wnd
{
    /// <summary>
    /// Логика взаимодействия для wndAuthorization.xaml
    /// </summary>
    public partial class wndAuthorization : Window
    {
        private int countError = 0;
        private TimeSpan timeBlockStart;
        public wndAuthorization()
        {
            InitializeComponent();
        }

        private void btnAuthorization_Click(object sender, RoutedEventArgs e)
        {            

            if (countError >= 3)
            {
                TimeSpan co = timeBlockStart.Add(TimeSpan.FromSeconds(15)) - DateTime.Now.TimeOfDay;
                MessageBox.Show($"Попробуйте войти через {co.Seconds} секунд", "", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (txtbxLogin.Text != "" && (txtbxPassword.Text != "" || psbPassword.Password != ""))
            {
                var User = ToursBase_49_22Entities.GetContext().Users;
                var CurrentUser = User.FirstOrDefault(x => x.Login == txtbxLogin.Text && (x.Password == txtbxPassword.Text || x.Password == psbPassword.Password));
                if(CurrentUser != null)
                {
                    var RolesOfUsers = CurrentUser.Roles.ToList();

                    List<string> roles = new List<string>();
                    for (int i = 0; i < RolesOfUsers.Count; i++)
                    {
                        roles.Add(RolesOfUsers[i].RoleName);
                    }
                    
                    wndTours.userName = CurrentUser.UserName;
                    wndTours.userType = roles;
                    wndTours tours = new wndTours();
                    MessageBox.Show("Вход в пользователя произведен успешно!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    tours.Show();
                    this.Close();
                }              
                else
                {
                    countError++;
                    lblTimeForEndBlock.Content = $"Количество попыток осталось: {3 - countError}";
                    if (countError != 3)
                    {
                        MessageBox.Show("Вы ввели неправильный логин или пароль!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    txtbxLogin.Text = "";
                    txtbxLogin.Style = (Style)Application.Current.FindResource("txtbLoginWatermark");
                    ActivatingSendButton();
                }
            }
            else
            {
                countError++;
                lblTimeForEndBlock.Content = $"Количество попыток осталось: {3 - countError}";
                if (countError != 3)
                {
                    MessageBox.Show("Вы ввели неправильный логин или пароль!", "", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                txtbxLogin.Text = "";
                txtbxLogin.Style = (Style)Application.Current.FindResource("txtbLoginWatermark");
                ActivatingSendButton();
            }

            if (countError >= 3)
            {
                DispatcherTimer timer = new DispatcherTimer();
                DispatcherTimer timer1 = new DispatcherTimer();

                timer.Interval = TimeSpan.FromSeconds(15);
                timer1.Interval = TimeSpan.FromSeconds(1);

                timeBlockStart = DateTime.Now.TimeOfDay;
                timer.Start();
                timer1.Start();

                timer1.Tick += (sender1, e1) =>
                {
                    timer1.Start();
                    TimeSpan co1 = timeBlockStart.Add(TimeSpan.FromSeconds(15)) - DateTime.Now.TimeOfDay;
                    lblTimeForEndBlock.Content = $"До конца блокировки {co1.Seconds} секунд";
                };

                timer.Tick += (sender1, e1) =>
                {
                    countError = 0;
                    timer.Stop();
                    timer1.Stop();
                    lblTimeForEndBlock.Content = "";
                    MessageBox.Show($"Блокировка снята", "", MessageBoxButton.OK, MessageBoxImage.Information);
                };

                MessageBox.Show("Попробуйте войти через 15 секунд", "", MessageBoxButton.OK, MessageBoxImage.Information);
                wndCaptcha captcha = new wndCaptcha();
                captcha.Show();
                this.Close();
            }
        }

        private void chbPasswordVisibility_Click(object sender, RoutedEventArgs e)
        {            
            if (chbPasswordVisibility.IsChecked == false)
            {
                chbPasswordVisibility.Style = (Style)Application.Current.FindResource("CheckBoxByDefault");
                txtbxPassword.Visibility = Visibility.Hidden;
                psbPassword.Password = txtbxPassword.Text;
                if (txtbxPassword.IsEnabled == true)
                {
                    psbPassword.Style = (Style)Application.Current.FindResource("PasswordBoxBasic");
                }                    
                int cursorPosition = txtbxPassword.CaretIndex;
                txtbxPassword.Text = "";
                psbPassword.Visibility = Visibility.Visible;
                //psbPassword.Focus();
                lblTitlePassVisibility.Content = "Показать пароль";
            }
            else
            {
                chbPasswordVisibility.Style = (Style)Application.Current.FindResource("CheckBoxIsChecked");
                psbPassword.Visibility = Visibility.Hidden;
                txtbxPassword.Text = psbPassword.Password;
                if(txtbxPassword.IsEnabled == true)
                {
                    txtbxPassword.Style = (Style)Application.Current.FindResource("TextBoxBasic");                    
                }                
                psbPassword.Password = "";
                txtbxPassword.Visibility = Visibility.Visible;
                //txtbxPassword.Focus();
                lblTitlePassVisibility.Content = "Спрятать пароль";
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Control control = (Control)sender;
            if(control.Name == "txtbxLogin")
            {
                if(txtbxLogin.Text == "")
                {
                    psbPassword.Password = "";
                    txtbxPassword.Text = "";
                    psbPassword.IsEnabled = false;
                    txtbxPassword.IsEnabled = false;
                    psbPassword.Style = (Style)Application.Current.FindResource("psbWatermarkIsNotEnabled");
                    txtbxPassword.Style = (Style)Application.Current.FindResource("txtbPassWatermarkIsNotEnabled");
                }
                else
                {
                    psbPassword.IsEnabled = true;
                    txtbxPassword.IsEnabled = true;
                    if(psbPassword.Password != "" || txtbxPassword.Text != "")
                    {
                        psbPassword.Style = (Style)Application.Current.FindResource("PasswordBoxBasic");
                        txtbxPassword.Style = (Style)Application.Current.FindResource("TextBoxBasic");
                        ActivatingSendButton();
                    }
                    else
                    {
                        psbPassword.Style = (Style)Application.Current.FindResource("psbWatermark");
                        txtbxPassword.Style = (Style)Application.Current.FindResource("txtbPasswordWatermark");
                    }                    
                }
            }
            else if(control.Name == "txtbxPassword")
            {
                ActivatingSendButton();
            }
        }

        private void psbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(psbPassword.Password.Length <= 1)
            {
                ActivatingSendButton();
            }            
        }

        private void ActivatingSendButton()
        {
            if (txtbxLogin.Text != "" && (psbPassword.Password != "" || txtbxPassword.Text != ""))
            {
                btnAuthorization.IsEnabled = true;
                btnAuthorization.Style = (Style)Application.Current.FindResource("ButtonBasicNoSize");
            }
            else
            {
                btnAuthorization.IsEnabled = false;
                btnAuthorization.Style = (Style)Application.Current.FindResource("ButtonNoSizeIsNotEnabled");
            }
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

        private void Watermark_GotFocus(object sender, RoutedEventArgs e)
        {
            Control control = (Control)sender;
            switch (control.Name)
            {
                case "txtbxLogin":
                case "txtbxPassword":
                    control.Style = (Style)Application.Current.FindResource("TextBoxBasic");
                    break;
                case "psbPassword":
                    control.Style = (Style)Application.Current.FindResource("PasswordBoxBasic");
                    break;
            }
            control.BorderBrush = Brushes.Blue;
        }

        private void Watermark_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtbxLogin.Text == "")
            {
                txtbxLogin.Style = (Style)Application.Current.FindResource("txtbLoginWatermark");
            }
            else
            {
                if (txtbxPassword.Text == "")
                {
                    txtbxPassword.Style = (Style)Application.Current.FindResource("txtbPasswordWatermark");
                }
                if (psbPassword.Password == "")
                {
                    psbPassword.Style = (Style)Application.Current.FindResource("psbWatermark");
                }
            }
            Control control = (Control)sender;
            control.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#445c93"));
        }

        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            wndRegistration registration = new wndRegistration();
            registration.Show();
            this.Close();
        }



        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            control.BorderBrush = Brushes.Blue;
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            if (control.IsFocused == false)
            {
                control.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#445c93"));
            }
        }        
    }
}
