using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicosRX.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MedicosRX.Controllers
{
    [CheckSessionTimeOut]
    public class MedicineController : Controller
    {
        // GET: Medicine
        HospitalManagementEntities db = new Models.HospitalManagementEntities();
        SqlConnection con = new SqlConnection("data source=AZAM-PC\\SQLEXPRESS;initial catalog=HospitalManagement;integrated security=True");
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString());

        //SqlConnection con = new SqlConnection("Data Source=ISMAIL\\SQLEXPRESS;Initial Catalog=HospitalManagement;User Id=sa;Password=123;");
   public ActionResult GetMedicine()
        {
            return View(db.sp_get_Medicine());
        }

        [HttpGet]
        public ActionResult CreateMedicineDetail()
        {
            ViewBag.Medicine = new SelectList(db.sp_get_Dissease(), "DiseasesId", "DisseaseName");

            return View();
        }
        [HttpPost]
        public ActionResult CreateMedicineDetail(sp_get_Medicine_Result obj)
        {
            ViewBag.Medicine = new SelectList(db.sp_get_Dissease(), "DiseasesId", "DisseaseName");

            sp_get_Medicine_Result createobj = new Models.sp_get_Medicine_Result();
            createobj.DiseasesId = obj.DiseasesId;
            createobj.MedicineName = obj.MedicineName;
            createobj.Comment = obj.Comment;
            createobj.Formula = obj.Formula;
            createobj.CreatedBy = 1;
            createobj.UpdatedBy =1;

            db.sp_Insert_Medicine(createobj.MedicineName, createobj.Formula, createobj.DiseasesId, createobj.Comment, createobj.CreatedBy, createobj.UpdatedBy);
            int Result = 1;
            if (Result > 0)
            {
                return RedirectToAction("GetMedicine");
            }
            else
            {
                ViewBag.Message = "Record Failed To Insert";
                return View(createobj);
            }
        }


        [HttpGet]
        public ActionResult EditMedicine(int?id)
        {
            ViewBag.Medicine = new SelectList(db.sp_get_Dissease(), "DiseasesId", "DisseaseName");
            var GetMedicineById = (from m in db.Medicines
                                where m.MId==id
                                  select new DisseaseDetails
                                  {
                                      MedicineId = m.MId,
                                      DiseasesId = m.DisseaseId,
                                      Comment = m.Comment,
                                      Formula = m.Formula,
                                      MedicineName = m.MedicineName

                                  }).SingleOrDefault();

                                                               

            return View(GetMedicineById);
        }
        [HttpPost]
        public ActionResult EditMedicine(DisseaseDetails obj)//
        {
            ViewBag.Medicine = new SelectList(db.sp_get_Dissease(), "DiseasesId", "DisseaseName");

            Medicine createobj = db.Medicines.Find(obj.MedicineId);
            createobj.DisseaseId = obj.DiseasesId;
            createobj.MedicineName = obj.MedicineName;
            createobj.Comment = obj.Comment;
            createobj.Formula = obj.Formula;
            createobj.UpdatedBy = 1;
            db.Entry(createobj).State = System.Data.Entity.EntityState.Modified;
          
            // db.sp_Insert_Medicine(createobj.MedicineName, createobj.Formula, createobj.DisseaseId, createobj.Comment, createobj.CreatedBy, createobj.UpdatedBy);


            SqlCommand cmd = new SqlCommand("sp_getMediciine", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DisseaseId", createobj.DisseaseId);
            cmd.Parameters.AddWithValue("@MedicineName", createobj.MedicineName);
            cmd.Parameters.AddWithValue("@Content", createobj.Comment);
            cmd.Parameters.AddWithValue("@Formula", createobj.Formula);
            cmd.Parameters.AddWithValue("@Mid", createobj.MId);
            // cmd.Parameters.AddWithValue("@Mid", createobj.UpdatedBy);
            con.Open();
            int Result = cmd.ExecuteNonQuery();

            if (Result > 0)
            {
                con.Close();

                return RedirectToAction("GetMedicine");
            }
            else
            {
                con.Close();

                ViewBag.Message = "Record Failed To Insert";
                return View(createobj);
            }
        }
    }
}
