using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.model
{
    public class Medicine : INotifyPropertyChanged, ICloneable
    {
        public String ID_THUOC {  get; set; }   
        public String TEN_THUOC { get; set; }
        public String DONVITINH { get; set; }
        public String CHI_DINH { get; set; }
        public int TONKHO { get; set; }
        public String NGAY_HH { get; set; }
        public int DONGIA {  get; set; }
        public String ID_QTV {  get; set; }
       
        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
           return MemberwiseClone();
        }
    }
}
