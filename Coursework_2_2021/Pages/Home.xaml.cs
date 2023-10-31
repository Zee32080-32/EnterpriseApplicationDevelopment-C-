using System.Windows;
using System.Windows.Controls;

namespace Coursework_2_2021.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>

    public partial class Home : Page
    {
        MainPage MainPage = new MainPage();
        public Home()
        {
            InitializeComponent();
        }

        public static bool CheckIfUserDetailsSet()
        {
            return false;

        }

        private void SignUp_Button(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SignUp());
        }

        private void LogIn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
