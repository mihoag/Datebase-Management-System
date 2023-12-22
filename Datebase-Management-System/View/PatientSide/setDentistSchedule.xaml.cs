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
    /// Interaction logic for setDentistSchedule.xaml
    /// </summary>
    public partial class setDentistSchedule : Window
    {
        private BindingList<dentisSchedule> listSchedule { get; set; } = new BindingList<dentisSchedule>();
        public setDentistSchedule()
        {
            InitializeComponent();
            Loaded += SetDentistSchedule_Loaded;
        }

        private void SetDentistSchedule_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_tatcaLHNS", connection))
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
                                    string ID_NHASI = reader["ID_NS"].ToString();
                                    string NGAYHEN = reader["NGAYHEN"].ToString().Substring(0, 10);
                                    string GIOBD = reader["GIO_BD"].ToString();
                                    string GIOKT = reader["GIO_KT"].ToString();
                                    string CHITIET = reader["CHITIET"].ToString();


                                    dentisSchedule d = new dentisSchedule() { ID_NS = ID_NHASI, NGAYHEN = NGAYHEN, GIO_BD = GIOBD, GIO_KT = GIOKT, CHITIET = CHITIET };
                                    listSchedule.Add(d);
                                }
                            }
                        }
                    }
                }
                ComboboxScheduleTime.ItemsSource = listSchedule;
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

        private void datlic(object sender, RoutedEventArgs e)
        {
            setDentistSchedule2 setSchedule = new setDentistSchedule2();
            setSchedule.Show();
            this.Close();
        }
    }
}
