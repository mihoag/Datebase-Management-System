using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.model
{
    public class patientSchedule : ICloneable, INotifyPropertyChanged
    {

        public string ID_LICHHEN { get; set; }
        public string ID_NS {  get; set; } 
        public string ID_KH {  get; set; }
        public string ID_NV { get; set; }
        public string NGAYHEN {  get; set; }
        public string GIO_BD { get; set; }
        public string GIO_KT { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
