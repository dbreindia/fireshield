using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class dbreService : System.Web.Services.WebService
{
   
        [WebMethod]
        public String PostContactDetails(string name, string email, string message,string phone)
        {

            #region Send Mail 
            var resoonse = clsMail.SendMail(name, email, phone, message, "New lead received", "Enquiry submitted successfully", "~/EmailTemplate/contactAdminEmailTemplate.html", "~/EmailTemplate/contactUserEmailTemplate.html");
            return resoonse;
            #endregion

        }
}

