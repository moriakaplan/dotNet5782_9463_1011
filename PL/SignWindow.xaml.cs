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
            if(txtName.Text=="")
            {
                MessageBox.Show("Please choose a username");
                return;
            }
            if(txtPassword.Text=="")
            {
                MessageBox.Show("Please choose a password");
                return;
            }
            if(!passIsStrong(txtPassword.Text))
            {
                MessageBox.Show("this password is not strong enugh, please choose another one.\n in the password must be big letters, small letters and numbers and it must have at least 8 chars.");
                return;
            }
            try
            {
                if ((typesOfUsers)typeOfUser.SelectedItem == typesOfUsers.Manager)
                {
                    if (txtManager.Text == blObject.getManagmentPassword())
                        blObject.AddManager(txtName.Text, txtPassword.Text);
                    else
                        MessageBox.Show("this is not the right password, \n please ask another manager what is the current password.");
                }
                else
                    blObject.AddUser(int.Parse(txtId.Text), txtName.Text, txtPassword.Text);
                MessageBox.Show("the user added successfully");
                this.Close();
            }
            catch(ExistIdException)
            {
                MessageBox.Show("the username already exist, try to choose another one");
            }
        }

        private bool passIsStrong(string pass)
        {
            if (pass.Length < 8 || 
                pass.Distinct().Count() < 6) 
                return false;
            if (pass.Any(x=> x >= 'a' && x <= 'z') && 
                pass.Any(x => x >= 'A' && x <= 'Z') && 
                pass.Any(x => x >= '0' && x <= '9')) 
                return true;
            return false;
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

        private void passChange(object sender, TextChangedEventArgs e)
        {
            if (txtPassword.Text == "")
            {
                lblGoodOrWrong.Content = "";
                return;
            }
            if (passIsStrong(txtPassword.Text))
            {
                lblGoodOrWrong.Content = "strong password";
                lblGoodOrWrong.Foreground = Brushes.Green;
            }
            else
            {
                lblGoodOrWrong.Content = $"not strong password\nThe password must be at least 8 chars (6 different) and with big letters, small letters and numbers.";
                lblGoodOrWrong.Foreground = Brushes.Red;
            }
        }
    }
}
