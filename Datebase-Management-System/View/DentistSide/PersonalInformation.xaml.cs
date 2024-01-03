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
    /// Interaction logic for PersonalInformation.xaml
    /// </summary>
    public partial class PersonalInformation : Window
    {
        public PersonalInformation()
        {
            InitializeComponent();
        }

        private void btnUpdate(object sender, RoutedEventArgs e)
        {

        }

        private void btnBack(object sender, RoutedEventArgs e)
        {
            HomeDentist homeDentist = new HomeDentist(HomeDentist.ID_dentist);
            homeDentist.Show();
            this.Close();
        }
    }
}
