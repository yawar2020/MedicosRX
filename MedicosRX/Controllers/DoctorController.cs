using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicosRX.Models;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace MedicosRX.Controllers
{
     
    public class DoctorController : Controller
    {
        // GET: Doctor
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString());
        SqlConnection con = new SqlConnection("data source=AZAM-PC\\SQLEXPRESS;initial catalog=HospitalManagement;integrated security=True");

        //SqlConnection con = new SqlConnection("Data Source=ISMAIL\\SQLEXPRESS;Initial Catalog=HospitalManagement;User Id=sa;Password=123;");


        HospitalManagementEntities db = new Models.HospitalManagementEntities();
        public ActionResult GetDoctor()
        {
            return View(db.sp_get_Doctors());
        }
        [HttpGet]
        public ActionResult CreateDoctor()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateDoctor(sp_get_Doctors_Result obj)
        {
            HospitalManagementEntities db = new Models.HospitalManagementEntities();
            sp_get_Doctors_Result createobj = new Models.sp_get_Doctors_Result();
            createobj.Name = obj.Name;
            createobj.EmailId = obj.EmailId;
            createobj.Mobile = obj.Mobile;

            createobj.Specialist = obj.Specialist;
            createobj.Qualification = obj.Qualification;
            createobj.Gender = obj.Gender;
            createobj.Date_of_joining = obj.Date_of_joining;
            createobj.Address = obj.Address;
            createobj.Timing = obj.Timing;
            createobj.CreatedBy = Convert.ToInt32(Session["UserId"]);
            createobj.UpdatedBy = Convert.ToInt32(Session["UserId"]);

            db.sp_InsertDoctors(
                  createobj.Name,
                  createobj.EmailId,
                  createobj.Mobile,
                  createobj.Gender,
                  createobj.Specialist,
                  createobj.Date_of_joining,
                  createobj.Qualification,
                  createobj.Address,
                  createobj.Timing,
                  createobj.CreatedBy,
                  createobj.UpdatedBy);
            int Result = 1;
            if (Result > 0)
            {
                return RedirectToAction("GetDoctor");
            }
            else
            {
                ViewBag.Message = "Record Failed To Insert";
                return View(createobj);
            }

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
        [HttpGet]
        public ActionResult PatientReport(int ?id)
        {
            Patient ptobj = db.Patients.Find(id);
            return View("Index", ptobj);
        }

        [HttpPost]
        public ActionResult PatientReport(PatientReport obj,TabNameandDoses tbandDosesobj)
        {
            HospitalManagementEntities db = new Models.HospitalManagementEntities();
            int Pid = obj.Pid<=0 ? 0 :obj.Pid;
         
            if (Pid != 0)
            {


                SqlCommand cmd = new SqlCommand("sp_insertPatientReport", con);
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Pid", Pid);
                cmd.Parameters.AddWithValue("@did", 1);
                cmd.Parameters.AddWithValue("@PTName", obj.PName!=string.Empty?obj.PName:"");
                cmd.Parameters.AddWithValue("@Gender", obj.Gender != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@ReferBy", obj.Referby != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@HT", obj.HT != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@WT", obj.WT != string.Empty ? obj.WT : "");
                cmd.Parameters.AddWithValue("@Age", obj.Age != string.Empty ? obj.Age : "");
                cmd.Parameters.AddWithValue("@Emergency", obj.Emergency != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@Review", obj.Reveiw != string.Empty ? obj.Reveiw      : "");
                cmd.Parameters.AddWithValue("@Complain", obj.Complain != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@History", obj.History != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@Dx", obj.Dx != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@Investigation", obj.Investigation != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@Advice", obj.Advice != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@GC", obj.GCs != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@Temp", obj.Temp != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@PR", obj.PR != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@LR", obj.LR != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@BP", obj.BP != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@SPO2", obj.SPO2 != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@PA", obj.PA != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@CNS", obj.CNS != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@GRBS", obj.GRBS);
                cmd.Parameters.AddWithValue("@HR", obj.HR != string.Empty ? obj.PName : "");
                cmd.Parameters.AddWithValue("@CreatedBy", obj.CreatedBY);
                cmd.Parameters.AddWithValue("@CreatedDatetime", DateTime.Now);
                cmd.Parameters.AddWithValue("@UpdatedBy", obj.Updatedby);
                cmd.Parameters.AddWithValue("@Updateddatetime", DateTime.Now );
                Int32 i = Convert.ToInt32(cmd.ExecuteScalar());

                if (i > 0)
                {
                    TabNameandDoses tbobj = new TabNameandDoses();
                    StringBuilder tb = new StringBuilder();
                    if (tbandDosesobj.txtSearchMedicines1 != string.Empty)
                    {


                    }
                    tb.Append(tbandDosesobj.Doses1);

                    return View(obj);
                }
                else
                {
                    ViewBag.Visable = false;
                    ViewBag.Message = "Record Failed To Insert";
                    return View(obj);
                }
            }
            else {
                ViewBag.Visable = false;
                ViewBag.Message = "Record Failed To Insert";
                return View(obj);
            }
        }
        public JsonResult GetAllDiseasses()
        {
            HospitalManagementEntities db = new HospitalManagementEntities();
            List<string> Diseasses;

            Diseasses = db.Diseasses.Where(m=>m.DisseaseName!=null).Select(m=>m.DisseaseName).ToList();
           

            return Json(Diseasses, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMedicine(string term)
        {
            HospitalManagementEntities db = new HospitalManagementEntities();
            List<string> Medicines;

            Medicines = db.Medicines.Where(x => x.MedicineName.StartsWith(term))
            .Select(y => y.MedicineName).ToList();

            return Json(Medicines, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInvestigation()
        {
            HospitalManagementEntities db = new HospitalManagementEntities();
            List<string> Diseasses;

            Diseasses = db.InvestigationDetails.Where(m => m.Invistagation != null).Select(m => m.Invistagation).ToList();


            return Json(Diseasses, JsonRequestBehavior.AllowGet);
        }
        public int InsertTabandDoses(TabNameandDoses TabDoses) {
            SqlCommand cmdtab = new SqlCommand("sp_insertTabandDoses", con);
            cmdtab.CommandType = CommandType.StoredProcedure;
            cmdtab.Parameters.AddWithValue("@Pid", TabDoses.Pid);
            cmdtab.Parameters.AddWithValue("@TABNAME",TabDoses.TabName);
            cmdtab.Parameters.AddWithValue("@RpId", TabDoses.Rpid);
            cmdtab.Parameters.AddWithValue("@Dosses", TabDoses.Doses1);
            cmdtab.Parameters.AddWithValue("@CreatedBy", TabDoses.CreatedBy);
            cmdtab.Parameters.AddWithValue("@updatedBy", TabDoses.UpdatedBy);
            int i=0;
            return i;
        }
    }
}
