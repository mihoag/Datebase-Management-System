using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagement.model
{
    public class DetailPatientRC : ICloneable, INotifyCollectionChanged
    {
        public static string ID_BN { get; set; }   
        public string ngaykham { get; set; }
        public string tinhtrang { get; set; }
        public string nguoikham { get; set; }
        
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
