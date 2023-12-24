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

namespace HospitalManagement.View.AdminSide
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Window
    {
        public HomePage()
        {
            InitializeComponent();
        }


        private void AddAccount_click(object sender, RoutedEventArgs e)
        {
            addAcountPage page = new addAcountPage();
            page.Show();
            this.Close();
        }

        private void logout(object sender, RoutedEventArgs e)
        {
            MainWindow mainWD = new MainWindow();
            mainWD.Show();
            this.Close();
        }

        private void accountHandler_Click(object sender, RoutedEventArgs e)
        {
            AccountHandlerPage page = new AccountHandlerPage();
            page.Show();
            this.Close();
        }
    }
}
