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

namespace HospitalManagement.View.EmployeeSide
{
    /// <summary>
    /// Interaction logic for setbill.xaml
    /// </summary>
    public partial class setbill : Window
    {
        public bill b { get; set; }
        public setbill(string id_BN, string ngaykham)
        {
            b = new bill();
            b.id_bn = id_BN;    
            b.ngaykham  = ngaykham; 
            InitializeComponent();
        }
        private void MainLoad(object sender, RoutedEventArgs e)
        {
           
            this.DataContext = b;
        }

        private void back(object sender, RoutedEventArgs e)
        {
            DetailPatientRecord record = new DetailPatientRecord(b.id_bn);
            record.Show();
             this.Close();
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            try
            {
        
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_laphoadon", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_bn", b.id_bn );
                        cmd.Parameters.AddWithValue("@ngaykham", b.ngaykham);
                        cmd.Parameters.AddWithValue("@phikham", b.phikham);
                        cmd.Parameters.AddWithValue("@id_nv", b.id_nv);
                        cmd.Parameters.AddWithValue("@id_kh", b.id_kh);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Lap hoa don thanh cong");

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
