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
    /// Interaction logic for setbill.xaml
    /// </summary>
    public partial class setbill : Window
    {
        public setbill()
        {
            InitializeComponent();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            DetailPatientRecord record = new DetailPatientRecord();
            record.Show();
            this.Close();
        }

        private void Export(object sender, RoutedEventArgs e)
        {

        }
    }
}
