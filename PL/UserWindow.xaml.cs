using BLApi;
using BO;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        IBL blObject;
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="customerId"></param>
        public UserWindow(IBL obj, int customerId)
        {
            InitializeComponent();
            blObject = obj;
            Customer customer = blObject.GetCustomer(customerId);
            DataContext = customer;
        }

        /// <summary>
        /// send new parcel (that the user is the sender)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendNewParcel(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blObject, int.Parse(txtId.Content.ToString()), true).ShowDialog();
            refresh();
        }

        /// <summary>
        /// refresh the window
        /// </summary>
        private void refresh()
        {
            DataContext = blObject.GetCustomer((DataContext as Customer).Id);
        }

        /// <summary>
        /// update the customer data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateCustomer(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blObject, int.Parse(txtId.Content.ToString())).ShowDialog();
        }

        /// <summary>
        /// go back to the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backToMain(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
        /// <summary>
        /// By double-clicking on a row in the table of parcels to the customer a specific parcel window opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewParcelTo(object sender, MouseButtonEventArgs e)
        {

            new ParcelWindow(blObject, ((BO.ParcelInCustomer)ParcelsTo.SelectedItem).Id).ShowDialog();
            refresh();
        }
        /// <summary>
        /// By double-clicking on a row in the table of parcels from the customer a specific parcel window opens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewParcelFrom(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(blObject, ((BO.ParcelInCustomer)ParcelsFrom.SelectedItem).Id).ShowDialog();
        }

    }
}
