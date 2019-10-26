using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
namespace MedicosRX.Models
{
    [AttributeUsage(
     AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class CheckSessionTimeOut : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Session != null)
            {
                if (context.Session.IsNewSession)
                {
                    string sessionCookie = context.Request.Headers["UserName"];

                    if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        FormsAuthentication.SignOut();
                        string redirectTo = "~/Login/Login";
                        if (!string.IsNullOrEmpty(context.Request.RawUrl))
                        {
                            redirectTo = string.Format("~/Login/Login?ReturnUrl={0}",
                                HttpUtility.UrlEncode(context.Request.RawUrl));
                        }

                    }
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
 