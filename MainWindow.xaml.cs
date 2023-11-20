using HospitalManagement.View;
using HospitalManagement.View.EmployeeSide;
using HospitalManagement.View.PatientSide;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HospitalManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string role = "nhan vien";
        private void login(object sender, RoutedEventArgs e)
        {
          
            if (role == "benh nhan")
            {
                HomePatient homePatient = new HomePatient();
                homePatient.Show();
            }
            else if (role == "quan tri vien")
            {

            }
            else if (role == "nhan vien")
            {
                HomeEmployee homeEmployee = new HomeEmployee();
                homeEmployee.Show();
            }
            else if (role == "nha si")
            {

            }
            else return;

            this.Close();
        }

        private void register(object sender, RoutedEventArgs e)
        {
            Register registerWD = new Register();
            registerWD.Show();
            this.Close();
        }
    }
}
