using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicosRX.Models
{
    public class PatientDetail
    {
        public int Pid { get; set; }
        public string PName { get; set; }
        public Nullable<int> PatientType { get; set; }
        public string ReferBy { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string PatientTypeName { get; set; }
        public Nullable<System.DateTime> Date_of_Admission { get; set; }
        public Nullable<System.DateTime> Date_of_DisCharge { get; set; }
        public Nullable<int> Createdby { get; set; }
        public Nullable<int> Updatedby { get; set; }
        public string CreatedDate { get; set; }
        public string UpdatetedTime { get; set; }
    }
}