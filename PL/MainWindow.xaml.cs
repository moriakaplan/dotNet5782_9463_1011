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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal readonly IBL blObject = BLFactory.GetBl(); //צריך לדאוג שיהיה שדה של המחלקה

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LogManager(object sender, RoutedEventArgs e)
        {
            //new ManagerWindow(blObject).Show();//צריך להוסיף פרמטרים מתאימים
            new PasswordWindow(blObject, true).Show();//צריך להוסיף פרמטרים מתאימים
        }

        private void LogUser(object sender, RoutedEventArgs e)
        {
            new PasswordWindow(blObject, false).Show();//צריך להוסיף פרמטרים מתאימים
        }

        private void Sign(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mb= MessageBox.Show("are you a manager or worker of the company?", "sighn up", MessageBoxButton.YesNoCancel);
            if(mb==MessageBoxResult.No) new SignWindow(blObject, false).Show();//צריך להוסיף פרמטרים מתאימים
            if(mb==MessageBoxResult.Yes) new SignWindow(blObject, true).Show();//צריך להוסיף פרמטרים מתאימים
        }
    }
}