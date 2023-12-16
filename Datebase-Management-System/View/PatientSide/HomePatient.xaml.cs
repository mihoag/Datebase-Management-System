using HospitalManagement.View.EmployeeSide;
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

namespace HospitalManagement.View.PatientSide
{
    /// <summary>
    /// Interaction logic for HomePatient.xaml
    /// </summary>
    public partial class HomePatient : Window
    {
        public HomePatient()
        {
            InitializeComponent();
        }

        private void btnSetSchedule(object sender, RoutedEventArgs e)
        {
            setDentistSchedule setDentistSchedule = new setDentistSchedule();
            setDentistSchedule.Show();
            this.Close();
        }

        private void btnLogout(object sender, RoutedEventArgs e)
        {
            MainWindow mainWD = new MainWindow();
            mainWD.Show();
            this.Close();
        }

        private void btnViewPatientRecord(object sender, RoutedEventArgs e)
        {
            patientRecordPage patientRecordPage = new patientRecordPage();
            patientRecordPage.Show();
            this.Close();
        }

        private void btnAboutMe(object sender, RoutedEventArgs e)
        {
            PersonalInformation personalInformation = new PersonalInformation();
            personalInformation.Show();
            this.Close();
        }

        private void btnViewSchedule(object sender, RoutedEventArgs e)
        {
            ViewSchedulePage viewSchedulePage = new ViewSchedulePage();
            viewSchedulePage.Show();
            this.Close();
        }
    }
}
