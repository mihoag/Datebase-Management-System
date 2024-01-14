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

namespace HospitalManagement.View.AdminSide
{
    /// <summary>
    /// Interaction logic for MedicinePage.xaml
    /// </summary>
    public partial class MedicinePage : Window
    {
        BindingList<Medicine> listMedicine = null;
        public MedicinePage()
        {
            InitializeComponent();
            Loaded += MedicinePage_Loaded;
        }

        private void MedicinePage_Loaded(object sender, RoutedEventArgs e)
        {
            listMedicine = new BindingList<Medicine>();
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_xemthuoc", connection))
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
                                    string ID_Thuoc = reader["ID_THUOC"].ToString();
                                    string tenthuoc = reader["TENTHUOC"].ToString();
                                    string donvitinh = reader["DONVITINH"].ToString();
                                    string chidinh = reader["DONVITINH"].ToString();
                                    int tonkho = (int)reader["TONKHO"];
                                    string ngayhethan = reader["NGAYHETHAN"].ToString();
                                    int dongia = (int)reader["DONGIA"];
                                    string ID_QTV = reader["ID_QTV"].ToString();

                                    Medicine medicine = new Medicine()
                                    {
                                        ID_THUOC = ID_Thuoc,
                                        TEN_THUOC = tenthuoc,
                                        DONVITINH = donvitinh,
                                        CHI_DINH = chidinh,
                                        TONKHO = tonkho,
                                        NGAY_HH = ngayhethan,
                                        DONGIA = dongia,
                                        ID_QTV = ID_QTV
                                    };
                                    listMedicine.Add(medicine);
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
            ComboboxMedicine.ItemsSource = listMedicine;
        }

        private void back(object sender, RoutedEventArgs e)
        {
            HomePage page = new HomePage();
            page.Show();
            this.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Edit_medicine(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ComboboxMedicine.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= listMedicine.Count)
            {
                return;
            }
            string id_medicine = listMedicine[selectedIndex].ID_THUOC;
            EditMedicinePage page = new EditMedicinePage(id_medicine);
            page.Show();
            this.Close();
        }

        private void addMedicine(object sender, RoutedEventArgs e)
        {
            addMedicine addMedicine = new addMedicine();
            addMedicine.Show();
            this.Close();
        }
    }
}
