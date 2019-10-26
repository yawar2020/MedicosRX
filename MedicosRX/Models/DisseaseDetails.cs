using System.ComponentModel.DataAnnotations;

namespace MedicosRX.Models
{
    public class DisseaseDetails
    {
        public int? DiseasesId { get; set; }

        public string DisseaseName { get; set; }
        [Key]
        public int MedicineId { get; set; }

        public string MedicineName { get; set; }

        public string Formula { get; set; }

        public string Comment { get; set; }
    }
}