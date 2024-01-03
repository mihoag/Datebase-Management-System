using HospitalManagement.model;
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

namespace HospitalManagement.View.DentistSide
{
    /// <summary>
    /// Interaction logic for keDon.xaml
    /// </summary>
    public partial class keDon : Window
    {
        public DonThuoc dt { get; set; }
        public keDon(string id_BN, string ngaykham)
        {
            dt = new DonThuoc();
            dt.id_bn = id_BN;
            if (DateTime.TryParse(ngaykham, out DateTime selectedDate))
            {
                string NGAYKHAM = selectedDate.ToString("yyyy-MM-dd");
                dt.ngaykham = NGAYKHAM;
            }
            InitializeComponent();
        }
        private void MainLoad(object sender, RoutedEventArgs e)
        {
           
            this.DataContext = dt;
        }

        private void back(object sender, RoutedEventArgs e)
        {
            DetailPatientRecord record = new DetailPatientRecord(dt.id_bn);
            record.Show();
            this.Close();
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            try
            {
        
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_KEDON", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_BN", dt.id_bn );
                        cmd.Parameters.AddWithValue("@ID_THUOC", dt.id_thuoc );
                        cmd.Parameters.AddWithValue("@NGAYKHAM", dt.ngaykham);
                        cmd.Parameters.AddWithValue("@SOLUONG", dt.soluong);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Ke don thuoc thanh cong");

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
