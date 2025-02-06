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

namespace Приложение_Турагенства
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private clCaptchaGenerator generator = new clCaptchaGenerator();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnCreateCaptcha_Click(object sender, RoutedEventArgs e)
        {
            imgCaptcha.Source = generator.GenerateCaptcha(imgCaptcha.Width, imgCaptcha.Height);
        }

        private void btnCheckCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (txbxCaptchaText.Text.ToLower() == generator.CaptchaText.ToLower())
                MessageBox.Show("Верно!");
            else
                MessageBox.Show("Ошибка!");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            imgCaptcha.Source = generator.GenerateCaptcha(imgCaptcha.Width, imgCaptcha.Height);
        }
    }
}
