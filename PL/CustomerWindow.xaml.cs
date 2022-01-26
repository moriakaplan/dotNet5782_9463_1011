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
using BLApi;
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        IBL blObject;
        /// <summary>
        /// constractor for update the customer data
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="cusId"></param>
        public CustomerWindow(IBL obj, int cusId) 
        {
            blObject = obj;
            InitializeComponent();
            Customer cus;
            try { cus = blObject.GetCustomer(cusId); }
            catch (NotExistIDException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            DataContext = cus;
            if (cus.ParcelsFrom.Count() == 0 && cus.ParcelsTo.Count() == 0)//if the customer have no parcel- to or from
                parcels.Visibility = Visibility.Collapsed;
            else
            {
                lstParcelsFrom.DataContext = cus.ParcelsFrom.Select(x => x.Id);
                lstParcelsTo.DataContext = cus.ParcelsTo.Select(x => x.Id);
            }

            txtId.IsEnabled = false;
            txtLatti.IsEnabled = false;
            txtLongi.IsEnabled = false;
            txtLongi.IsEnabled = false;

            options.Content = "Update Customer Data";
            options.Click -= addCustomer;
            options.Click += updateCustomer;

        }
        /// <summary>
        /// constractor for adding new customer
        /// </summary>
        /// <param name="obj"></param>
        public CustomerWindow(IBL obj) 
        {
            blObject = obj;
            InitializeComponent();

            ParcelsFrom.Visibility = Visibility.Collapsed;
            ParcelsTo.Visibility = Visibility.Collapsed;
            lstParcelsFrom.Visibility = Visibility.Collapsed;
            lstParcelsTo.Visibility = Visibility.Collapsed;
            txtId.Text = "9 digits";
            options.Content = "Add Customer";
            options.Click -= updateCustomer;
            options.Click += addCustomer;
        }

        /// <summary>
        /// By double-clicking on a row in the table of parcels to the customer a specific parcel window opens 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewParcelTo(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(blObject, ((int)lstParcelsTo.SelectedItem)).ShowDialog();
        }

        /// <summary>
        /// By double-clicking on a row in the table of parcels from the customer a specific parcel window opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewParcelFrom(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(blObject, ((int)lstParcelsFrom.SelectedItem)).ShowDialog();
        }

        private void addCustomer(object sender, RoutedEventArgs e)
        {
            int id;
            if (int.TryParse(txtId.Text, out id) == false)//if the id is not good
            {
                MessageBox.Show("the id is not a valid number, please try again\n");
                return;
            }
            if (id < 100000000 || id > 999999999)//if the id is more or less then 9 digits
            {
                MessageBox.Show("the id suppose to be a number with 9 digits, please choose another id and try again\n");
                return;
            }
            if (txtName.Text == null)//if there is no name
            {
                MessageBox.Show("please enter a name\n");
                return;
            }
            if (!phoneIsOk())//if the phone is not good
            {
                MessageBox.Show("please enter a valid phone\n");
                return;
            }
            try
            {
                blObject.AddCustomer(id, txtName.Text, txtPhone.Text, new Location { Latti = double.Parse(txtLatti.Text), Longi = double.Parse(txtLongi.Text) });
                MessageBox.Show("The customer added successfully");
                this.Close();
            }
            catch (ExistIdException)
            {
                MessageBox.Show("this id already exist, please choose another one and try again\n");
                txtId.Foreground = Brushes.Red;
            }
        }

        private void updateCustomer(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == null)//if there is no name
            {
                MessageBox.Show("please enter a name\n");
                return;
            }
            if (!phoneIsOk())//if the phone is not good
            {
                MessageBox.Show("the number of charge slots is not a valid number, please try again\n");
                return;
            }
            blObject.UpdateStation(int.Parse(txtId.Text), txtName.Text, int.Parse(txtPhone.Text));
        }

        /// <summary>
        /// check if the phone number is good
        /// </summary>
        /// <returns></returns>
        private bool phoneIsOk()
        {
            int phone;
            if (int.TryParse(txtPhone.Text, out phone) == false || phone<0 || txtPhone.Text.Length!=10) return false;
            return true;
        }

        /// <summary>
        /// if the id is good- the color is green. if not- red.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void idColor(object sender, TextChangedEventArgs e)
        {
            int id;
            if (txtId.IsEnabled == false) return;
            if (int.TryParse(txtId.Text, out id) && id >= 100000000 && id <= 999999999)
            {
                txtId.Foreground = Brushes.Green;
            }
            else
            {
                if (txtId.Text == "9 digits") txtId.Foreground = Brushes.Black;
                else txtId.Foreground = Brushes.Red;
            }
        }

        /// <summary>
        /// if the phone number is good- the color is green. if not- red
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void phoneColor(object sender, TextChangedEventArgs e)
        {
            if (phoneIsOk())
            {
                txtPhone.Foreground = Brushes.Green;
            }
            else
            {
                if (txtPhone.Text == "format") txtPhone.Foreground = Brushes.Black;
                else txtPhone.Foreground = Brushes.Red;
            }
        }
    }
}
