using HospitalManagement.model;
using Microsoft.Data.SqlClient;
using HospitalManagement.Utils;
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
using System.Data;

namespace HospitalManagement.View.EmployeeSide
{
    /// <summary>
    /// Interaction logic for deleteBill.xaml
    /// </summary>
    public partial class deleteBill : Window
    {
        public deleteBill()
        {
            InitializeComponent();
        }

        private void delete_btn(object sender, RoutedEventArgs e)
        {
            // Lấy mã hoá đơn từ TextBox
            string idHoaDon = IDMtb.Text.Trim();

            if (string.IsNullOrEmpty(idHoaDon))
            {
                MessageBox.Show("Vui lòng nhập mã hoá đơn.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // Kết nối đến cơ sở dữ liệu
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    // Tạo SqlCommand để gọi stored procedure
                    using (SqlCommand cmd = new SqlCommand("sp_XOAHOADON", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số cho stored procedure
                        cmd.Parameters.AddWithValue("@id_hoadon", idHoaDon);

                        // Thực thi stored procedure
                        int result = cmd.ExecuteNonQuery();

                        MessageBox.Show("Xoá hoá đơn thành công");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            HomeEmployee home = new HomeEmployee(HomeEmployee.id_nv);
            home.Show();
            this.Close();
        }
    }
}
