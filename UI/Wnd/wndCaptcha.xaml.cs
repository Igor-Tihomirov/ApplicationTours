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

namespace Приложение_Турагенства.UI.Wnd
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class wndCaptcha : Window
    {
        private clCaptchaGenerator Generator = new clCaptchaGenerator();
        public string TransitionWindow { get; set; }
        public wndCaptcha()
        {
            InitializeComponent();
        }

        private void btnCreateCaptcha_Click(object sender, RoutedEventArgs e)
        {
            imgCaptcha.Source = Generator.GenerateCaptcha(imgCaptcha.Width, imgCaptcha.Height);
        }

        private void btnCheckCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (txbxCaptchaText.Text.ToLower() == Generator.CaptchaText.ToLower())
            {
                MessageBox.Show("Верно!");
                
                if (TransitionWindow == "authorization")
                {
                    wndAuthorization authorization = new wndAuthorization();
                    //authorization.countError = 4;
                    authorization.Show();
                    this.Close();
                }
                else
                {
                    wndRegistration registration = new wndRegistration(null);
                    registration.SendingDataOnBasicWindow();
                    wndTours tours = new wndTours();
                    tours.Show();
                    this.Close();
                    
                }                                
            }                
            else
            {
                MessageBox.Show("Ошибка!");
            }                
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            imgCaptcha.Source = Generator.GenerateCaptcha(imgCaptcha.Width, imgCaptcha.Height);
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

        private void txbxCaptchaText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txbxCaptchaText.Text != "")
            {
                btnCheckCaptcha.IsEnabled = true;
                btnCheckCaptcha.Style = (Style)Application.Current.FindResource("ButtonBasicNoSize");
            }
            else
            {
                btnCheckCaptcha.IsEnabled = false;
                btnCheckCaptcha.Style = (Style)Application.Current.FindResource("ButtonNoSizeIsNotEnabled");
            }
        }
    }
}
