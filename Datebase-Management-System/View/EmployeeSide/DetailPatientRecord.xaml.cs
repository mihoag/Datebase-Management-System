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
    /// Interaction logic for DetailPatientRecord.xaml
    /// </summary>
    public partial class DetailPatientRecord : Window
    {
       public BindingList<DetailPatientRC> listDetailPatient {  get; set; }
       public string idBN { get; set; }
        public DetailPatientRecord(string id_bn)
        {
            idBN = id_bn;
            InitializeComponent();
        }

        

        private void back(object sender, RoutedEventArgs e)
        {
            patientRecord patientRecord = new patientRecord();
            patientRecord.Show();
            this.Close();
        }

        private void search(object sender, KeyEventArgs e)
        {

        }

        private void MainLoad(object sender, RoutedEventArgs e)
        {
            listDetailPatient = new BindingList<DetailPatientRC>();
            LoadDetailRecord();
            ComboboxDetailRecord.ItemsSource = listDetailPatient;
        }

        private void LoadDetailRecord()
        {
            listDetailPatient.Clear();
            try
            {
                listDetailPatient.Clear();
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_chitietHSBN", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_bn", idBN);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                   
                                    string ID_BN = reader["ID_BN"].ToString();
                                    string NGAYKHAM = reader["NGAYKHAM"].ToString().Substring(0,10);
                                    string TINHTRANG = reader["TINHTRANG"].ToString();
                                    string NGUOIKHAM = reader["NGUOIKHAM"].ToString();

                                    DetailPatientRC dt = new DetailPatientRC() {ngaykham = NGAYKHAM , tinhtrang = TINHTRANG, nguoikham = NGUOIKHAM };
                                    DetailPatientRC.ID_BN = ID_BN;
                                    listDetailPatient.Add(dt);
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

        private void setbill(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string ngaykham = btn.Tag as string;
            setbill wd = new setbill(DetailPatientRC.ID_BN, ngaykham);
            wd.Show();
            this.Close();
        }
    }
}
