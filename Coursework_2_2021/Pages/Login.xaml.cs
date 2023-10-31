using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Coursework_2_2021.Pages
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        bool isDetailsCorrect;
        public static string FirstName;
        public static string LastName;
        public static string password;

        string path = (Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\UserLoginDetails.txt");


        public static int id;

        public Login()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayLoggingInText();
            SearchForUser();
        }

        // Searches for user using JSON serialisation and deserialisation and if theres a match they are taken to the next page
        private void SearchForUser()
        {
            string json = File.ReadAllText(path);
            var people = JsonConvert.DeserializeObject<List<Person>>(json);

            //Filter the list using a property value
            var FindUser = people.FirstOrDefault(Person => Person.FirstName == FirstNameTB.Text && Person.LastName == LastNameTB.Text && Person.Password == PasswordTB.Text && Person.Username == UserNameTB.Text);


            if (FindUser != null)
            {
                Trace.WriteLine("User found");
                FirstName = FindUser.FirstName;
                NavigationService.Navigate(new MainPage());
                id = FindUser.ID;
                Trace.WriteLine(id);
            }
            else
            {
                Trace.WriteLine("UserNotFound");
                MessageBox.Show("User Has Not Been Found");
                LoggingInLabel.Visibility = Visibility.Hidden ;
            }

        }

        private void DisplayLoggingInText()
        {
            LoggingInLabel.Visibility = Visibility.Visible;
        }
        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home());
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ForgotPassword());

        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
