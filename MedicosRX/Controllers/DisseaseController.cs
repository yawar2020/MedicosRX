using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicosRX.Models;
namespace MedicosRX.Controllers
{
  
    public class DisseaseController : Controller
    {
        // GET: Dissease
        HospitalManagementEntities db = new Models.HospitalManagementEntities();

        public ActionResult GetDisseaseDetails()
        {
            
            return View(db.sp_get_Dissease());
        }
        [HttpGet]
        public ActionResult CreateDisseaseDetail()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CreateDisseaseDetail(sp_get_Dissease_Result obj)
        {
            sp_get_Dissease_Result createobj = new Models.sp_get_Dissease_Result();
            createobj.DisseaseName = obj.DisseaseName;
            createobj.Description = obj.Description;
            createobj.CreatedBy =Convert.ToInt32(Session["UserId"]);
            createobj.UpdatedBy = Convert.ToInt32(Session["UserId"]);
            db.sp_Insert_Dissease(createobj.DisseaseName, createobj.Description, createobj.CreatedBy, createobj.UpdatedBy);
            int Result = 1;
            if (Result > 0)
            {
                return RedirectToAction("GetDisseaseDetails");
            }
            else
            {
                ViewBag.Message = "Record Failed To Insert";
                return View(createobj);
            }
        }


        [HttpGet]
        public ActionResult EditDisseaseDetail(int? DiseasesId)
        {

            var dsobj = db.Diseasses.Find(DiseasesId);

            return View(dsobj);
        }
        [HttpPost]
        public ActionResult EditDisseaseDetail(Diseass obj)
        {
            //  db.sp_Update_Dissease_ById(obj.DiseasesId,obj.DisseaseName, obj.Description, obj.CreatedBy, Convert.ToInt32(Session["UserId"]));
            //Diseass dobj = db.Diseasses.Find(obj.DiseasesId);
            //dobj.DisseaseName=
            obj.UpdatedBy = 1;
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
          
            int Result = db.SaveChanges();
            if (Result > 0)
            {
                return RedirectToAction("GetDisseaseDetails");
            }
            else
            {
                ViewBag.Message = "Record Failed To Update";
                return View(obj);
            }
        }

        [HttpGet]
        public ActionResult DeleteDisseaseDetail(int? DiseasesId)
        {
            Diseass dsobj = db.Diseasses.Find(DiseasesId);
            
            return View(dsobj);
        }
        [HttpPost]
        public ActionResult DeleteDisseaseDetail(sp_get_Dissease_Result obj)
        {
            Diseass dsobj = db.Diseasses.Find(obj.DiseasesId);
            db.Diseasses.Remove(dsobj);
            db.SaveChanges();
            return RedirectToAction("GetDisseaseDetails");
        }
    }
}