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

namespace HospitalManagement.View.DentistSide
{
    /// <summary>
    /// Interaction logic for patientRecord.xaml
    /// </summary>
    public partial class patientRecord : Window
    {
        public patientRecord()
        {
            InitializeComponent();
        }
        public BindingList<PatientRecord> listPatientRecord { get; set; }


        private void Mainload(object sender, RoutedEventArgs e)
        {
            listPatientRecord = new BindingList<PatientRecord>();
            LoadAllPatientRecord();
            ComboboxPatientRecord.ItemsSource = listPatientRecord;
        }
        private void LoadAllPatientRecord()
        {
            listPatientRecord.Clear();
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_HSBN", connection))
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
                                    string ID_BN = reader["ID_BN"].ToString();
                                    string IDKH = reader["ID_KH"].ToString();
                                    string HOTEN = reader["HOTEN"].ToString();

                                    PatientRecord p = new PatientRecord() { ID_BN = ID_BN, ID_KH = IDKH, hoten = HOTEN };
                                    listPatientRecord.Add(p);
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

        private void back(object sender, RoutedEventArgs e)
        {
            HomeDentist homeDentist = new HomeDentist();
            homeDentist.Show();
            this.Close();
        }

        private void search(object sender, KeyEventArgs e)
        {
           
            if (e.Key == Key.Enter)
            {
                var keyword = keywordSearch.Text;
                if (keyword == "")
                {
                    LoadAllPatientRecord();
                }
                else
                {
                    //
                    listPatientRecord.Clear();
                    using (SqlConnection connection = DB.Instance.Connection)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_HSBN_DT", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@dt", keyword);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Check if the reader has rows
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        // Access the columns in the result set
                                        string ID_BN = reader["ID_BN"].ToString();
                                        string IDKH = reader["ID_KH"].ToString();
                                        string HOTEN = reader["HOTEN"].ToString();

                                        PatientRecord p = new PatientRecord() { ID_BN = ID_BN, ID_KH = IDKH, hoten = HOTEN };
                                        listPatientRecord.Add(p);
                                    }
                                }
                            }
                        }
                    }

                }
            }

        }

        private void detail(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string id_BN = btn.Tag as string;
            //MessageBox.Show(id_BN);
            DetailPatientRecord wd = new DetailPatientRecord(id_BN);
            wd.Show();
            this.Close();
        }
    }
}
