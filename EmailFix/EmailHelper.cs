using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EmailFix
{
    public static class EmailHelper
    {
        public static bool SendEmail(string fromName, string fromAddress, string toName, string toAddress, string subject, string message )
        {
            //Thread.SpinWait(100000);
            //Console.WriteLine(" ... ok ");
            //return true;
            try
            {
                var mailDefinition = new MailDefinition
                {
                    From = string.Format("{0} <{1}>", fromName, fromAddress)
                };
                var mailTo = !string.IsNullOrEmpty(toName) ? string.Format("{0} <{1}>", toName, toAddress) : toAddress;
                
                var mailMessage = mailDefinition.CreateMailMessage(mailTo, null, message, new Control());
                mailMessage.From = new MailAddress(fromAddress, fromName);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = SubjectFormat(subject);
                var smtpClient = new SmtpClient()
                {
                    Port = 25,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Host = "EX-HTCA1-HO.BHF.ADS",
                };
                //send email
                smtpClient.Send(mailMessage);
                Console.WriteLine($" ... ok, sent to {mailMessage.To[0].Address} ");
                return true;
            }
            catch (Exception ex)
            {
             //   Log.Error(string.Format("Error sending email: to: {0}, subject: {1}, error: {2}", toAddress, subject, ex.Message), MethodBase.GetCurrentMethod().ToString());
                Console.WriteLine(ex.Message);
                if(ex.InnerException != null) Console.WriteLine(ex.InnerException.Message);
                return false;
            }
        }

        

        public static string ReadFromFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                string fileText = File.ReadAllText(filePath);

                return fileText;
            }

            return string.Empty;
        }

        public static string SubjectFormat(string subject)
        {
            return subject.Replace("\r", " ").Replace("\n", " ");
        }

        public static string BuildEmailBody(string emailBody, ListDictionary replacements)
        {
            var mailDefinition = new MailDefinition();
            string fakeEmail = "info@comms.bhf.org.uk";
            mailDefinition.From = fakeEmail;
            var mailMessage = mailDefinition.CreateMailMessage(fakeEmail, replacements, emailBody, new Control());
            return mailMessage.Body;
        }
    }
}
