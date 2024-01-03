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

namespace HospitalManagement.View.PatientSide
{
   
    public partial class ViewSchedulePage : Window
    {
        private string id_kh { get; set; }
        private BindingList<lichKhamBN> lichKhamBNs { get; set; } = new BindingList<lichKhamBN>();
        public ViewSchedulePage()
        {
            InitializeComponent();
            id_kh = "BN001";
            Loaded += ViewSchedulePage_Loaded;
        }

        private void ViewSchedulePage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_xemLichKham", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_kh", id_kh);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    lichKhamBNs.Add(new lichKhamBN()
                                    {
                                        NGAYHEN = reader["NGAYHEN"].ToString().Split()[0],
                                        GIO_BD = reader["GIO_BD"].ToString(),
                                        GIO_KT = reader["GIO_KT"].ToString(),
                                        HOTEN = reader["HOTEN"].ToString(),
                                        ID_NS = reader["ID_NS"].ToString()
                                    });
                                }
                            }
                        }
                    }
                    ComboboxSchedule.ItemsSource = lichKhamBNs;
                }
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
