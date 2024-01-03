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

namespace HospitalManagement.View.DentistSide
{
    /// <summary>
    /// Interaction logic for setSchedule.xaml
    /// </summary>
    public partial class setSchedule : Window
    {
        public dentistSchedule ds {  get; set; }
        public string idNS { get; set; }
        private void MainLoad(object sender, RoutedEventArgs e)
        {
            ds = new dentistSchedule();
            ds.ID_NS = idNS;
            this.DataContext = ds;
        }
        public setSchedule(string id_ns)
        {
            idNS = id_ns;
            InitializeComponent();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            DentistSchedule wd = new DentistSchedule();
            wd.Show();
            this.Close();
        }

        private void SetSchedule(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = (DateTime)date.SelectedDate;
            ds.NGAYHEN = selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day;
            //MessageBox.Show(ps.ID_KH + " " + ps.ID_LICHHEN + " " + ps.GIO_BD + " " + ps.GIO_KT + " " + ps.NGAYHEN);
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DATLICHBAN", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_ns", ds.ID_NS);
                        cmd.Parameters.AddWithValue("@ngayhen", ds.NGAYHEN);
                        cmd.Parameters.AddWithValue("@gio_bd", ds.GIO_BD);
                        cmd.Parameters.AddWithValue("@gio_kt", ds.GIO_KT);
                        cmd.Parameters.AddWithValue("@chitiet", ds.CHITIET);

                        Object check = cmd.ExecuteScalar();
                        DentistSchedule wd = new DentistSchedule();
                        wd.Show();
                        this.Close();
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
