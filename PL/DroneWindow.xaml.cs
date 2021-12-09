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
using IBL;
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDrone.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private Ibl blObject;
        public DroneWindow(Ibl obj)
        {
            InitializeComponent();
            AddDroneOption.Visibility = Visibility.Visible;
            blObject = obj;
            txtStatus.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            txtWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }
    }
}
