using ProjectPV178.BussinessLayer;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ProjectPV178.Views.Pages;

namespace ProjectPV178.Views
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private async void Button_Click_Signup(object sender, RoutedEventArgs e)
        {
            try
            {
                await PeopleRepository.AddPerson(Username.Text, Name.Text, Surname.Text, Password.Password, IsDoctor.IsChecked ?? false);
                var user = PeopleRepository.CurrentUser;
                this.NavigationService?.Navigate(user.IsDoctor ? new HomePageDoctor() : new HomePagePatient());
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                Username.BorderBrush = Brushes.Red;
                ExceptionsSignUp.Content = "Username already exists";
            }
            catch (ArgumentNullException ex)
            {
                ExceptionsSignUp.Content = "All fields must be filled";

            }
            catch (Exception ex)
            {
                ExceptionsSignUp.Content = "Unknown error";
            }

        }

        private async void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            try
            {
                var user = await PeopleRepository.GetPerson(LoginUsername.Text);
                if (user == null)
                {
                    throw new ArgumentOutOfRangeException();
                }
                if (user.Password != LoginPassword.Password)
                {
                    throw new ArgumentException();
                }
                PeopleRepository.CurrentUser = user;
                this.NavigationService?.Navigate(user.IsDoctor ? new HomePageDoctor() : new HomePagePatient());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ExceptionsLogIn.Content = "Username doesnt exist";
            }
            catch (ArgumentException ex)
            {
                ExceptionsLogIn.Content = "Wrong password";
            }
            catch (Exception ex)
            {
                ExceptionsLogIn.Content = "Unknown error";
            }
        }
    }
}
