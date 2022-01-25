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
        public UserWindow(IBL obj, int customerId)
        {
            InitializeComponent();
            blObject = obj;
            txtId.Text = customerId.ToString();
            Customer customer = blObject.GetCustomer(customerId);
            txtname.Text = customer.Name;
            txtId.IsEnabled = false;
            txtname.IsEnabled = false;
            //txtParcelsFrom.DataContext = cus.parcelFrom.Select(x => x.Id);
            //txtParcelsTo.DataContext = cus.parcelTo.Select(x => x.Id);
        }

        private void sendNewParcel(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blObject, int.Parse(txtId.Text), true).ShowDialog();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void updateCustomer(object sender, RoutedEventArgs e)
        {

        }

        private void BackToMain(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
