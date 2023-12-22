using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.Utils
{
    public class DB 
    {
        private static DB? _instance = null;
        private SqlConnection _connection = null;
        private static SqlConnectionStringBuilder _builder = null;
        public string ConnectionString { get; set; } = "";

        public static DB Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DB();
                }

                return _instance;
            }
        }

        public SqlConnection? Connection
        {
            get
            {
                
                    // Xay dung builer de noi chuoi
                    _builder = new SqlConnectionStringBuilder();
                    _builder.DataSource = ".\\SQLEXPRESS";
                    _builder.UserID = "haonhat";
                    _builder.Password = "1234";
                    _builder.InitialCatalog = "QUANLYPHONGKHAM";
                    _builder.TrustServerCertificate = true;
                    //
                    
                    ConnectionString = _builder.ConnectionString;

                    // Ket noi toi sql
                    _connection = new SqlConnection(ConnectionString); ;
                   
                _connection.Open();
                return _connection;
            }
        }
    }
}
