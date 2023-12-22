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
    /// <summary>
    /// Interaction logic for patientRecordPage.xaml
    /// </summary>
    public partial class patientRecordPage : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string id_kh;
        public BindingList<patientRecord2> PatientRecord2 { get; set; } = new BindingList<patientRecord2>();
        public patientRecordPage()
        {
            InitializeComponent();
            id_kh = "BN001";
            Loaded += PatientRecordPage_Loaded;
        }

        private void PatientRecordPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_xemHoSoBN", connection))
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
                                    PatientRecord2.Add(new patientRecord2()
                                    {
                                        ID_BN = reader["ID_BN"].ToString(),
                                        NGAYKHAM = reader["NGAYKHAM"].ToString().Split()[0],
                                        NGUOIKHAM = reader["NGUOIKHAM"].ToString(),
                                        TINHTRANG = reader["TINHTRANG"].ToString(),
                                        HOTEN = reader["HOTEN"].ToString(),
                                    });
                                }
                            }
                        }
                    }

                }
                ComboboxHSBN.ItemsSource = PatientRecord2;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void btnBack(object sender, RoutedEventArgs e)
        {
            HomePatient homePatient = new HomePatient();
            homePatient.Show();
            this.Close();
        }

        private void ComboboxHSBN_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = ComboboxHSBN.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= PatientRecord2.Count)
            {
                return;
            }
        
            detailPatientRecord detail = new detailPatientRecord(PatientRecord2[selectedIndex]);
            detail.Show();
            this.Close();
        }
    }
}
