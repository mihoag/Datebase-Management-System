using HospitalManagement.Utils;
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
    /// Interaction logic for addMedicine.xaml
    /// </summary>
    public partial class addMedicine : Window
    {
        public addMedicine()
        {
            InitializeComponent();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            MedicinePage page = new MedicinePage();
            page.Show();
            this.Close();
        }

        private void addMedidice(object sender, RoutedEventArgs e)
        {
            try
            {
                // Lấy giá trị từ các TextBox và DatePicker
                string tenThuoc = tenthuoc.Text.Trim();
                string donViTinh = donvitinh.Text.Trim();
                string chiDinh = chidinh.Text.Trim();
                int tonKho = int.Parse(tonkho.Text.Trim());
                DateTime ngayHetHan = ngayhethan.SelectedDate ?? DateTime.Now;
                int donGia = int.Parse(dongia.Text.Trim());
                string idQTV = id_qtv.Text.Trim();

                // Kết nối đến cơ sở dữ liệu
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    // Tạo SqlCommand để gọi stored procedure
                    using (SqlCommand cmd = new SqlCommand("sp_THEMTHUOC", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Thêm tham số cho stored procedure
                        cmd.Parameters.AddWithValue("@tenthuoc", tenThuoc);
                        cmd.Parameters.AddWithValue("@donvitinh", donViTinh);
                        cmd.Parameters.AddWithValue("@chidinh", chiDinh);
                        cmd.Parameters.AddWithValue("@tonkho", tonKho);
                        cmd.Parameters.AddWithValue("@ngayhethan", ngayHetHan);
                        cmd.Parameters.AddWithValue("@dongia", donGia);
                        cmd.Parameters.AddWithValue("@id_qtv", idQTV);

                        // Thực thi stored procedure
                        int result = cmd.ExecuteNonQuery();

                        MessageBox.Show("Thêm thuốc thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
