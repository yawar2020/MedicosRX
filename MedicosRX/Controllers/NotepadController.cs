using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using MedicosRX.Models;
using System.Net.Mail;
using System.Net;
using System.Configuration;

namespace MedicosRX.Controllers
{
    public class NotepadController : Controller
    {
        // GET: Notepad
        SqlConnection con = new SqlConnection("data source=AZAM-PC\\SQLEXPRESS;initial catalog=HospitalManagement;integrated security=True");
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString());

        //SqlConnection con = new SqlConnection("Data Source=ISMAIL\\SQLEXPRESS;Initial Catalog=HospitalManagement;User Id=sa;Password=123;");

        public ActionResult GetAllTopicName()
        {
            SqlCommand cmd = new SqlCommand("sp_GetNotepad", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            List<Notepad> obj = new List<Notepad>();

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Notepad objNote = new Models.Notepad();

                objNote.TopicId = Convert.ToInt32(dr[0]);
                objNote.TopicName = dr[1].ToString();
                objNote.TopicContent = dr[2].ToString();
                obj.Add(objNote);
            }
           
            return View(obj);
        }

        public ActionResult Index()
        {
            SqlCommand cmd = new SqlCommand("getHeaderReciept", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            Notepad obj = new Notepad();
            obj.TopicContent = ds.Tables[0].Rows[0]["TitleName"].ToString();
            return View(obj);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(Notepad Obj)
        {
            SqlCommand cmd = new SqlCommand("sp_InsertNotepad", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TopicName", Obj.TopicName);
            cmd.Parameters.AddWithValue("@TopicContent", Obj.TopicContent);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                return RedirectToAction("GetAllTopicName");
            }
            else
            {
                return Content("<script>Alert('Failed To Save Record')</script>");
            }
        }
        [HttpGet]
        public ActionResult Edit(int? id)//sp_Update_Notepad
        {
            Notepad obj = new Notepad();
            SqlCommand cmdNotepad = new SqlCommand("getNotepadbyId", con);
            cmdNotepad.CommandType = CommandType.StoredProcedure;
            cmdNotepad.Parameters.AddWithValue("@TopicId", id);
            SqlDataAdapter da = new SqlDataAdapter(cmdNotepad);
            DataSet ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                obj.TopicId = Convert.ToInt32(dr[0]);
                obj.TopicName = Convert.ToString(dr[1]);
                obj.TopicContent = Convert.ToString(dr[2]);
            }
            return View(obj);
        }
        [ValidateInput(false)]
        public ActionResult Edit(Notepad nobj)//sp_Update_Notepad
        {
            Notepad obj = new Notepad();
            SqlCommand cmdNotepad = new SqlCommand("sp_Update_Notepad", con);
            cmdNotepad.CommandType = CommandType.StoredProcedure;
            cmdNotepad.Parameters.AddWithValue("@TopicId", nobj.TopicId);
            cmdNotepad.Parameters.AddWithValue("@TopicName", nobj.TopicName);
            cmdNotepad.Parameters.AddWithValue("@TopicContent", nobj.TopicContent);
            con.Open();
            int i = cmdNotepad.ExecuteNonQuery();
            if (i > 0)
            {
                return RedirectToAction("GetAllTopicName");
            }
            else
            {
                return Content("<script>Alert('Failed To Update Record')</script>");
            }
        }
        [HttpGet]

        public ActionResult SendNotePadMail(
        string TopicName, string TopicContent )
        {
            SendMail("yawarali17@gmail.com",TopicName,TopicContent);
            return RedirectToAction("GetAllTopicName");
        }

        public void SendMail(string txtto, string txtsub, string txtbody)
        {
            try
            {
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("emailid", "pwd");
                MailMessage msgobj = new MailMessage();
                msgobj.To.Add(txtto);
                msgobj.From = new MailAddress("ajaz2688");
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
    }
}