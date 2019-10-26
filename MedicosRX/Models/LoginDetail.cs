using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicosRX.Models
{
    public class LoginDetail
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public int UserTypeId { get; set; }
        public string EmailId { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Updatedby { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}