﻿using System;
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
    /// Interaction logic for ViewSchedulePage.xaml
    /// </summary>
    public partial class ViewSchedulePage : Window
    {
        public ViewSchedulePage()
        {
            InitializeComponent();
        }

        private void btnBack(object sender, RoutedEventArgs e)
        {
            HomePatient homePatient = new HomePatient();
            homePatient.Show();
            this.Close();
        }
    }
}
