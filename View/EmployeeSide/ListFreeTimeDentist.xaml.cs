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

namespace HospitalManagement.View.EmployeeSide
{
    /// <summary>
    /// Interaction logic for ListFreeTimeDentist.xaml
    /// </summary>
    public partial class ListFreeTimeDentist : Window
    {
        public ListFreeTimeDentist()
        {
            InitializeComponent();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            HomeEmployee homeEmployee = new HomeEmployee();
            homeEmployee.Show();
            this.Close();   
        }
    }
}
