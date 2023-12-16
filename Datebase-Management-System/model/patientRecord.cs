using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.model
{
    public class PatientRecord : ICloneable, INotifyPropertyChanged
    {
        public string ID_BN { get; set; }
        public string ID_KH { get; set; }
        public string hoten { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;


        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
