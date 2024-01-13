using HospitalManagement.model;
using HospitalManagement.Utils;
using Microsoft.Data.SqlClient;
using System.Data;
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
using System.ComponentModel;

namespace HospitalManagement.View.AdminSide
{
    /// <summary>
    /// Interaction logic for showBill.xaml
    /// </summary>
    public partial class showBill : Window
    {
        public bool FixError { get; set; }
        public showBill()
        {
            InitializeComponent();
        }

        BindingList<bill> listBill = null;
        private void back(object sender, RoutedEventArgs e)
        {
            HomePage HomePage = new HomePage();
            HomePage.Show();
            this.Close();
        }

        private void MainLoad(object sender, RoutedEventArgs e)
        {
            listBill = new BindingList<bill>();
            LoadAllBill();
            ComboboxBill.ItemsSource = listBill;
        }

        private void LoadAllBill()
        {
            listBill.Clear();
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    string procName = "sp_XEMHOADON";
                    if (FixError)
                    {
                        procName = "sp_XEMHOADON_fix";
                    }
                    using (SqlCommand cmd = new SqlCommand(procName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                reader.Read();
                                int tongSoHD = (int)reader["TONG_SO_HD"];
                                lblSoLuong.Content = tongSoHD.ToString();
                                reader.NextResult();
                                while (reader.Read())
                                {
                                    // Access the columns in the result set
                                    string ID_HOADON = reader["ID_HOADON"].ToString();
                                    string ID_BN = reader["ID_BN"].ToString();
                                    string ngaykham = reader["NGAYKHAM"].ToString();
                                    int phikham = (int)reader["PHIKHAM"];
                                    int thanhtien = (int)reader["THANHTIEN"];
                                    string ID_NV = reader["ID_NV"].ToString();


                                    bill bill = new bill()
                                    {
                                        id_hd = ID_HOADON,
                                        id_bn = ID_BN,
                                        ngaykham = ngaykham,
                                        phikham = phikham,
                                        thanhtien = thanhtien,
                                        id_nv = ID_NV,
                                    };
                                    listBill.Add(bill);
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
    }
}
