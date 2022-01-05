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

namespace PL
{
    /// <summary>
    /// Interaction logic for SignWindow.xaml
    /// </summary>
    public partial class SignWindow : Window
    {
        IBL blObject;
        bool isManager;
        public SignWindow(IBL obj, bool isManag)
        {
            InitializeComponent();
            blObject = obj;
            isManager = isManag;
            if(isManager)
            {
                txtId.Visibility = Visibility.Collapsed;
                lblId.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtManager.Visibility = Visibility.Collapsed;
                lblManager.Visibility = Visibility.Collapsed;
            }
        }

        private void Sighn(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isManager)
                    blObject.AddManager(txtName.Text, txtPassword.Text);
                else
                    blObject.AddUser(int.Parse(txtId.Text), txtName.Text, txtPassword.Text);
            }
            catch(ExistIdException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
