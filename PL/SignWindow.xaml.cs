using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        enum typesOfUsers { RegularUser, Manager };
        public SignWindow(IBL obj)
        {
            InitializeComponent();
            blObject = obj;
            typeOfUser.ItemsSource = Enum.GetValues(typeof(typesOfUsers));
            typeOfUser.SelectedItem = typesOfUsers.RegularUser;
        }

        private void Sighn(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((typesOfUsers)typeOfUser.SelectedItem == typesOfUsers.Manager)
                    blObject.AddManager(txtName.Text, txtPassword.Text);
                else
                    blObject.AddUser(int.Parse(txtId.Text), txtName.Text, txtPassword.Text);
                MessageBox.Show("the user added successfully");
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void typeOfUserChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((typesOfUsers)typeOfUser.SelectedItem==typesOfUsers.Manager)
            {
                txtManager.Visibility = Visibility.Visible;
                lblManager.Visibility = Visibility.Visible;
                txtId.Visibility = Visibility.Collapsed;
                lblId.Visibility = Visibility.Collapsed;
                help.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtId.Visibility = Visibility.Visible;
                lblId.Visibility = Visibility.Visible;
                help.Visibility = Visibility.Visible;
                txtManager.Visibility = Visibility.Collapsed;
                lblManager.Visibility = Visibility.Collapsed;
            }
        }
        void DataWindow_Closing(object sender, CancelEventArgs e)
        {
            //MessageBoxResult mb;
            //mb = MessageBox.Show("do you want to close the window?", "close", MessageBoxButton.YesNo);
            //if (mb == MessageBoxResult.No) e.Cancel=true;
             new MainWindow().Show();
        }
    }
}
