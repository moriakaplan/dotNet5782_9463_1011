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
            //txtId.Text = customerId.ToString();
            Customer customer = blObject.GetCustomer(customerId);
            DataContext = customer;
            //txtname.Text = customer.Name;
            //txtParcelsFrom.DataContext = cus.parcelFrom.Select(x => x.Id);
            //txtParcelsTo.DataContext = cus.parcelTo.Select(x => x.Id);
        }

        private void sendNewParcel(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blObject, int.Parse(txtId.Content.ToString()), true).ShowDialog();
            refresh();
        }

        private void refresh()
        {
            DataContext = blObject.GetCustomer((DataContext as Customer).Id);
        }

        private void updateCustomer(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blObject, int.Parse(txtId.Content.ToString())).ShowDialog() ;
        }

        private void BackToMain(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void ViewParcelTo(object sender, MouseButtonEventArgs e)
        {
            
            new ParcelWindow(blObject, ((BO.ParcelInCustomer)ParcelsTo.SelectedItem).Id).ShowDialog();
            refresh();
        }
        private void ViewParcelFrom(object sender, MouseButtonEventArgs e)
        {
            new ParcelWindow(blObject, ((BO.ParcelInCustomer)ParcelsFrom.SelectedItem).Id).ShowDialog();
        }
       
    }
}
