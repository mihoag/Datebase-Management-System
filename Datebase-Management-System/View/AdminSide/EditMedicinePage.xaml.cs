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

namespace HospitalManagement.View.AdminSide
{
    /// <summary>
    /// Interaction logic for EditMedicinePage.xaml
    /// </summary>
    public partial class EditMedicinePage : Window
    {
        private bool fixErr = false;
        private string _id_thuoc;
        private Medicine detailMedicine =new Medicine();
        public EditMedicinePage(string id_thuoc)
        {
            InitializeComponent();
            _id_thuoc = id_thuoc;
            Loaded += EditMedicinePage_Loaded;
        }

        private void EditMedicinePage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_chitietThuoc", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_THUOC", _id_thuoc);
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

                                    detailMedicine = new Medicine()
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

            IDMtb.Text = detailMedicine.ID_THUOC;
            Nametb.Text = detailMedicine.TEN_THUOC;
            soluongTb.Text = detailMedicine.TONKHO.ToString();
            dvtTb.Text = detailMedicine.DONVITINH;

        }

        private void save_click(object sender, RoutedEventArgs e)
        {
            int slton = Int32.Parse(soluongTb.Text);
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    string procName = "USP_UPDATE_SL_TON_THUOC";
                    if (fixErr)
                    {
                        procName = "USP_UPDATE_SL_TON_THUOC_FIX";
                    }
                    using (SqlCommand cmd = new SqlCommand(procName, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID_THUOC", _id_thuoc);
                        cmd.Parameters.AddWithValue("@TONKHO", slton);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Cập nhật số lượng thuốc thành công");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            MedicinePage page = new MedicinePage();
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
