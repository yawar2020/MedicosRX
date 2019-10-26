using MedicosRX.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MedicosRX.Controllers
{
    public class AppointmentController : Controller
    {
        // GET: Appointment
        // SqlConnection con = new SqlConnection("data source=AZAM-PC\\SQLEXPRESS;initial catalog=HospitalManagement;integrated security=True");
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString());
        SqlConnection con = new SqlConnection("data source=AZAM-PC\\SQLEXPRESS;initial catalog=HospitalManagement;integrated security=True");

        // SqlConnection con = new SqlConnection("Data Source=ISMAIL\\SQLEXPRESS;Initial Catalog=HospitalManagement;User Id=sa;Password=123;");


        public ActionResult ViewAppointment()
        {
            SqlCommand cmd = new SqlCommand("sp_getAppointment", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            List<AppointmentDetail> GetAppointmentDetails = new List<AppointmentDetail>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GetAppointmentDetails.Add(new AppointmentDetail
                {

                    AppointmentId = Convert.ToInt32(dr["AppointmentId"]),
                    DateOfAppointment = Convert.ToDateTime(dr["DateOfAppointment"]),
                    TimeOfAppointment = Convert.ToString(dr["TimeOfAppointment"]),
                    PId = Convert.ToInt32(dr["Pid"]),
                    PName = Convert.ToString(dr["PName"]),
                    DName = Convert.ToString(dr["Name"]),


                });
            }

            return View(GetAppointmentDetails);


        }
        [HttpGet]
        public ActionResult CreateAppointment()
        {
            SqlCommand cmd = new SqlCommand("sp_get_Patient", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            List<PatientDetail> pat = new List<PatientDetail>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                pat.Add(new PatientDetail
                {
                    Pid = Convert.ToInt32(dr["Pid"]),
                    PName = Convert.ToString(dr["PName"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    ReferBy = Convert.ToString(dr["ReferBy"]),
                    UpdatetedTime =Convert.ToString(dr["UpdatetedTime"])

                });
            }

            return View(pat.ToList());
        }
        [HttpPost]
        public ActionResult CreateAppointment(int id)
        {
            HospitalManagementEntities db = new HospitalManagementEntities();

            return View(db.sp_getAppointment().ToList());
        }
    }
}