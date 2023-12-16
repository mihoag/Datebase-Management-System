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

namespace HospitalManagement.View
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void register(object sender, RoutedEventArgs e)
        {
            // dang ki thanh cong quay lai trang dang nhap
            MainWindow mainWD = new MainWindow();   
            mainWD.Show();  
            this.Close();
        }

        private void login(object sender, RoutedEventArgs e)
        {
            MainWindow mainWD = new MainWindow();
            mainWD.Show();
            this.Close();
        }
    }
}
