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

namespace HospitalManagement.View.DentistSide
{
    /// <summary>
    /// Interaction logic for HomeDentist.xaml
    /// </summary>
    public partial class HomeDentist : Window
    {
        public static string ID_dentist;
        public HomeDentist(string ID)
        {
            ID_dentist = ID;
            InitializeComponent();
        }

        private void checkSchedule(object sender, RoutedEventArgs e)
        {
            DentistSchedule dentistSchedule = new DentistSchedule();
            dentistSchedule.Show();
            this.Close();
        }

        private void personalInfo(object sender, RoutedEventArgs e)
        {
            PersonalInformation personalInformation = new PersonalInformation();
            personalInformation.Show();
            this.Close();
        }

        private void patientRecord(object sender, RoutedEventArgs e)
        {
            patientRecord pr = new patientRecord();
            pr.Show();
            this.Close();
        }

        private void showMedicine(object sender, RoutedEventArgs e)
        {
            showMedicine sm = new showMedicine();
            sm.Show();
            this.Close();
        }
        private void logout(object sender, RoutedEventArgs e)
        {
            MainWindow mainWD = new MainWindow();
            mainWD.Show();
            this.Close();
        }

        private void checkScheduleCommitted(object sender, RoutedEventArgs e)
        {
            DentistScheduleCommitted dentistScheduleCommitted = new DentistScheduleCommitted();
            dentistScheduleCommitted.Show();
            this.Close();
        }
    }
}
