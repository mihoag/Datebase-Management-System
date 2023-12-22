using HospitalManagement.model;
using HospitalManagement.Utils;
using HospitalManagement.View.EmployeeSide;
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

namespace HospitalManagement.View.PatientSide
{
    /// <summary>
    /// Interaction logic for detailPatientRecord.xaml
    /// </summary>
    public partial class detailPatientRecord : Window
    {
        private patientRecord2 PatientRecord2 = new patientRecord2();
        private BindingList<thuoc> dsthuoc = new BindingList<thuoc>();
        private BindingList<DV> dsDV = new BindingList<DV>();
        private bill bill = new bill();
        public detailPatientRecord(patientRecord2 _patientRecord2)
        {
            InitializeComponent();
            PatientRecord2 = _patientRecord2;
            Loaded += DetailPatientRecord_Loaded;
        }

        private void DetailPatientRecord_Loaded(object sender, RoutedEventArgs e)
        {
            ngaykhamLB.Content = $"Ngày khám: {PatientRecord2.NGAYKHAM}";
            bacSiLB.Content = $"Người khám: {PatientRecord2.HOTEN}";
            tinhTrangLB.Content = $"Tình trạng: {PatientRecord2.TINHTRANG}";
            // lay danh sach thuoc
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_chiTietBenhAn_DV", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_bn", PatientRecord2.ID_BN);
                        cmd.Parameters.AddWithValue("@ngay_kham", PatientRecord2.NGAYKHAM);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    dsDV.Add(new DV()
                                    {
                                        TENDV = reader["TENDV"].ToString(),
                                        SOLUONG = Int32.Parse(reader["SOLUONG"].ToString()),
                                        DONGIA = Int32.Parse(reader["DONGIA"].ToString()),
                                    });
                                }
                            }
                        }
                        ComboboxDetailRecord_DV.ItemsSource = dsDV;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            // lay danh sach thuoc
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_chiTietBenhAn_Thuoc", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_bn", PatientRecord2.ID_BN);
                        cmd.Parameters.AddWithValue("@ngay_kham", PatientRecord2.NGAYKHAM);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    dsthuoc.Add(new thuoc()
                                    {
                                        TENTHUOC = reader["TENTHUOC"].ToString(),
                                        CHIDINH = reader["CHIDINH"].ToString(),
                                        SOLUONG = Int32.Parse(reader["SOLUONG"].ToString()),
                                        DONGIA = Int32.Parse(reader["DONGIA"].ToString())
                                    });
                                }
                            }
                        }
                    }
                    ComboboxDetailRecord_Thuoc.ItemsSource = dsthuoc;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            // lay phi kham va thanh tien
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_chiTietBenhAn_HD", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_bn", PatientRecord2.ID_BN);
                        cmd.Parameters.AddWithValue("@ngay_kham", PatientRecord2.NGAYKHAM);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    bill = new bill()
                                    {
                                        phikham = Int32.Parse(reader["PHIKHAM"].ToString()),
                                        thanhtien = Int32.Parse(reader["THANHTIEN"].ToString())
                                    };
                                }
                            }
                        }
                    }
                    //
                    phikhamLB.Content = bill.phikham;
                    thanhtienLB.Content = bill.thanhtien;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void back(object sender, RoutedEventArgs e)
        {
            patientRecordPage patientRecordPage = new patientRecordPage();
            patientRecordPage.Show();
            this.Close();
        }
    }
}
