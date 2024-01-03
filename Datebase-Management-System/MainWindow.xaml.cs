using HospitalManagement.model;
using HospitalManagement.Utils;
using HospitalManagement.View;
using HospitalManagement.View.AdminSide;
using HospitalManagement.View.DentistSide;
using HospitalManagement.View.EmployeeSide;
using HospitalManagement.View.PatientSide;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HospitalManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string role = "nha si";
        private void login(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(sdt.Text);
            //MessageBox.Show(pass.Password);
            string sodt = sdt.Text;
            string password = pass.Password as string;
            string dienthoai = "";
            string passW = "";
            string role = "";
            string ID_user = "";

            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("getUser", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@sdt", sodt);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    dienthoai = reader.GetString(reader.GetOrdinal("DIENTHOAI"));
                                    passW = reader.GetString(reader.GetOrdinal("MATKHAU")); ; // reader["MATKHAU"].ToString();
                                    role = reader["VAITRO"].ToString();
                                    ID_user = reader["ID_USER"].ToString();
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
            //MessageBox.Show(password.Length + "");
          //  MessageBox.Show(passW.Length + "");
           // MessageBox.Show(role);
            if (dienthoai == "")
            {
                MessageBox.Show("Số điện thoại không tồn tại trong hệ thống");
                return;
            }
            passW = passW.TrimEnd();
             if(!passW.Equals(password))
            {
                MessageBox.Show("Mật khẩu không đúng");
                return;
            }
            role = role.TrimEnd();
            ID_user = ID_user.TrimEnd();
            if (role == "KHACH HANG")
            {
                HomePatient homePatient = new HomePatient(ID_user);
                homePatient.Show();
            }
            else if (role == "QUAN TRI VIEN")
            {
                HomePage homeAdmin = new HomePage();
                homeAdmin.Show();
            }
            else if (role == "NHAN VIEN")
            {
                HomeEmployee homeEmployee = new HomeEmployee(ID_user);
                homeEmployee.Show();
            }
            else if (role == "NHA SI")
            {
                HomeDentist homeDentist = new HomeDentist(ID_user);
                homeDentist.Show();
            }
            else return;

            this.Close();
        }

        private void register(object sender, RoutedEventArgs e)
        {
            Register registerWD = new Register();
            registerWD.Show();
            this.Close();
        }

     
    }
}
