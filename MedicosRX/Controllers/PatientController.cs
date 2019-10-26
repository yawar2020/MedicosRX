using MedicosRX.Models;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
namespace MedicosRX.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        HospitalManagementEntities db = new Models.HospitalManagementEntities();
          SqlConnection con = new SqlConnection("data source=AZAM-PC\\SQLEXPRESS;initial catalog=HospitalManagement;integrated security=True");
      //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString());

     // SqlConnection con = new SqlConnection("Data Source=ISMAIL\\SQLEXPRESS;Initial Catalog=HospitalManagement;User Id=sa;Password=123;");
        public ActionResult GetPatient()
        {

            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString());

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

                    PName = Convert.ToString(dr["PName"]),
                    Age = Convert.ToString(dr["Age"]),
                    Address = Convert.ToString(dr["Address"]),
                    Gender = Convert.ToString(dr["Gender"]),
                    MobileNo = Convert.ToString(dr["MobileNo"]),
                    ReferBy = Convert.ToString(dr["ReferBy"]),
                    Pid = Convert.ToInt32(dr["Pid"]),
                    PatientTypeName = Convert.ToString(dr["PatientType"]),
                                       
                });
            }
            



            return View(pat.ToList());
        }

        public ActionResult CreatePatientReceipt()
        {
            string patient = "0";
            ViewBag.Pid = patient;
            return View();
        }

        [HttpGet]
        public ActionResult CreatePatient()
        {
            ViewBag.Visable = "none";

            return View();
        }
        [HttpPost]
        public ActionResult CreatePatient(Patient obj)
        {
            HospitalManagementEntities db = new Models.HospitalManagementEntities();
            
            SqlCommand cmd = new SqlCommand("sp_InsertPatient",con);
            con.Open();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Ptname", obj.PName);
            cmd.Parameters.AddWithValue("@gender", obj.Gender);
            cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
            cmd.Parameters.AddWithValue("@PatientType", obj.PatientType);
            cmd.Parameters.AddWithValue("@Address", obj.Address);
            cmd.Parameters.AddWithValue("@ReferBy", obj.ReferBy);
            cmd.Parameters.AddWithValue("@Age", obj.Age);
            cmd.Parameters.AddWithValue("@CreatedBy", 1);




            Int32 i = Convert.ToInt32(cmd.ExecuteScalar());


           
            if (i > 0)
            {
                obj.Pid = (int)i;
                ViewBag.Visable = "block";
                ViewBag.Pid = i.ToString();
                return View(obj);
            }
            else
            {
                ViewBag.Visable = false;
                ViewBag.Message = "Record Failed To Insert";
                return View(obj);
            }
            con.Close();
        }

        [HttpGet]
        public ActionResult EditDoctor(int? id)
        {
            Doctor GetDoctorById = db.Doctors.Find(id);

            return View(GetDoctorById);
        }
        [HttpPost]
        public ActionResult EditDoctor(sp_get_Doctors_Result Editobj)
        {
            HospitalManagementEntities db = new Models.HospitalManagementEntities();


            db.sp_Update_Doctors(
                  Editobj.Name,
                  Editobj.EmailId,
                  Editobj.Mobile,
                  Editobj.Gender,
                  Editobj.Specialist,
                  Editobj.Date_of_joining,
                  Editobj.Qualification,
                  Editobj.Address,
                  Editobj.Timing,
                  Convert.ToInt32(Session["UserId"]),
                  Convert.ToInt32(Session["UserId"]),
                  Editobj.DoctorId
                  );
            int Result = 1;
            if (Result > 0)
            {
                return RedirectToAction("GetDoctor");
            }
            else
            {
                ViewBag.Message = "Record Failed To Update";
                return View(Editobj);
            }
        }
        [HttpGet]
        public ActionResult GetDoctorbyId(int? DoctorId)
        {
            Doctor GetDoctorById = db.Doctors.Find(DoctorId);

            return View(GetDoctorById);
        }

        [HttpGet]
        public ActionResult DeleteDoctorbyId(int? DoctorId)
        {
            Doctor GetDoctorById = db.Doctors.Find(DoctorId);

            return View(GetDoctorById);
        }
        [HttpPost]
        public ActionResult DeleteDoctorbyId(Doctor doctor)
        {
            Doctor deleteDoctorById = db.Doctors.Find(doctor.DoctorId);
            db.Doctors.Remove(deleteDoctorById);
            db.SaveChanges();
            return RedirectToAction("GetDoctor");
        }

        public JsonResult GetDoctorName(string term)
        {
            HospitalManagementEntities db = new HospitalManagementEntities();
            

            SqlCommand cmd = new SqlCommand("sp_getDoctorName", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@searchText",term);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            List<DoctorNameSpeciality> drNameSpty = new List<DoctorNameSpeciality>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                drNameSpty.Add(new DoctorNameSpeciality
                {
                    NameSpecialist = Convert.ToString(dr["name"]) + "-" + Convert.ToString(dr["Specialist"])
                });

            }
            List<string> drNameSptyterm = drNameSpty.Select(s=>s.NameSpecialist).ToList(); ;

            return Json(drNameSptyterm, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CreateAppointMent(FormCollection frm)
        {
            HospitalManagementEntities db = new Models.HospitalManagementEntities();
            CreateAppointment obj = new CreateAppointment();
            SqlCommand cmd = new SqlCommand("sp_CreateAppointment", con);
            obj.DrNameSpeciality = frm["Doctor"].ToString();
            con.Open();
            string name= obj.DrNameSpeciality.Split('-')[0];
            string specialist= obj.DrNameSpeciality.Split('-')[1];
            obj.Drid = Convert.ToInt32(db.Doctors.Where(s => s.Name == name && s.Specialist==specialist).Select(s=>s.DoctorId).FirstOrDefault());
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PatId", frm["AppPid"]);
            cmd.Parameters.AddWithValue("@DrId", obj.Drid);
            cmd.Parameters.AddWithValue("@TotalAmount", frm["Fees"]);
            cmd.Parameters.AddWithValue("@timeAppoint", frm["Timing"]);
            cmd.Parameters.AddWithValue("@CreatedBy", 1);




            Int32 i = Convert.ToInt32(cmd.ExecuteScalar());



            if (i > 0)
            {
                string Controller=string.Empty,Action=string.Empty;
                if (Convert.ToInt32(Session["RoleId"]) == 1)
                {
                    Controller = "Appointment";
                    Action = "ViewAppointment";
                    
                }
                else if (Convert.ToInt32(Session["RoleId"]) == 2)
                {
                    Controller = "DashBoard";
                    Action = "DoctorDashBoard";

                }
             
                return RedirectToAction(Action, Controller);
            }
            else
            {
                con.Close();
                 
                return View("GetPatient");
            }
            
        }


        [HttpGet]
        public ActionResult EditPatient(int? id)
        {
           Patient ptobj= db.Patients.Find(id);

            return View(ptobj);
        }
    }
}