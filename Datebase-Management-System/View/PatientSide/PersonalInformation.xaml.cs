using HospitalManagement.model;
using HospitalManagement.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
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
    /// Interaction logic for PersonalInformation.xaml
    /// </summary>
    public partial class PersonalInformation : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public User user { get; set; }= new User();
        private string sdt;
        public PersonalInformation()
        {
            InitializeComponent();
            sdt = "0123456789";
            Loaded += PersonalInformation_Loaded;
        }


        private void PersonalInformation_Loaded(object sender, RoutedEventArgs e)
        {
            // load information
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_thongTinKhachHang", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SDT", "0123456789");
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Access the columns in the result set
                                    user.DIENTHOAI = reader["DIENTHOAI"].ToString();
                                    user.HOTEN = reader["HOTEN"].ToString();
                                    user.NGAYSINH = reader["NGAYSINH"].ToString().Split(" ")[0];
                                    user.DIACHI = reader["DIACHI"].ToString();
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

        private void btnUpdate(object sender, RoutedEventArgs e)
        {

            //HomePatient homePatient = new HomePatient();
            //homePatient.Show();
            //this.Close();
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_CapNhatThongTin", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DIENTHOAI", user.DIENTHOAI);
                        cmd.Parameters.AddWithValue("@HOTEN", user.HOTEN);
                        cmd.Parameters.AddWithValue("@NGAYSINH", user.NGAYSINH);
                        cmd.Parameters.AddWithValue("@DIACHI", user.DIACHI);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {

                                }
                            }
                        }
                    }

                }

                MessageBox.Show("Cập nhật thông tin thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnBack(object sender, RoutedEventArgs e)
        {
            HomePatient homePatient = new HomePatient(HomePatient.id_patient);
            homePatient.Show();
            this.Close();
        }
    }
}
