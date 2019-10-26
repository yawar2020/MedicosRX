using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicosRX.Models
{
    public class PatientReport
    {
        public int Pid { get; set; }
        public string PName { get; set; }
        public string Referby { get; set; }
        public string WT { get; set; }
        public string HT { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public int did { get; set; }
        public string Complain { get; set; }
        public string History { get; set; }
        public string CNS { get; set; }
        public string PA { get; set; }
        public string SPO2 { get; set; }
        public string BP { get; set; }
        public string LR { get; set; }
        public string PR { get; set; }
        public string Temp { get; set; }
        public string GCs { get; set; }
        public string Advice { get; set; }
        public string Investigation { get; set; }
        public string Dx { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBY { get; set; }
        public string HR { get; set; }
        public string GRBS { get; set; }
        public string Emergency { get; set; }
        public string Reveiw { get; set; }

        public int Updatedby { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
    public class TabNameandDoses
    {
        public int Pid { get; set; }
        public string TabName { get; set; }
        public int Rpid { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime Updateddate { get; set; }

        public string txtSearchMedicines1 { get; set; }
        public string txtSearchMedicines2 { get; set; }
        public string txtSearchMedicines3 { get; set; }
        public string txtSearchMedicines4 { get; set; }
        public string txtSearchMedicines5 { get; set; }
        public string txtSearchMedicines6 { get; set; }
        public string txtSearchMedicines7 { get; set; }
        public string txtSearchMedicines8 { get; set; }
        public string txtSearchMedicines9 { get; set; }


        public string Doses1 { get; set; }
        public string Doses2 { get; set; }
        public string Doses3 { get; set; }
        public string Doses4 { get; set; }
        public string Doses5 { get; set; }
        public string Doses6 { get; set; }
        public string Doses7 { get; set; }
        public string Doses8 { get; set; }
        public string Doses9 { get; set; }

    }
}