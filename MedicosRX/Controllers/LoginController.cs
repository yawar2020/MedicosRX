using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MedicosRX.Models;
using System.Web.Security;
using System.Data.SqlClient;
using System.Data;

using System.Net;
using System.Net.Mail;
using System.Text;
using System.Configuration;

namespace MedicosRX.Controllers
{

    public class LoginController : Controller
    {
        // GET: Login
        HospitalManagementEntities db = new Models.HospitalManagementEntities();
        SqlConnection con = new SqlConnection("data source=AZAM-PC\\SQLEXPRESS;initial catalog=HospitalManagement;integrated security=True");
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString());

       //SqlConnection con = new SqlConnection("Data Source=ISMAIL\\SQLEXPRESS;Initial Catalog=HospitalManagement;User Id=sa;Password=123;");





        public ActionResult Login()
        {
            ViewBag.RoleType = new SelectList(db.Roles, "RoleId", "RoleName");

            return View();
        }

        [HttpPost]
        public ActionResult Login(sp_getLoginUsers_Result obj)
        {
            ViewBag.RoleType = new SelectList(db.Roles, "RoleId", "RoleName");
            ViewBag.UserTypeId = new SelectList(db.Doctors, "DoctorId", "Name");
            //var loginobj = db.sp_getLoginUsers(obj.UserName, obj.RoleId, obj.Password).SingleOrDefault();
            SqlCommand cmd = new SqlCommand("sp_getLoginUsers", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserName", obj.UserName);
            cmd.Parameters.AddWithValue("@RoleId", obj.RoleId);
            cmd.Parameters.AddWithValue("@Password", obj.Password);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds.Tables[0].Rows.Count>0 && Convert.ToBoolean(ds.Tables[0].Rows[0]["IsActive"]) == true)
            {
                FormsAuthentication.SetAuthCookie(Convert.ToString(ds.Tables[0].Rows[0]["UserName"]), false);
                Session["UserId"] = Convert.ToInt32(ds.Tables[0].Rows[0]["UserId"]);
                Session["UserName"] = Convert.ToString(ds.Tables[0].Rows[0]["UserName"]);
                Session["RoleId"] = Convert.ToInt32(ds.Tables[0].Rows[0]["RoleId"]);
                Session["UserTypeId"] = Convert.ToInt32(ds.Tables[0].Rows[0]["UserTypeId"]);
                string RedirectPage = string.Empty;
                if (Convert.ToInt32(ds.Tables[0].Rows[0]["RoleId"])==1)
                {
                      RedirectPage = "Index";
                }
                else if (Convert.ToInt32(ds.Tables[0].Rows[0]["RoleId"]) == 2)
                {
                    RedirectPage = "DoctorDashBoard";   
                }
                return RedirectToAction(RedirectPage, "DashBoard");

            }
            else
            {
                ViewBag.Message = "Wrong userName and Password";
                return View();

            }

        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return Redirect("~/Login/Login");
        }
        [HttpGet]
        public ActionResult ViewLogin()
        {
            SqlCommand cmd = new SqlCommand("sp_get_ViewLoginDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            List<LoginDetail> GetLoginDetail = new List<LoginDetail>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GetLoginDetail.Add(new LoginDetail
                {

                    UserId = Convert.ToInt32(dr["UserId"]),
                    UserName = Convert.ToString(dr["UserName"]),
                    Password = Convert.ToString(dr["Password"]),
                    RoleName = Convert.ToString(dr["RoleName"]),
                    IsActive = Convert.ToBoolean(dr["IsActive"]),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"]),
                    UpdatedDate = Convert.ToDateTime(dr["UpdatedDate"])


                });
            }


