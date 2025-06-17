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
using System.Text.RegularExpressions;
using Приложение_Турагенства.Classes;
using Приложение_Турагенства.Data;

namespace Приложение_Турагенства.UI.Wnd
{
    /// <summary>
    /// Логика взаимодействия для wndRegistration.xaml
    /// </summary>
    public partial class wndRegistration : Window
    {
        private Users _currentUser = new Users();
        public wndRegistration(Users selectedUser)
        {
            InitializeComponent();

            if (selectedUser != null)
            {
                _currentUser = selectedUser;
            }
            DataContext = _currentUser;
        }        

        private void btnLogInUser_Click(object sender, RoutedEventArgs e)
        {
            List<Users> listUsers = ToursBase_49_22Entities.GetContext().Users.ToList();

            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentUser.Login))
            {
                errors.AppendLine("Укажите логин");
            }
            else if(listUsers.FirstOrDefault(u => u.Login == _currentUser.Login) != null)
            {
                errors.AppendLine("Пользователь с таким логином уже существует");
            }

            if (string.IsNullOrWhiteSpace(pltxbxPassword.Text) || string.IsNullOrWhiteSpace(pltxbxPasswordAgain.Text))
            {
                errors.AppendLine("Укажите пароль");
            }
            else if(pltxbxPassword.Text != pltxbxPasswordAgain.Text)
            {
                errors.AppendLine("Пароли должны совпадать");
            }
            else
            {
                _currentUser.Password = pltxbxPasswordAgain.Text;
            }
            
            if (string.IsNullOrWhiteSpace(_currentUser.Phone))
            {
                errors.AppendLine("Укажите номер телефона");
            }
            if (string.IsNullOrWhiteSpace(_currentUser.UserName))
            {
                errors.AppendLine("Укажите имя пользователя");
            }
            else if (listUsers.FirstOrDefault(u => u.UserName == _currentUser.UserName) != null)
            {
                errors.AppendLine("Польователь с таким именем уже существует");
            }




            if (_currentUser.Phone != null && _currentUser.Phone != "")
            {
                if (!IsValidPhone(_currentUser.Phone))
                {
                    errors.AppendLine("Форма номера телефона не соответствует");
                }
            }

            if (_currentUser.Surname != null && _currentUser.Surname != "")
            {
                if (!IsOnlyLetters(_currentUser.Surname))
                {
                    errors.AppendLine("Фамилия может состоять только из букв");
                }
            }
            if (_currentUser.Name != null && _currentUser.Name != "")
            {
                if (!IsOnlyLetters(_currentUser.Name))
                {
                    errors.AppendLine("Имя может состоять только из букв");
                }
            }
            if (_currentUser.Patronymic != null && _currentUser.Patronymic != "")
            {
                if (!IsOnlyLetters(_currentUser.Patronymic))
                {
                    errors.AppendLine("Отчество может состоять только из букв");
                }
            }
            if (_currentUser.Email != null && _currentUser.Email != "")
            {
                if (!IsValidEmail(_currentUser.Email))
                {
                    errors.AppendLine("Адрес электронной почты должен содержать символ @");
                }
            }






                
            
            
            



            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }


            

            if (_currentUser.Id == 0)
            {
                Roles role = ToursBase_49_22Entities.GetContext().Roles.FirstOrDefault(r => r.RoleName == "Клиент");
                _currentUser.Roles.Add(role);

                ToursBase_49_22Entities.GetContext().Users.Add(_currentUser);                                                
            }

            try
            {
                ToursBase_49_22Entities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!", "", MessageBoxButton.OK, MessageBoxImage.Information);


                wndCaptcha captcha = new wndCaptcha();
                captcha.TransitionWindow = "registration";
                captcha.Show();
                this.Close();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        public void SendingDataOnBasicWindow()
        {
            wndTours tours = new wndTours();
            wndTours.userName = _currentUser.UserName;
            if(_currentUser.Roles != null)
            {
                List<string> userType = new List<string>();
                foreach (Roles item in _currentUser.Roles)
                {
                    userType.Add(item.RoleName);
                }
                wndTours.userType = userType;
            }            
            MessageBox.Show("Вход в пользователя произведен успешно!", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        static bool IsOnlyLetters(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Zа-яА-ЯёЁ]+$");
        }


        static bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        static bool IsValidPhone(string phone)
        {
            string pattern = @"^8-?\d{3}-?\d{3}-?\d{2}-?\d{2}$";
            return Regex.IsMatch(phone, pattern);
        }


        private void ActivatingSendButton(object sender, TextChangedEventArgs e)
        {
            if (pltxbxLogin.Text != "" && pltxbxPassword.Text != "" && pltxbxPasswordAgain.Text != "" && pltxbxPhone.Text != "" && pltxbxUserName.Text != "")
            {
                btnLogInUser.IsEnabled = true;
                btnLogInUser.Style = (Style)Application.Current.FindResource("ButtonBasicNoSize");
            }
            else
            {
                btnLogInUser.IsEnabled = false;
                btnLogInUser.Style = (Style)Application.Current.FindResource("ButtonNoSizeIsNotEnabled");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            wndAuthorization authorization = new wndAuthorization();
            authorization.Show();
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
