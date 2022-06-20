using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CVPortal.App_Code
{
    public static class Utility
    {
        public static string UserCode
        {
            get
            {
                return (HttpContext.Current.Session["UserCode"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["UserCode"].ToString())) ? HttpContext.Current.Session["UserCode"].ToString() : null;
            }
            set
            {
                HttpContext.Current.Session["UserCode"] = value;
            }
        }

        public static int UserId
        {
            get
            {
                return (HttpContext.Current.Session["UserId"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["UserId"].ToString())) ? Convert.ToInt32(HttpContext.Current.Session["UserId"].ToString()) : 0;
            }
            set
            {
                HttpContext.Current.Session["UserId"] = value;
            }
        }

        public static string DefaultPassword
        {
            get
            {
                return "test#123";
            }
        }

        #region SendMail

        public static bool SendMail(string EmailTo, string CC, string BCC, string Subject, string Body, string DisplayName, string Attachments, bool IsBodyHtml)
        {
            bool result = true;
            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    string strUserName = ConfigurationManager.AppSettings["EmailUserName"].ToString();
                    string strPassword = ConfigurationManager.AppSettings["EmailPassword"].ToString();
                    string strSmtpHost = ConfigurationManager.AppSettings["EmailSMTPHost"].ToString();
                    int intSmtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["EmailSMTPPort"].ToString());
                    string strMailFrom = ConfigurationManager.AppSettings["EmailMailFrom"].ToString();

                    //Set EnableSsl = false for live site
                    smtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                    smtpClient.Host = strSmtpHost;
                    smtpClient.Port = intSmtpPort;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(strUserName, strPassword);

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = (string.IsNullOrEmpty(DisplayName)) ? new MailAddress(strMailFrom) : new MailAddress(strMailFrom, DisplayName);
                        message.Sender = new MailAddress(strMailFrom);
                        message.Subject = Subject;
                        //Set IsBodyHtml to true means you can send HTML email.
                        message.IsBodyHtml = IsBodyHtml;
                        message.Body = Body;
                        message.To.Add(EmailTo);
                        if (!string.IsNullOrEmpty(CC))
                            foreach (string cc in CC.Split(new char[] { ',' }))
                                message.CC.Add(cc);
                        if (!string.IsNullOrEmpty(BCC))
                            foreach (string bcc in BCC.Split(new char[] { ',' }))
                                message.Bcc.Add(bcc);
                        if (!string.IsNullOrEmpty(Attachments))
                        {
                            foreach (string sAttachment in Attachments.Split(",".ToCharArray()))
                                message.Attachments.Add(new Attachment(sAttachment));
                        }
                        try
                        {
                            smtpClient.Send(message);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
            catch (Exception)
            {
                result = false;
            }
            return result;
        }

        #endregion
    }
}