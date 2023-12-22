using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.model
{
    public class bill : ICloneable, INotifyPropertyChanged
    {
        public string id_bn { get; set; }
        public string ngaykham { get; set; }
        public int phikham { get; set; }
        public int thanhtien { get; set; }
        public string id_nv {  get; set; }
        public string id_kh { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
