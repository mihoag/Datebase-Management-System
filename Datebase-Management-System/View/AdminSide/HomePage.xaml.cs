using HospitalManagement.Utils;
using HospitalManagement.View.EmployeeSide;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
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
        private bool fixErr = false;
        public HomePage()
        {
            InitializeComponent();
            Loaded += HomePage_Loaded;
        }

        private void HomePage_Loaded(object sender, RoutedEventArgs e)
        {
            var id_admin = ConfigurationManager.AppSettings["id"];
            idLB.Content = "ID: " + id_admin?.ToString();
            
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getNameAdmin", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_ADMIN", id_admin);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    nameLB.Content = "Họ và tên: " + reader["HOTEN"].ToString();
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

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

        private void medicine_Btn(object sender, RoutedEventArgs e)
        {
            MedicinePage page = new MedicinePage();
            page.Show();
            this.Close();
        }

        private void bill_Btn(object sender, RoutedEventArgs e)
        {
            showBill page = new showBill();
            page.FixError = fixErr;
            page.Show();
            this.Close();
        }

        private void fix_click(object sender, RoutedEventArgs e)
        {
            fixErr = !fixErr;
            if (fixErr)
            {
                errorBtn.Content = "Tắt fix lỗi";
            }
            else
            {
                errorBtn.Content = "Bật fix lỗi";
            }
        }
    }
}
