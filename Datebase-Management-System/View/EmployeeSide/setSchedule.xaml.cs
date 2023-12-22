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

namespace HospitalManagement.View.EmployeeSide
{
    /// <summary>
    /// Interaction logic for setSchedule.xaml
    /// </summary>
    public partial class setSchedule : Window
    {
        public patientSchedule ps {  get; set; }
        private void MainLoad(object sender, RoutedEventArgs e)
        {
            ps = new patientSchedule();
            this.DataContext = ps;
        }
        public setSchedule()
        {
            InitializeComponent();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            ListFreeTimeDentist wd = new ListFreeTimeDentist();
            wd.Show();
            this.Close();
        }

        private void SetSchedule(object sender, RoutedEventArgs e)
        {
            DateTime selectedDate = (DateTime)date.SelectedDate;
            ps.NGAYHEN = selectedDate.Year + "-" + selectedDate.Month + "-" + selectedDate.Day;
            //MessageBox.Show(ps.ID_KH + " " + ps.ID_LICHHEN + " " + ps.GIO_BD + " " + ps.GIO_KT + " " + ps.NGAYHEN);
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DATLICHKHAM", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_ns", ps.ID_NS);
                        cmd.Parameters.AddWithValue("@id_kh", ps.ID_KH);
                        cmd.Parameters.AddWithValue("@id_nv", ps.ID_NV);
                        cmd.Parameters.AddWithValue("@ngayhen", ps.NGAYHEN);
                        cmd.Parameters.AddWithValue("@gio_bd", ps.GIO_BD);
                        cmd.Parameters.AddWithValue("@gio_kt", ps.GIO_KT);

                        Object check = cmd.ExecuteScalar();
                        ListFreeTimeDentist wd = new ListFreeTimeDentist();
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
