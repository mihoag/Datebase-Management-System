using HospitalManagement.model;
using HospitalManagement.Utils;
using HospitalManagement.View.EmployeeSide;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for addAcountPage.xaml
    /// </summary>
    public partial class addAcountPage : Window
    {
        public addAcountPage()
        {
            InitializeComponent();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            HomePage page = new HomePage();
            page.Show();
            this.Close();
        }

        private void addAccount_click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem typeItem = (ComboBoxItem)cbRole.SelectedItem;
            string value = typeItem.Content.ToString();
            User newUser = new User()
            {
                DIENTHOAI = SDTtb.Text,
                MATKHAU = MATKHAUtb.Password.ToString(),
                HOTEN = HoTentb.Text,
                VAITRO = value,
            };
            
            // add new user
            try
            {

                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_addNewAccount", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DIENTHOAI", newUser.DIENTHOAI);
                        cmd.Parameters.AddWithValue("@MATKHAU", newUser.MATKHAU);
                        cmd.Parameters.AddWithValue("@HOTEN", newUser.HOTEN);
                        if (newUser.VAITRO == "Nha sĩ")
                        {
                            cmd.Parameters.AddWithValue("@VAITRO", "NHA SI");
                        }else
                        {
                            cmd.Parameters.AddWithValue("@VAITRO", "QUAN TRI VIEN");
                        }

                        cmd.ExecuteNonQuery();

                    }
                }
                MessageBox.Show("Thêm tài khoản thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
