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
        public CustomerWindow(IBL obj, int cusId) //update
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
            //txtId.Text = cus.Id.ToString();
            //txtName.Text = cus.Name;
            //txtLatti.Text = cus.Location.Latti.ToString();
            //txtLongi.Text = cus.Location.Longi.ToString();
            //txtPhone.Text = cus.Phone;
            txtParcelsFrom.DataContext = cus.parcelFrom.Select(x => x.Id);
            txtParcelsTo.DataContext = cus.parcelTo.Select(x => x.Id);

            txtId.IsEnabled = false;
            txtLatti.IsEnabled = false;
            txtLongi.IsEnabled = false;
            txtLongi.IsEnabled = false;

            options.Content = "Update Customer Data";
            options.Click -= AddCustomer;
            options.Click += UpdateCustomer;

        }
        public CustomerWindow(IBL obj) //add
        {
            blObject = obj;
            InitializeComponent();

            ParcelsFrom.Visibility = Visibility.Collapsed;
            ParcelsTo.Visibility = Visibility.Collapsed;
            txtParcelsFrom.Visibility = Visibility.Collapsed;
            txtParcelsTo.Visibility = Visibility.Collapsed;
            txtId.Text = "9 digits";
            //לדאוג שהמספר טלפון יהיה תקין פה ובהמשך
            options.Content = "Add Customer";
            options.Click -= UpdateCustomer;
            options.Click += AddCustomer;
        }



        private void viewParcelTo(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(blObject, ((int)txtParcelsTo.SelectedItem)).ShowDialog();
        }

        private void viewParcelFrom(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(blObject, ((int)txtParcelsFrom.SelectedItem)).ShowDialog();
        }

        private void AddCustomer(object sender, RoutedEventArgs e)
        {
            int id;
            if (int.TryParse(txtId.Text, out id) == false)
            {
                MessageBox.Show("the id is not a valid number, please try again\n");
                return;
            }
            if (id < 100000000 || id > 999999999)
            {
                MessageBox.Show("the id suppose to be a number with 9 digits, please choose another id and try again\n");
                return;
            }
            if (txtName.Text == null)
            {
                MessageBox.Show("please enter a name\n");
                return;
            }
            if (!phoneIsOk())
            {
                MessageBox.Show("please enter a valid phone\n");
                return;
            }
            //לבדוק את הלוקיישן
            try
            {
                blObject.AddCustomer(id, txtName.Text, txtPhone.Text, new Location { Latti = double.Parse(txtLatti.Text), Longi = double.Parse(txtLongi.Text) });
                MessageBox.Show("The customer added successfully");
                //canClose = true;
                this.Close();
            }
            catch (ExistIdException)
            {
                MessageBox.Show("this id already exist, please choose another one and try again\n");
                txtId.Foreground = Brushes.Red;
            }
        }

        private void UpdateCustomer(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == null)
            {
                MessageBox.Show("please enter a name\n");
                return;
            }
            if (!phoneIsOk())
            {
                MessageBox.Show("the number of charge slots is not a valid number, please try again\n");
                return;
            }
            //צריך לעשות משהו מיוחד כדי שהשינויים יהיו אופציונליים?
            blObject.UpdateStation(int.Parse(txtId.Text), txtName.Text, int.Parse(txtPhone.Text));
        }

        bool phoneIsOk()
        {
            int phone;
            if (int.TryParse(txtPhone.Text, out phone) == false || phone<0 || txtPhone.Text.Length!=10) return false;
            //להוסיף בדיקות
            return true;
        }

        private void IdColor(object sender, TextChangedEventArgs e)
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
        private void PhoneColor(object sender, TextChangedEventArgs e)
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
