using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Class1
/// </summary>
public class clsMail
{
    public static SmtpClient MailConfiguration()
    {
        var portNumber = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
        SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["Host"], portNumber);
        client.UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]); ;
        System.Net.NetworkCredential basicCredential = new
        System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"]);
        client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
        client.Credentials = basicCredential;
        return client;
    }
    public static MailMessage GetMailBody(string to, string subject, string name, string email, string phone, string message, string emailTemplatePath)
    {
        var from = ConfigurationManager.AppSettings["EmailFrom"];

        MailMessage mailMessage = new MailMessage(from, to);

        string mailbody = string.Empty;

        using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath(emailTemplatePath)))
        {
            mailbody = reader.ReadToEnd();
        }
        mailbody = mailbody.Replace("{name}", name);
        mailbody = mailbody.Replace("{email}", email);
        mailbody = mailbody.Replace("{message}", message);
       // mailbody = mailbody.Replace("{adminName}", adminName);
        mailbody = mailbody.Replace("{phone}", phone);

        mailMessage.Subject = subject;
        mailMessage.Body = mailbody;
        mailMessage.BodyEncoding = Encoding.UTF8;
        mailMessage.IsBodyHtml = true;
        return mailMessage;
    }
    public static string SendMail(string name, string email, string phone, string description, string adminSubject, string userSubject, string adminTemplate, string userTemplate)
    {
        try
        {
            #region Mail Configuration
            var client = MailConfiguration();
            #endregion

            #region for sending mail to admin address
            var adminMailMessage = GetMailBody(ConfigurationManager.AppSettings["AdminEmail"], adminSubject, name, email, phone, description, adminTemplate);
            client.Send(adminMailMessage);
            #endregion

            #region for sending mail to visitor address
            var userMailMessage = GetMailBody(email, userSubject, name, email, phone, "", userTemplate);

            foreach (var mailadress in userMailMessage.To)
            {
                if (mailadress.Address == ConfigurationManager.AppSettings["EmailFrom"])
                {
                    userMailMessage.To.Remove(mailadress);
                    break;
                }
            }

            userMailMessage.To.Add(new MailAddress(email));
            client.Send(userMailMessage);
            #endregion for sending mail to visitor address


            return "Success";
        }

        catch (Exception ex)
        {
            return ex.Message + " Inner exp: " + ex.InnerException;
        }
    }

}