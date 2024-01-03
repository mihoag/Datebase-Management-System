using HospitalManagement.model;
using HospitalManagement.Utils;
using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace HospitalManagement.View.EmployeeSide
{
    /// <summary>
    /// Interaction logic for ListFreeTimeDentist.xaml
    /// </summary>
    public partial class ListFreeTimeDentist : Window
    {
        public ListFreeTimeDentist()
        {
            InitializeComponent();
        }
        public BindingList<dentistSchedule> listSchedule;

        private void MainLoad(object sender, RoutedEventArgs e)
        {
            listSchedule = new BindingList<dentistSchedule>();
            loadAllSchedule();
            ComboboxSchedule.ItemsSource = listSchedule;
        }
        private void back(object sender, RoutedEventArgs e)
        {
            HomeEmployee homeEmployee = new HomeEmployee();
            homeEmployee.Show();
            this.Close();   
        }

        private void loadAllSchedule()
        {
            listSchedule.Clear();
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
                                    string NGAYHEN = reader["NGAYHEN"].ToString().Substring(0,10);
                                    string GIOBD = reader["GIO_BD"].ToString();
                                    string GIOKT = reader["GIO_KT"].ToString();
                                    string CHITIET = reader["CHITIET"].ToString();

                                    //MessageBox.Show(ID_NHASI);

                                    dentistSchedule d = new dentistSchedule() { ID_NS = ID_NHASI, NGAYHEN = NGAYHEN, GIO_BD = GIOBD, GIO_KT = GIOKT, CHITIET = CHITIET };
                                    listSchedule.Add(d);                                }
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

        private void datlic(object sender, RoutedEventArgs e)
        {
             setSchedule wd = new setSchedule();
            wd.Show();
            this.Close();
        }

        private void search(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var keyword = keywordsearch.Text;
                if(keyword == "")
                {
                    loadAllSchedule();
                }
                else
                {
                   //
                   listSchedule.Clear();
                    using (SqlConnection connection = DB.Instance.Connection)
                    {
                        using (SqlCommand cmd = new SqlCommand("sp_LHNS_ID", connection))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID_NS", keyword);
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
            }
        }
    }
}
