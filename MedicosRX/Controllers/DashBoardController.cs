using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicosRX.Models;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Configuration;

namespace MedicosRX.Controllers
{
    public class DashBoardController : Controller
    {
        // GET: DashBoard
       SqlConnection con = new SqlConnection("data source=AZAM-PC\\SQLEXPRESS;initial catalog=HospitalManagement;integrated security=True");
       // SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString());

        // SqlConnection con = new SqlConnection("Data Source=ISMAIL\\SQLEXPRESS;Initial Catalog=HospitalManagement;User Id=sa;Password=123;");


        public ActionResult Index()
        {

            SqlCommand cmd = new SqlCommand("sp_get_Dashboard_Detail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            DashboardDetail GetDashboardCount = new DashboardDetail();



            if (TempData["CurrentPatient"] == null)
            {
                GetDashboardCount.CountExistingPatient = Convert.ToInt32(ds.Tables[0].Rows[0]["CurrentPatient"]);
            }
            else
            {
                GetDashboardCount.CountExistingPatient = Convert.ToInt32(TempData["CurrentPatient"]);

            }
            if (TempData["CurrentCountAppointment"] == null)
            {
                ViewBag.CountCurrentAppointment = Convert.ToInt32(ds.Tables[1].Rows[0]["Appointment"]);
            }
            else
            {
                ViewBag.CountCurrentAppointment = Convert.ToInt32(TempData["CurrentCountAppointment"]);

            }
            if (TempData["TotalEarning"] == null)
            {
                object value = ds.Tables[2].Rows[0]["TotalEarning"];
                if (value != DBNull.Value)
                {
                    GetDashboardCount.SumEarningPerday = Convert.ToSingle(ds.Tables[2].Rows[0]["TotalEarning"]);

                }
                else
                {
                    GetDashboardCount.SumEarningPerday = Convert.ToSingle("0.00");

                }
            }
            else
            {
                GetDashboardCount.SumEarningPerday = Convert.ToSingle(TempData["TotalEarning"]);
            }
            return View(GetDashboardCount);


        }

        public JsonResult Index1()
        {

            SqlCommand cmd = new SqlCommand("sp_get_Dashboard_Detail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            DashboardDetail GetDashboardCount = new DashboardDetail();
            GetDashboardCount.CountCurrentAppointment = Convert.ToInt32(ds.Tables[1].Rows[0]["Appointment"]);
            GetDashboardCount.CountExistingPatient = Convert.ToInt32(ds.Tables[0].Rows[0]["CurrentPatient"]);
            ViewBag.CountCurrentAppointment = Convert.ToInt32(ds.Tables[1].Rows[0]["Appointment"]);
            GetDashboardCount.SumEarningPerday = Convert.ToInt32(ds.Tables[2].Rows[0]["TotalEarning"]);

            return Json(GetDashboardCount, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExitingPatientChanngeinfo(DoctorNameSpeciality drobj)
        {
            HospitalManagementEntities db = new Models.HospitalManagementEntities();
            DoctorNameSpeciality obj = new DoctorNameSpeciality();

            string fromdate = string.Empty;
            string todate = string.Empty;
            string Drname = string.Empty;
            string flagExitPatient = string.Empty;

            fromdate = Convert.ToString(drobj.fromDate);
            todate = Convert.ToString(drobj.toDate);
            Drname = Convert.ToString(drobj.NameSpecialist);
            flagExitPatient = Convert.ToString(drobj.flag);



            SqlCommand cmd = new SqlCommand("[sp_getExistingPatientsByDrId]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            //Existing Patient Logic when only drname is passed
            if ((Drname != string.Empty) && (fromdate == string.Empty) && (todate == string.Empty) && flagExitPatient == "ExistingPatient")
            {
                SqlCommand cmdOnlyDrName = new SqlCommand("[sp_getExistingPatientsByDrId]", con);
                obj.NameSpecialist = Drname.ToString();
                con.Open();
                string name = obj.NameSpecialist.Split('-')[0];
                string specialist = obj.NameSpecialist.Split('-')[1];
                obj.DrId = Convert.ToInt32(db.Doctors.Where(s => s.Name == name && s.Specialist == specialist).Select(s => s.DoctorId).FirstOrDefault());
                cmdOnlyDrName.CommandType = CommandType.StoredProcedure;
                cmdOnlyDrName.Parameters.AddWithValue("@DrId", obj.DrId);
                cmdOnlyDrName.Parameters.AddWithValue("@fromdate", obj.fromDate);
                cmdOnlyDrName.Parameters.AddWithValue("@todate", obj.toDate);
                SqlDataAdapter daOnlyDrName = new SqlDataAdapter(cmdOnlyDrName);
                DataSet dsOnlyDrName = new DataSet();
                daOnlyDrName.Fill(dsOnlyDrName);
                int ExistingCountPatient = Convert.ToInt32(dsOnlyDrName.Tables[0].Rows[0]["CurrentPatient"]);
                TempData["CurrentPatient"] = ExistingCountPatient;
            }
            //Passed only from and todate
            else if ((Drname == string.Empty) && (fromdate != string.Empty) && (todate != string.Empty) && flagExitPatient == "ExistingPatient")
            {
                SqlCommand cmdfromToDate = new SqlCommand("[sp_getExistingPatientsByDrId]", con);
                obj.NameSpecialist = Drname.ToString();
                con.Open();
                string name = obj.NameSpecialist.Split('-')[0];
                string specialist = obj.NameSpecialist.Split('-')[1];
                obj.DrId = Convert.ToInt32(db.Doctors.Where(s => s.Name == name && s.Specialist == specialist).Select(s => s.DoctorId).FirstOrDefault());
                cmdfromToDate.CommandType = CommandType.StoredProcedure;
                cmdfromToDate.Parameters.AddWithValue("@DrId", obj.DrId);
                cmdfromToDate.Parameters.AddWithValue("@fromdate", obj.fromDate);
                cmdfromToDate.Parameters.AddWithValue("@todate", obj.toDate);
                SqlDataAdapter dafromToDate = new SqlDataAdapter(cmd);
                DataSet dsfromToDate = new DataSet();
                dafromToDate.Fill(dsfromToDate);
                int ExistingCountPatient = Convert.ToInt32(dsfromToDate.Tables[0].Rows[0]["CurrentPatient"]);
                TempData["CurrentPatient"] = ExistingCountPatient;
            }
            //Passed all the Values
            else if ((Drname != string.Empty) && (fromdate != string.Empty) && (todate != string.Empty) && flagExitPatient == "ExistingPatient")
            {
                SqlCommand cmdAllValues = new SqlCommand("[sp_getExistingPatientsByDrId]", con);
                obj.NameSpecialist = Drname.ToString();
                con.Open();
                string name = obj.NameSpecialist.Split('-')[0];
                string specialist = obj.NameSpecialist.Split('-')[1];
                obj.DrId = Convert.ToInt32(db.Doctors.Where(s => s.Name == name && s.Specialist == specialist).Select(s => s.DoctorId).FirstOrDefault());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DrId", obj.DrId);
                cmd.Parameters.AddWithValue("@fromdate", obj.fromDate);
                cmd.Parameters.AddWithValue("@todate", obj.toDate);
                SqlDataAdapter daAllValues = new SqlDataAdapter(cmd);
                DataSet dsAllValues = new DataSet();
                daAllValues.Fill(dsAllValues);
                int ExistingCountPatient = Convert.ToInt32(dsAllValues.Tables[0].Rows[0]["CurrentPatient"]);
                TempData["CurrentPatient"] = ExistingCountPatient;
            }
            DashboardDetail GetDashboardCount = new DashboardDetail();
            GetDashboardCount.CountExistingPatient = (int)TempData["CurrentPatient"];
            //   ViewBag.CountExistingPatient = (int)TempData["CurrentPatient"]
            return Json(GetDashboardCount, JsonRequestBehavior.AllowGet);

        }





        [HttpPost]
        public ActionResult GetAppointCount(FormCollection frm)
        {
            HospitalManagementEntities db = new Models.HospitalManagementEntities();
            DoctorNameSpeciality obj = new DoctorNameSpeciality();

            string fromdate = string.Empty;
            string todate = string.Empty;
            string Drname = string.Empty;
            string flagExitPatient = string.Empty;
            string flagCurrentEarning = string.Empty;

            if (frm["fromappDate"] != string.Empty)
            {
                fromdate = Convert.ToString(frm["fromappDate"]);
            }
            if (frm["toappDate"] != string.Empty)
            {
                todate = Convert.ToString(frm["toappDate"]);
            }
            if (frm["DrNameAndSpeciality"] != string.Empty)
            {
                Drname = Convert.ToString(frm["DrNameAndSpeciality"]);
            }
            if (frm["hfAppointments"] != string.Empty)
            {
                flagExitPatient = Convert.ToString(frm["hfAppointments"]);
            }
            if (frm["CurrentOPEarning"] != string.Empty)
            {
                flagCurrentEarning = Convert.ToString(frm["hfTotalAccounts"]);
            }


            //Current Appointment Details By DrName!
            if ((Drname != string.Empty) && (fromdate == string.Empty) && (todate == string.Empty) && flagExitPatient != "ExistingPatient")
            {
                SqlCommand cmd = new SqlCommand("[sp_getCurrentAppointmentByDrId]", con);
                obj.NameSpecialist = frm["DrNameAndSpeciality"].ToString();
                con.Open();
                string name = obj.NameSpecialist.Split('-')[0];
                string specialist = obj.NameSpecialist.Split('-')[1];
                obj.DrId = Convert.ToInt32(db.Doctors.Where(s => s.Name == name && s.Specialist == specialist).Select(s => s.DoctorId).FirstOrDefault());
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DrId", obj.DrId);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                int CurrentCountAppointment = Convert.ToInt32(ds.Tables[0].Rows[0]["CurrentAppointmentCount"]);
                TempData["CurrentCountAppointment"] = CurrentCountAppointment;
            }
            else if ((Drname != string.Empty) && (fromdate == string.Empty) && (todate == string.Empty) && flagExitPatient == "ExistingPatient")
            {
                SqlCommand cmdOnlyDrName = new SqlCommand("[sp_getExistingPatientsByDrId]", con);
                obj.NameSpecialist = Drname.ToString();
                con.Open();
                string name = obj.NameSpecialist.Split('-')[0];
                string specialist = obj.NameSpecialist.Split('-')[1];
                obj.DrId = Convert.ToInt32(db.Doctors.Where(s => s.Name == name && s.Specialist == specialist).Select(s => s.DoctorId).FirstOrDefault());
                cmdOnlyDrName.CommandType = CommandType.StoredProcedure;
                cmdOnlyDrName.Parameters.AddWithValue("@DrId", obj.DrId);
                cmdOnlyDrName.Parameters.AddWithValue("@fromdate", obj.fromDate);
                cmdOnlyDrName.Parameters.AddWithValue("@todate", obj.toDate);
                SqlDataAdapter daOnlyDrName = new SqlDataAdapter(cmdOnlyDrName);
                DataSet dsOnlyDrName = new DataSet();
                daOnlyDrName.Fill(dsOnlyDrName);
                int ExistingCountPatient = Convert.ToInt32(dsOnlyDrName.Tables[0].Rows[0]["CurrentPatient"]);
                TempData["CurrentPatient"] = ExistingCountPatient;
            }
            //Passed only from and todate
            else if ((Drname == string.Empty) && (fromdate != string.Empty) && (todate != string.Empty) && flagExitPatient == "ExistingPatient")
            {
                SqlCommand cmdfromToDate = new SqlCommand("sp_getExistingPatientsByDrId", con);

                con.Open();

                cmdfromToDate.CommandType = CommandType.StoredProcedure;
                obj.DrId = Convert.ToInt32(db.Doctors.Where(s => s.Name == "").Select(s => s.DoctorId).FirstOrDefault());


                if (obj.DrId == 0)
                {
                    obj.DrId = null;
                }
                cmdfromToDate.Parameters.AddWithValue("@DrId", obj.DrId);
                cmdfromToDate.Parameters.AddWithValue("@fromdate", Convert.ToString(fromdate));
                cmdfromToDate.Parameters.AddWithValue("@todate", Convert.ToString(todate));
                SqlDataAdapter dafromToDate = new SqlDataAdapter(cmdfromToDate);
                DataSet dsfromToDate = new DataSet();
                dafromToDate.Fill(dsfromToDate);
                int ExistingCountPatient = Convert.ToInt32(dsfromToDate.Tables[0].Rows[0]["CurrentPatient"]);
                TempData["CurrentPatient"] = ExistingCountPatient;
            }
            //Passed all the Values
            else if ((Drname != string.Empty) && (fromdate != string.Empty) && (todate != string.Empty) && flagExitPatient == "ExistingPatient")
            {
                SqlCommand cmdAllValues = new SqlCommand("[sp_getExistingPatientsByDrId]", con);
                obj.NameSpecialist = Drname.ToString();
                con.Open();
                string name = obj.NameSpecialist.Split('-')[0];
                string specialist = obj.NameSpecialist.Split('-')[1];
                obj.DrId = Convert.ToInt32(db.Doctors.Where(s => s.Name == name && s.Specialist == specialist).Select(s => s.DoctorId).FirstOrDefault());
                cmdAllValues.CommandType = CommandType.StoredProcedure;
                cmdAllValues.Parameters.AddWithValue("@DrId", obj.DrId);
                cmdAllValues.Parameters.AddWithValue("@fromdate", fromdate);
                cmdAllValues.Parameters.AddWithValue("@todate", todate);
                SqlDataAdapter daAllValues = new SqlDataAdapter(cmdAllValues);
                DataSet dsAllValues = new DataSet();
                daAllValues.Fill(dsAllValues);
                int ExistingCountPatient = Convert.ToInt32(dsAllValues.Tables[0].Rows[0]["CurrentPatient"]);
                TempData["CurrentPatient"] = ExistingCountPatient;
            }
            //DashboardDetail GetDashboardCount = new DashboardDetail();
            //GetDashboardCount.CountExistingPatient = (int)TempData["CurrentPatient"];
            // Billing Information based on Doctor name for current Date

            else if ((Drname != string.Empty) && (fromdate == string.Empty) && (todate == string.Empty) && flagCurrentEarning == "CurrentOPEarning")
            {
                SqlCommand cmdOnlyDrName = new SqlCommand("[sp_getCurrentAccountDetailsByDrId]", con);
                obj.NameSpecialist = Drname.ToString();
                con.Open();
                string name = obj.NameSpecialist.Split('-')[0];
                string specialist = obj.NameSpecialist.Split('-')[1];
                obj.DrId = Convert.ToInt32(db.Doctors.Where(s => s.Name == name && s.Specialist == specialist).Select(s => s.DoctorId).FirstOrDefault());
                cmdOnlyDrName.CommandType = CommandType.StoredProcedure;
                cmdOnlyDrName.Parameters.AddWithValue("@DrId", obj.DrId);
                cmdOnlyDrName.Parameters.AddWithValue("@fromdate", fromdate);
                cmdOnlyDrName.Parameters.AddWithValue("@todate", todate);
                SqlDataAdapter daOnlyDrName = new SqlDataAdapter(cmdOnlyDrName);
                DataSet dsOnlyDrName = new DataSet();
                daOnlyDrName.Fill(dsOnlyDrName);
                int CurrentEarningOP = Convert.ToInt32(dsOnlyDrName.Tables[0].Rows[0]["TotalEarning"]);
                TempData["TotalEarning"] = CurrentEarningOP;
            }
            //Passed only from and todate
            else if ((Drname == string.Empty) && (fromdate != string.Empty) && (todate != string.Empty) && flagCurrentEarning == "CurrentOPEarning")
            {
                SqlCommand cmdfromToDate = new SqlCommand("sp_getCurrentAccountDetailsByDrId", con);

                con.Open();

                cmdfromToDate.CommandType = CommandType.StoredProcedure;
                obj.DrId = Convert.ToInt32(db.Doctors.Where(s => s.Name == "").Select(s => s.DoctorId).FirstOrDefault());


                if (obj.DrId == 0)
                {
                    obj.DrId = null;
                }
                cmdfromToDate.Parameters.AddWithValue("@DrId", obj.DrId);
                cmdfromToDate.Parameters.AddWithValue("@fromdate", Convert.ToString(fromdate));
                cmdfromToDate.Parameters.AddWithValue("@todate", Convert.ToString(todate));
                SqlDataAdapter dafromToDate = new SqlDataAdapter(cmdfromToDate);
                DataSet dsfromToDate = new DataSet();
                dafromToDate.Fill(dsfromToDate);
                int CurrentEarningOP = Convert.ToInt32(dsfromToDate.Tables[0].Rows[0]["TotalEarning"]);
                TempData["TotalEarning"] = CurrentEarningOP;
            }
            //Passed all the Values
            else if ((Drname != string.Empty) && (fromdate != string.Empty) && (todate != string.Empty) && flagCurrentEarning == "CurrentOPEarning")
            {
                SqlCommand cmdAllValues = new SqlCommand("[sp_getCurrentAccountDetailsByDrId]", con);
                obj.NameSpecialist = Drname.ToString();
                con.Open();
                string name = obj.NameSpecialist.Split('-')[0];
                string specialist = obj.NameSpecialist.Split('-')[1];
                obj.DrId = Convert.ToInt32(db.Doctors.Where(s => s.Name == name && s.Specialist == specialist).Select(s => s.DoctorId).FirstOrDefault());
                cmdAllValues.CommandType = CommandType.StoredProcedure;
                cmdAllValues.Parameters.AddWithValue("@DrId", obj.DrId);
                cmdAllValues.Parameters.AddWithValue("@fromdate", fromdate);
                cmdAllValues.Parameters.AddWithValue("@todate", todate);
                SqlDataAdapter daAllValues = new SqlDataAdapter(cmdAllValues);
                DataSet dsAllValues = new DataSet();
                daAllValues.Fill(dsAllValues);
                int CurrentEarningOP = Convert.ToInt32(dsAllValues.Tables[0].Rows[0]["TotalEarning"]);
                TempData["TotalEarning"] = CurrentEarningOP;
            }


            return RedirectToAction("Index");
        }
        public JsonResult GetDoctorAndPatientChart()
        {

            var DoctorAndPatientChart = "";
            DashboardDetail GetDashboardCount = new DashboardDetail();

            SqlCommand cmdDRAndPat = new SqlCommand("sp_get_CountPatientByDrId", con);
            cmdDRAndPat.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter daDRAndPat = new SqlDataAdapter(cmdDRAndPat);
            DataSet dsDRAndPat = new DataSet();
            daDRAndPat.Fill(dsDRAndPat);

            List<PatientAndDoctorChart> GetPatientAndDoctorChart = new List<PatientAndDoctorChart>();

            foreach (DataRow dr in dsDRAndPat.Tables[0].Rows)
            {
                GetPatientAndDoctorChart.Add(new PatientAndDoctorChart
                {

                    DoctorName = Convert.ToString(dr["Name"]),
                    PatientCount = Convert.ToInt32(dr["PatientCount"]),


                });
            }

            GetDashboardCount.PatientAndDoctorCharts = GetPatientAndDoctorChart;

            return Json(GetPatientAndDoctorChart, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCountAppointmentByDRId(DateTime fromappDate, DateTime toappDate, int? doctorId)
        {


            SqlCommand cmd = new SqlCommand("sp_get_Dashboard_GetCountAppointmentByDRId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@fromdate", fromappDate.ToShortDateString());
            cmd.Parameters.AddWithValue("@todate", toappDate.ToShortDateString());
            cmd.Parameters.AddWithValue("@Drid", doctorId);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            DashboardDetail GetDashboardCount = new DashboardDetail();
            GetDashboardCount.CountCurrentAppointment = Convert.ToInt32(ds.Tables[0].Rows[0]["Appointment"]);


            return View();
        }

        [HttpGet]
        public ActionResult DoctorDashBoard()
        {

            SqlCommand cmd = new SqlCommand("sp_getAppointmentByDrId", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DRID", Convert.ToInt32(Session["UserTypeId"]));
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
    }

}
