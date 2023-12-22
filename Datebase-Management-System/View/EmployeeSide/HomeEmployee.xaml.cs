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

namespace HospitalManagement.View.EmployeeSide
{
    /// <summary>
    /// Interaction logic for HomeEmployee.xaml
    /// </summary>
    public partial class HomeEmployee : Window
    {
        public HomeEmployee()
        {
            InitializeComponent();
        }

        private void logout(object sender, RoutedEventArgs e)
        {
            MainWindow mainWD = new MainWindow();
            mainWD.Show();
            this.Close();
        }

        private void setSchedule(object sender, RoutedEventArgs e)
        {
            ListFreeTimeDentist listFreeTimeDentistWD = new ListFreeTimeDentist();
            listFreeTimeDentistWD.Show();   
            this.Close();
        }

     

        private void patientRecord(object sender, RoutedEventArgs e)
        {
            patientRecord patientRecord = new patientRecord();
            patientRecord.Show();   
            this.Close();
        }


        private void showMedicine(object sender, RoutedEventArgs e)
        {
            showMedicine showMedicineWD = new showMedicine();
            showMedicineWD.Show();
            this.Close();
        }

       
    }
}
