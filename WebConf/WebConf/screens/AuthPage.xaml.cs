using common;
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
using WebConf.network.packet;

namespace WebConf.screens
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        bool state = true;

        public AuthPage()
        {
            InitializeComponent();
        }

        private void Go_to_login_Click(object sender, RoutedEventArgs e)
        {
            state = !state;
            if (state)
            {
                go_to_login.Content = "Already have an account?";
                title.Content = "Register";
                submitLogin.Content = "Register";
            }
            else {
                go_to_login.Content = "Don't have an account yet?";
                title.Content = "Log In";
                submitLogin.Content = "Log In";
            }
        }

        private async void SubmitLogin_Click_1(object sender, RoutedEventArgs e)
        {
            bool success;

            if (state)
            {
                success = await App.Model.Register(loginInput.Text, passwordInput.Password);
            }
            else {
                success = await App.Model.LogIn(loginInput.Text, passwordInput.Password);
            }

            if (!success) {
                ErrorMessage.Visibility = Visibility.Visible;
                if (!state)
                {
                    loginInput.Text = "";
                    passwordInput.Password = "";
                    ErrorMessage.Visibility = Visibility.Visible;
                    ErrorMessage.Content = "Invalid Credentials";
                }
                else {
                    ErrorMessage.Content = "Username is already taken";

                }
                return;
            }

            App.getWindow().WelcomeText.Content = String.Format("Welcome, {0}!", App.Model.TheUser.name);
            App.getWindow().LogOutBtn.Visibility = Visibility.Visible;
            App.Navigate(new RoomSelectionPage());
        }
    }
}