            return View(GetLoginDetail.ToList()); 



        }


        [HttpGet]
        public ActionResult CreateLogin()
        {
            ViewBag.UserTypeId = new SelectList(db.Doctors, "DoctorId", "Name");


            SqlCommand cmd = new SqlCommand("sp_get_ROLES", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            List<MedicosRX.Models.Roles> GetRoles = new List<MedicosRX.Models.Roles>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GetRoles.Add(new MedicosRX.Models.Roles
                {

                    RoleId = Convert.ToInt32(dr["RoleId"]),
                    RoleName = Convert.ToString(dr["RoleName"]),


                });
                ViewBag.Roles = new SelectList(GetRoles.ToList(), "RoleId", "RoleName");

            }
            return View();
        }
        [HttpPost]
        public ActionResult CreateLogin(LoginDetail lst)
        {
            ViewBag.UserTypeId = new SelectList(db.Doctors, "DoctorId", "Name");
            SqlCommand cmd = new SqlCommand("sp_get_ROLES", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            List<MedicosRX.Models.Roles> GetRoles = new List<MedicosRX.Models.Roles>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GetRoles.Add(new MedicosRX.Models.Roles
                { 
                    RoleId = Convert.ToInt32(dr["RoleId"]),
                    RoleName = Convert.ToString(dr["RoleName"]),
                });
            }
                ViewBag.Roles = new SelectList(GetRoles.ToList(), "RoleId", "RoleName");
            SqlCommand cmd1 = new SqlCommand("sp_insert_LoginUsers", con);
            con.Open();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@UserName", lst.UserName);
            cmd1.Parameters.AddWithValue("@Password", lst.Password);
            cmd1.Parameters.AddWithValue("@RoleId", Convert.ToInt32(lst.RoleId));
            cmd1.Parameters.AddWithValue("@UserTypeId", Convert.ToInt32(lst.UserTypeId));
            cmd1.Parameters.AddWithValue("@IsActive",Convert.ToBoolean(lst.IsActive));
            cmd1.Parameters.AddWithValue("@EmailId",  (lst.EmailId));
            cmd1.Parameters.AddWithValue("@CreatedBy", 1);
            cmd1.Parameters.AddWithValue("@Updatedby", 1);
            Int32 i = Convert.ToInt32(cmd1.ExecuteNonQuery());

            if (i > 0)
            {
                string Sub = "Login Crediantial";
                string Body = "UserName:" + lst.UserName + " Password:" + lst.Password;
                SendMail(lst.EmailId,Sub,Body);
                con.Close();
                return RedirectToAction("ViewLogin");
            }
            else
            {
                ViewBag.Message = "Record Failed To Insert"; con.Close();
                return View();
            }
           
          
        }
        [HttpGet]
        public ActionResult EditLogin(int? Id)
        {
            ViewBag.UserTypeId = new SelectList(db.Doctors, "DoctorId", "Name");
            SqlCommand cmd = new SqlCommand("sp_get_ROLES", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            List<MedicosRX.Models.Roles> GetRoles = new List<MedicosRX.Models.Roles>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GetRoles.Add(new MedicosRX.Models.Roles
                {
                    RoleId = Convert.ToInt32(dr["RoleId"]),
                    RoleName = Convert.ToString(dr["RoleName"]),
                });
                ViewBag.Roles = new SelectList(GetRoles.ToList(), "RoleId", "RoleName");

            }

            SqlCommand cmd1 = new SqlCommand("sp_Update_LoginUsers", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@UserId", Id);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataSet ds1 = new DataSet();
            da1.Fill(ds1);
            LoginDetail lgReadDetails = new Models.LoginDetail();

            foreach (DataRow dr1 in ds1.Tables[0].Rows)
            {
                lgReadDetails.UserId = Convert.ToInt32(dr1["UserId"]);
                lgReadDetails.UserName = Convert.ToString(dr1["UserName"]);
                lgReadDetails.EmailId = Convert.ToString(dr1["EmailId"]);
                lgReadDetails.RoleId = Convert.ToInt32(dr1["RoleId"]);
                lgReadDetails.Password = Convert.ToString(dr1["Password"]);
                lgReadDetails.IsActive = Convert.ToBoolean(dr1["IsActive"]);         
            }
            Session["lgReadDetails"] = lgReadDetails;
            return View(lgReadDetails);
        }
        [HttpPost]
        public ActionResult EditLogin(LoginDetail lst)
        {
            ViewBag.UserTypeId = new SelectList(db.Doctors, "DoctorId", "Name");
            SqlCommand cmd = new SqlCommand("sp_get_ROLES", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);


            List<MedicosRX.Models.Roles> GetRoles = new List<MedicosRX.Models.Roles>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                GetRoles.Add(new MedicosRX.Models.Roles
                {

                    RoleId = Convert.ToInt32(dr["RoleId"]),
                    RoleName = Convert.ToString(dr["RoleName"]),


                });
            }
           string UpdateStatus=CheckFiledsUpdated(lst);
            ViewBag.Roles = new SelectList(GetRoles.ToList(), "RoleId", "RoleName");
            SqlCommand cmd1 = new SqlCommand("sp_UpdatebyUserId_LoginUsers", con);
            con.Open();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@userId", lst.UserId);
            cmd1.Parameters.AddWithValue("@UserName", lst.UserName);
            cmd1.Parameters.AddWithValue("@Password", lst.Password);
            cmd1.Parameters.AddWithValue("@EmailId", (lst.EmailId));
            cmd1.Parameters.AddWithValue("@RoleId", Convert.ToInt32(lst.RoleId));
            cmd1.Parameters.AddWithValue("@UserTypeId", Convert.ToInt32(lst.UserTypeId));
            cmd1.Parameters.AddWithValue("@IsActive", Convert.ToBoolean(lst.IsActive));
            cmd1.Parameters.AddWithValue("@Updatedby", 1);
            Int32 i = Convert.ToInt32(cmd1.ExecuteNonQuery());

            if (i > 0)
            {
                string Sub = "Login Crediantial";
                string Body = UpdateStatus + " has been Updated";
                SendMail(lst.EmailId, Sub, Body);
                con.Close();
                return RedirectToAction("ViewLogin");
            }
            else
            {
                ViewBag.Message = "Record Failed To Insert";
                con.Close();

                return View();  
            }


        }

        public void SendMail(string txtto,string txtsub,string txtbody)
        {
           try {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("email", "pwd");
            MailMessage msgobj = new MailMessage();
                msgobj.To.Add(txtto);
                msgobj.From = new MailAddress("ajaz2688@gmail.com");
                msgobj.Subject = txtsub;
                msgobj.Body = txtbody;
                client.Send(msgobj);
                Response.Write("msg has been sent successfully");
        }
            catch (Exception ex)
            {
                //result = "problem occurred";
                Response.Write("Exception in sendEmail:" + ex.Message);
            }
        }
        public string CheckFiledsUpdated(LoginDetail lgCheckDetails)
        {

            LoginDetail oldLogindetails = new Models.LoginDetail();
           StringBuilder UpdatedStaus = new StringBuilder();
            oldLogindetails  = (LoginDetail)Session["lgReadDetails"];
             
                if (oldLogindetails.UserName != lgCheckDetails.UserName)
                {
                    UpdatedStaus.Append("Updated UserName" + lgCheckDetails.UserName+"\n");
                }
                if (oldLogindetails.EmailId != lgCheckDetails.EmailId)
                {
                    UpdatedStaus.Append("Your Updated Email Id:" + lgCheckDetails.EmailId+"\n");
                }
                if (oldLogindetails.RoleId != lgCheckDetails.RoleId)
                {
                    UpdatedStaus.Append("Your Role has been Changed to:" + lgCheckDetails.RoleId+"\n");
                }
                if (oldLogindetails.Password != lgCheckDetails.Password)
                {
                    UpdatedStaus.Append("Your Updated Password is:" + lgCheckDetails.Password + "\n");
                }
                if (Convert.ToBoolean(oldLogindetails.IsActive) != lgCheckDetails.IsActive)
                {
                    UpdatedStaus.Append("Your Acccount is Blocked Please Contact to Administrator");
                }

            return UpdatedStaus.ToString();

        }

    }
}