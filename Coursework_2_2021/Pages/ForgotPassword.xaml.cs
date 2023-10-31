using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Page
    {

        string path = (Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\UserLoginDetails.txt");

        public ForgotPassword()
        {
            InitializeComponent();
        }

        // searches for the user using JSON deserialisation/serialisation and if there is a mathch password is displayed
        private void SearchForUser()
        {
            string json = File.ReadAllText(path);
            var people = JsonConvert.DeserializeObject<List<Person>>(json);

            //Filter the list using a property value
            var FindPassword = people.FirstOrDefault(Person => Person.FirstName == FirstNameTB.Text && Person.LastName == LastNameTB.Text && Person.Username == UserNameTB.Text);


            if (FindPassword != null )
            {
                Trace.WriteLine("User found");
                MessageBox.Show("Your password is " + FindPassword.Password );
            }
            else
            {
                Trace.WriteLine("UserNotFound");
                MessageBox.Show("User Has Not Been Found");
                
            }

        }

        private void FindPassword_Click(object sender, RoutedEventArgs e)
        {
            SearchForUser();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());

        }
    }
}
