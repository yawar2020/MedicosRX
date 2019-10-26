using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicosRX.Models
{
    public partial class AppointmentDetail
    {
        public int AppointmentId { get; set; }
        public Nullable<int> PId { get; set; }
        public string PName { get; set; }
        public string DName { get; set; }

        public Nullable<System.DateTime> DateOfAppointment { get; set; }
        public string TimeOfAppointment { get; set; }

    }
}