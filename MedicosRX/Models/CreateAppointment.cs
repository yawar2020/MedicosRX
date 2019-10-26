using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicosRX.Models
{
    public class CreateAppointment
    {
        public int AppointmentId { get; set; }
        public Nullable<int> PId { get; set; }
        public string DrNameSpeciality { get; set; }
        public Nullable<int> Drid { get; set; }
        public Nullable<System.DateTime> DateOfAppointment { get; set; }
        public string TimeOfAppointment { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdationDate { get; set; }
    }
}