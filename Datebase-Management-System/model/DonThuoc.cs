using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.model
{
    public class DonThuoc: ICloneable, INotifyPropertyChanged
    {
        public string id_thuoc { get; set; }
        public string id_bn { get; set; }
        public string ngaykham { get; set; }
        public int soluong { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
