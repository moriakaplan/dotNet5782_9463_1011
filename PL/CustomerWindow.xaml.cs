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
        public CustomerWindow(IBL obj, int cusId)
        {
            blObject = obj;
            InitializeComponent();

            Customer cus = blObject.DisplayCustomer(cusId);
            txtId.Text = cus.Id.ToString();
            txtName.Text = cus.Name;
            txtLatti.Text = cus.Location.Latti.ToString();
            txtLongi.Text = cus.Location.Longi.ToString();
            txtPhone.Text = cus.Phone;
            txtParcelFrom.ItemsSource = cus.parcelFrom.Select(x => x.Id);
            txtParcelTo.ItemsSource = cus.parcelTo.Select(x => x.Id);
        }
    }
}
