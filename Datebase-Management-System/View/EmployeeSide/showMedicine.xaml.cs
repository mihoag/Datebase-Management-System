using HospitalManagement.model;
using HospitalManagement.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for showMedicine.xaml
    /// </summary>
    public partial class showMedicine : Window
    {
        public showMedicine()
        {
            InitializeComponent();
        }

        BindingList<Medicine> listMedicine = null;


        private void back(object sender, RoutedEventArgs e)
        {
            HomeEmployee  homeEmployee = new HomeEmployee(HomeEmployee.id_nv);
            homeEmployee.Show();
            this.Close();
        }

        private void MainLoad(object sender, RoutedEventArgs e)
        {
            listMedicine = new BindingList<Medicine>();
            LoadAllMedicine();
            ComboboxMedicine.ItemsSource = listMedicine;
        }
        private void LoadAllMedicine()
        {
            listMedicine.Clear();
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_xemthuoc", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Access the columns in the result set
                                    string ID_Thuoc = reader["ID_THUOC"].ToString();
                                    string tenthuoc = reader["TENTHUOC"].ToString();
                                    string donvitinh = reader["DONVITINH"].ToString();
                                    string chidinh = reader["CHIDINH"].ToString();
                                    int tonkho = (int)reader["TONKHO"];
                                    string ngayhethan = reader["NGAYHETHAN"].ToString();
                                    int dongia = (int)reader["DONGIA"];
                                    string ID_QTV = reader["ID_QTV"].ToString();

                                    Medicine medicine = new Medicine() { ID_THUOC = ID_Thuoc,
                                        TEN_THUOC = tenthuoc, DONVITINH = donvitinh, CHI_DINH = chidinh, TONKHO = tonkho,
                                        NGAY_HH = ngayhethan, DONGIA = dongia, ID_QTV = ID_QTV
                                    };
                                    listMedicine.Add(medicine);
                                    // String lichhen  = reader["ID_reader["NGAYKHAM"]LICHHEN"].ToString();
                                    // String idNhasi = reader["ID_NS"].ToString();
                                    // String idKH = reader["ID_KH"].ToString();
                                    // String idNV = reader["ID_NV"].ToString();
                                    // String ngayhen = reader["NGAYHEN"].ToString();
                                    //  String gioBd = reader[""].ToString();
                                    //  String gioKt = reader["ID_NS"].ToString();
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

        private void search(object sender, KeyEventArgs e)
        {
            listMedicine.Clear();
            string keyword = keyWord.Text;
            if(keyword == "")
            {
                LoadAllMedicine();  
            }
            else
            {
                try
                {
                    using (SqlConnection connection = DB.Instance.Connection)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_timthuoctheoten", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@tenthuoc", keyword);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Check if the reader has rows
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        // Access the columns in the result set
                                        string ID_Thuoc = reader["ID_THUOC"].ToString();
                                        string tenthuoc = reader["TENTHUOC"].ToString();
                                        string donvitinh = reader["DONVITINH"].ToString();
                                        string chidinh = reader["DONVITINH"].ToString();
                                        int tonkho = (int)reader["TONKHO"];
                                        string ngayhethan = reader["NGAYHETHAN"].ToString();
                                        int dongia = (int)reader["DONGIA"];
                                        string ID_QTV = reader["ID_QTV"].ToString();

                                        Medicine medicine = new Medicine()
                                        {
                                            ID_THUOC = ID_Thuoc,
                                            TEN_THUOC = tenthuoc,
                                            DONVITINH = donvitinh,
                                            CHI_DINH = chidinh,
                                            TONKHO = tonkho,
                                            NGAY_HH = ngayhethan,
                                            DONGIA = dongia,
                                            ID_QTV = ID_QTV
                                        };
                                        listMedicine.Add(medicine);
                                        
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
}
