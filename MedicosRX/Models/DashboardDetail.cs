using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicosRX.Models
{
    public class DashboardDetail
    {
        public int CountCurrentAppointment { get; set; }
        public int CountExistingPatient { get; set; }
        public float SumEarningPerday { get; set; }

        public List<PatientAndDoctorChart> PatientAndDoctorCharts { get; set; }

    }

    public class PatientAndDoctorChart
    {
        public string DoctorName { get; set; }
        public int PatientCount { get; set; }

    }

}