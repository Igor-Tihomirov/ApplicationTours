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

namespace CustomControls
{    
    public class clPlaceholderTextBox : TextBox
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(
                "Placeholder", typeof(string), typeof(clPlaceholderTextBox), new PropertyMetadata(string.Empty));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register(
                "Radius", typeof(CornerRadius), typeof(clPlaceholderTextBox), new PropertyMetadata(new CornerRadius(0)));

        public CornerRadius Radius
        {
            get => (CornerRadius)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        static clPlaceholderTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(clPlaceholderTextBox), new FrameworkPropertyMetadata(typeof(clPlaceholderTextBox)));
        }

        public clPlaceholderTextBox()
        {
            this.MouseEnter += OnMouseEnter;
            this.MouseLeave += OnMouseLeave;
            this.GotFocus += OnGotFocus;
            this.LostFocus += OnLostFocus;  
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            this.BorderBrush = Brushes.Blue; 
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            if(this.IsFocused == false)
            {
                this.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#445c93"));
            }            
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.BorderBrush = Brushes.Blue;
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#445c93"));
        }
    }
}
