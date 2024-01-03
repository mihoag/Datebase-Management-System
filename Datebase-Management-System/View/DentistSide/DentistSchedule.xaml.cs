using HospitalManagement.model;
using HospitalManagement.Utils;
using HospitalManagement.View.EmployeeSide;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Net;
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
    /// Interaction logic for DentistSchedule.xaml
    /// </summary>
    public partial class DentistSchedule : Window
    {
        public BindingList<dentistSchedule> listSchedule;
        public string idNS { get; set; }

        public DentistSchedule()
        {
            idNS = "NS001";
            InitializeComponent();

        }

        private void MainLoad(object sender, RoutedEventArgs e)
        {
            listSchedule = new BindingList<dentistSchedule>();
            loadAllSchedule(idNS, null);
            ComboboxSchedule.ItemsSource = listSchedule;
        }

        private void back(object sender, RoutedEventArgs e)
        {
            HomeDentist homeDentist = new HomeDentist();
            homeDentist.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            setSchedule s = new setSchedule(idNS);
            s.Show();
            this.Close();
        }

        private void loadAllSchedule(string id_ns, string ngayhen)
        {
            listSchedule.Clear();
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_LICHNHASI", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_ns", id_ns);
                        if (ngayhen != null)
                        {
                            cmd.Parameters.AddWithValue("@ngayhen", ngayhen);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@ngayhen", DBNull.Value);
                        }
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

                                    //MessageBox.Show(ID_NHASI);

                                    dentistSchedule d = new dentistSchedule() { ID_NS = ID_NHASI, NGAYHEN = NGAYHEN, GIO_BD = GIOBD, GIO_KT = GIOKT, CHITIET = CHITIET };
                                    listSchedule.Add(d);
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

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = (DateTime)ngayhenDP.SelectedDate;
            string NGAYHEN = selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day;
            this.loadAllSchedule("NS001", NGAYHEN);
        }
    }
}
