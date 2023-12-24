using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.model
{
    public class User: INotifyPropertyChanged
    {
        public string DIENTHOAI { get; set; }
        public string HOTEN { get; set; }
        public string NGAYSINH { get; set;}
        public string DIACHI { get; set; }
        public string MATKHAU { get; set; }
        public string VAITRO { get; set; }
        public string ID_USER { get; set; }
        public string ACTIVE { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
