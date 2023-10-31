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
using System.Text.RegularExpressions;

namespace Coursework_2_2021.Pages
{
    /// <summary>
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Page
    {
        public static string FirstName;
        public static string LastName;
        public static string password;

      
        CollectionOfUsers CollectionOfUsers = new CollectionOfUsers();

        public static int id ;
        private bool isDetailsCorrect;

        public SignUp()
        {
            InitializeComponent();

        }

        // creates an instance of person and stores it to a txt file 
        void CreateUser()
        {
            string path = (Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\UserLoginDetails.txt");
            var jsonData = ReadObjectFromFile(path);

            var UserDetailsList = JsonConvert.DeserializeObject<List<Person>>(jsonData) ?? new List<Person>();
            id = UserDetailsList.Count + 1;

            UserDetailsList.Add(new Person()
            {
                Username = Username_TextBox.Text,
                FirstName = First_Name_TextBox.Text,
                LastName = Last_Name_TextBox.Text,
                Password = Password_TextBox.Text,
                ID = UserDetailsList.Count + 1

            }) ;
            //Trace.WriteLine("The Id Is " + Person.ID);

            //CollectionOfUsers.people.Append(Person);
            jsonData = JsonConvert.SerializeObject(UserDetailsList, Formatting.Indented);
            File.WriteAllText(path, jsonData);
           
        }



        private static void WriteObjectToFile(string Path, string jsonContent)
        {
            File.WriteAllText(Path, jsonContent);

        }

        private static string ReadObjectFromFile(string Path)
        {
            string JSON_Content = File.ReadAllText(Path);
            //Trace.WriteLine(JSON_Content);
            return JSON_Content;
        }

        
        void CreateID()
        {
            int user = CollectionOfUsers.people.Count;
            id = user + 1;

        }
        //makes sure all fields are filled out correctly 
        void ValidationForNames()
        {

            if (string.IsNullOrEmpty(First_Name_TextBox.Text) || string.IsNullOrEmpty(Last_Name_TextBox.Text) || string.IsNullOrEmpty(Username_TextBox.Text))
            {
                MessageBox.Show("Please Enter your First Name and Last Name");
                isDetailsCorrect = false;
            }



            else if (Regex.IsMatch(First_Name_TextBox.Text, "[0-9]") || Regex.IsMatch(Last_Name_TextBox.Text, "[0-9]"))
            {
                MessageBox.Show("Please Enter Letters Only for First Name and Last Name");
                isDetailsCorrect = false;
            }

            else
            {
                isDetailsCorrect = true;

                CreateUser();
                NavigationService.Navigate(new Login());

            }
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            ValidationForNames();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Home());
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}

