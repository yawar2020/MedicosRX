using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using MedicosRX.Models;
using System.Configuration;

namespace MedicosRX.Controllers
{
    public class RolesController : Controller
    {
        // GET: Role

        SqlConnection con = new SqlConnection("data source=AZAM-PC\\SQLEXPRESS;initial catalog=HospitalManagement;integrated security=True");
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString());

       // SqlConnection con = new SqlConnection("Data Source=ISMAIL\\SQLEXPRESS;Initial Catalog=HospitalManagement;User Id=sa;Password=123;");

        public ActionResult RolesView()
        {
            SqlCommand cmd = new SqlCommand("sp_get_ROLES", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            List<Roles> GetRoles = new List<Roles>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GetRoles.Add(new Roles
                {

                    RoleId = Convert.ToInt32(dr["RoleId"]),
                   RoleName = Convert.ToString(dr["RoleName"]),
                    

                });
            }


            return View(GetRoles.ToList());
        }


        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpGet]
        public ActionResult EditRole()
        {
            return View();
        }
    }
}