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

namespace HospitalManagement.View.AdminSide
{
    public partial class AccountHandlerPage : Window
    {
        private BindingList<User> listUser = new BindingList<User>();
        public AccountHandlerPage()
        {
            InitializeComponent();
            Loaded += AccountHandlerPage_Loaded;
        }

        private void getAll()
        {
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_getAllAccount", connection))
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
                                    listUser.Add(new User()
                                    {
                                        ID_USER = reader["ID_USER"].ToString(),
                                        DIENTHOAI = reader["DIENTHOAI"].ToString(),
                                        HOTEN = reader["HOTEN"].ToString(),
                                        VAITRO = reader["VAITRO"].ToString(),
                                        ACTIVE = reader["ACTIVE"].ToString() == "True" ? "ACTIVE" : "LOCK"
                                    }); ;
                                }
                            }
                        }
                    }
                }
                ComboboxListUser.ItemsSource = listUser;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void AccountHandlerPage_Loaded(object sender, RoutedEventArgs e)
        {
            getAll();
        }

        private void back(object sender, RoutedEventArgs e)
        {
            HomePage page = new HomePage();
            page.Show();
            this.Close();
        }

        private void search(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var keyword = keywordSearch.Text;
                if (keyword == "")
                {
                    getAll();
                }
                else
                {
                    listUser.Clear();
                    //
                    try
                    {
                        using (SqlConnection connection = DB.Instance.Connection)
                        {
                            using (SqlCommand cmd = new SqlCommand("sp_searchUser", connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@search", keyword);
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    // Check if the reader has rows
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            // Access the columns in the result set
                                            listUser.Add(new User()
                                            {
                                                ID_USER = reader["ID_USER"].ToString(),
                                                DIENTHOAI = reader["DIENTHOAI"].ToString(),
                                                HOTEN = reader["HOTEN"].ToString(),
                                                VAITRO = reader["VAITRO"].ToString(),
                                                ACTIVE = reader["ACTIVE"].ToString() == "True" ? "ACTIVE" : "LOCK"
                                            }); ;
                                        }
                                    }
                                }
                            }
                        }
                        ComboboxListUser.ItemsSource = listUser;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void activeAccount_click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ComboboxListUser.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= listUser.Count)
            {
                return;
            }
            string ID_USER = listUser[selectedIndex].ID_USER;
            if (listUser[selectedIndex].ACTIVE == "ACTIVE") return;

            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_activeAccount", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_user", ID_USER);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Tài khoản đã được mở khóa");
                        listUser[selectedIndex].ACTIVE = "ACTIVE";
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void lockAccount_click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = ComboboxListUser.SelectedIndex;
            if (selectedIndex < 0 || selectedIndex >= listUser.Count)
            {
                return;
            }
            string ID_USER = listUser[selectedIndex].ID_USER;
            if (listUser[selectedIndex].ACTIVE == "LOCK") return;

            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_lockAccount", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id_user", ID_USER);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Đã khóa tài khoản");
                        listUser[selectedIndex].ACTIVE = "LOCK";
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
