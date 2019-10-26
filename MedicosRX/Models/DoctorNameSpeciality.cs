using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicosRX.Models
{
    public class DoctorNameSpeciality
    {
        public string NameSpecialist { get; set; }
        public int? DrId { get; set; }
        public string flag { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }
}