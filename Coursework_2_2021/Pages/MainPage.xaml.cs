using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


namespace Coursework_2_2021.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
      
        String Path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\list.txt";
        private bool isDetailsCorrect;

        public MainPage()
        {
            InitializeComponent();
            LoadData();
            WelcomeUser();
          
        }

        private async void AddExpense_Button(object sender, RoutedEventArgs e)
        {
           
            CheckIfExpensesHasBeenEntered();

            if (isDetailsCorrect == true)
            {
                await Create_Instance();
                WelcomeUser();
                ClearTextChildren(ItemNameTextBox);
                ClearDropDownChildren(ItemTypeDropDownList);
                ClearDropDate(DateTextBox);
                ClearTextChildren(AmountOfMoneyTextBox);
            }

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            SeaarthroughPayment();
        }

        private void CreateReport_Click(object sender, RoutedEventArgs e)
        {
            CreateReport();
        }


       // creates a report based fo the users money in and money out, it places the users expenses in a temp list and the list is filtered by a uniquie ID 
        private void CreateReport()
        {
            string str = File.ReadAllText(Path);
            var AllPayments = JsonConvert.DeserializeObject<List<Finances>>(str);

            var UserPayments = AllPayments.Where(Finances => Finances.ID == Login.id).ToList();


            if (UserPayments.Count < 1)
            {
                MessageBox.Show("Not enough Data entries to produce report");
            }
            else
            {
                var FirstThreeWeeks = UserPayments.OrderByDescending(x => DateTime.Parse(x.Date)).Take(4).ToList();


                //dgr.ItemsSource = orderedList;

                int index = FirstThreeWeeks.Count;
                decimal MoneyInSumIndex = FirstThreeWeeks.Where(Finances => Finances.ID == Login.id && Finances.MoneyIn == true).Select(Finances => Finances.Payment).Count();
                decimal MoneyOutSumIndex = FirstThreeWeeks.Where(Finances => Finances.ID == Login.id && Finances.MoneyIn == false).Select(Finances => Finances.Payment).Count();


                decimal MoneyInSum = FirstThreeWeeks.Where(Finances => Finances.ID == Login.id && Finances.MoneyIn == true).Select(Finances => Finances.Payment).Sum();
                decimal MoneyOutSum = FirstThreeWeeks.Where(Finances => Finances.ID == Login.id && Finances.MoneyIn == false).Select(Finances => Finances.Payment).Sum();

                decimal MoneyInmedian = MoneyInSum / MoneyInSumIndex;
                decimal MoneyOutmedian = (MoneyOutSum / MoneyOutSumIndex) ;


                ReportText.Text = "Your expected expenses for money in is £" + decimal.Round(MoneyInmedian, 2, MidpointRounding.AwayFromZero) +
                    "\n and your expected expenses for money spent is £" + decimal.Round(MoneyOutmedian, 2, MidpointRounding.AwayFromZero);



                Trace.WriteLine("Your money in Report is £" + MoneyInmedian);
                Trace.WriteLine("Your money out Report is £" + MoneyOutmedian);
            }

        }

        // creates an instance of Finance and stores it in a json file via deserialisation/serialisation 
        private async  Task  Create_Instance()
        {
           
     
            string path = (Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\list.txt");
            //var jsonData =  ReadObjectFromFile(path);
            var  jsonData = await ReadObjectFromFile(path);
          

            var UserFinanceList = JsonConvert.DeserializeObject<List<Finances>>(jsonData) ?? new List<Finances>();

            bool foo = decimal.TryParse(AmountOfMoneyTextBox.Text, out var decValue);
            
            UserFinanceList.Add(new Finances()
            {
                MoneyIn  = MoneyInRB.IsChecked == true,
               
                ID = Login.id,
                NameOfPayment = ItemNameTextBox.Text,
                PaymentType = ItemTypeDropDownList.Text,
                Payment = decValue,
                Date = DateTextBox.Text



            });

            jsonData = JsonConvert.SerializeObject(UserFinanceList, Formatting.Indented);
            await File.WriteAllTextAsync(path, jsonData);

            // File.WriteAllText(path, jsonData) ;

            LoadData();
            TotalAmountOfCash();

        }
        //Reads file 
        private async static Task<string> ReadObjectFromFile(string Path)
        {
           
            String JSON_Content = await File.ReadAllTextAsync(Path);
            //Trace.WriteLine(JSON_Content);
            return JSON_Content;
        }

        //deserialises json file and places it in a table
        void LoadData()
        {
            string json = File.ReadAllText(Path);
            var AllPayments = JsonConvert.DeserializeObject<List<Finances>>(json);

            Trace.WriteLine("ID found");
            dgr.ItemsSource = AllPayments.Where(Finance => Finance.ID == Login.id);
          
            var MoneyIn = AllPayments.Where(Finance => Finance.MoneyIn == true);
            var MoneyOut = AllPayments.Where(Finance => Finance.MoneyIn == false);

        }

        //Searches for a payment by the name of a payment and users unique ID 
        private void SeaarthroughPayment()
        {
            string json = File.ReadAllText(Path);
            var AllPayments = JsonConvert.DeserializeObject<List<Finances>>(json);

            string NameOfPayment = SearchForUser.Text;

            var Search = AllPayments.FindAll(Finance => Finance.ID == Login.id && Finance.NameOfPayment == NameOfPayment);

            if (Search.Count == 0)
            {
               
                Trace.WriteLine("UserNotFound");
                MessageBox.Show("User Has Not Been Found");

              
            }
            else 
            {
                dgr.ItemsSource = Search;
            }
   

        }


        void DeletePayment(int DataGridRow)
        {
            string json = File.ReadAllText(Path);
            var AllPayments = JsonConvert.DeserializeObject<List<Finances>>(json);

            string NameOfPayment = SearchForUser.Text;

            var Search = AllPayments.FindIndex(Finance => Finance.ID == Login.id && Finance.NameOfPayment == NameOfPayment);

            AllPayments.RemoveAt(Search);


            json = JsonConvert.SerializeObject(Search, Formatting.Indented);
            File.WriteAllText(Path, json);


            Trace.WriteLine(AllPayments);
        }

        // displays the total amount of money spent 
        void TotalAmountOfCash()
        {
            string str = File.ReadAllText(Path);
            var AllPayments = JsonConvert.DeserializeObject<List<Finances>>(str);

            decimal index = AllPayments.Count;
            decimal MoneyInSum = AllPayments.Where(Finances => Finances.ID == Login.id && Finances.MoneyIn == true).Select(Finances => Finances.Payment).Sum();
            decimal MoneyOutSum = AllPayments.Where(Finances => Finances.ID == Login.id && Finances.MoneyIn == false).Select(Finances => Finances.Payment).Sum();

            decimal total = (MoneyInSum - MoneyOutSum);

            if (total < 0)
            {
                TotalCash.Foreground = Brushes.Red;
                TotalCash.Content = "Total Cash: £" + total;
            }
            else
            {
                TotalCash.Foreground = Brushes.Blue;
                TotalCash.Content = "Total Cash: + " + total;
            }

        }



        private void ClearTextChildren(TextBox container)
        {
            container.Text = string.Empty;

        }
        private void ClearDropDownChildren(ComboBox container)
        {
            container.SelectedIndex = -1;

        }

        private void ClearDropDate(DatePicker container)
        {
            container.SelectedDate = null;
        }

        void WelcomeUser()
        {
            NameLabel.Content = "Welcome " + Login.FirstName;

        }

        private void DeletePaymentButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void MoneyInRB_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckIfExpensesHasBeenEntered()
        {

            if (string.IsNullOrEmpty(ItemNameTextBox.Text))
            {
                MessageBox.Show("Please Enter The Name Of Your Payment");
                isDetailsCorrect = false;
            }
            else if (ItemTypeDropDownList.SelectedIndex == -1)
            {
                MessageBox.Show("Please Enter Your Expense Type");
                isDetailsCorrect = false;
            }
            else if (DateTextBox.SelectedDate == null)
            {
                MessageBox.Show("Please Enter The Date Of Your Payment");
                isDetailsCorrect = false;
            }
            else if (string.IsNullOrEmpty(AmountOfMoneyTextBox.Text))
            {
                MessageBox.Show("Please Enter The Type Of Expense");
                isDetailsCorrect = false;

            }
            else if (Regex.IsMatch(AmountOfMoneyTextBox.Text, "[a-zA-Z]"))
            {
                MessageBox.Show("Please Enter The Cost Of Expense");
                isDetailsCorrect = false;

            }
            else if (MoneyInRB.IsChecked == false && MoneyOutRB.IsChecked == false)
            {
                MessageBox.Show("Please Select Whether The Cost Is Money In Or Money Out");
                isDetailsCorrect = false;

            }
            else
            {
                isDetailsCorrect = true;

            }

        }



        private void GoBackSearch_Click(object sender, RoutedEventArgs e)
        {
            SearchForUser.Text = "";
            LoadData();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
