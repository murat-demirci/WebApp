using System.Net;
using System.Net.Mail;

namespace Mvc.Areas.Identity.Models
{
    public class EmailHelper
    {
        public bool SendEmail(string userEmail, string confirmationLink)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("muratdemircideneme@outlook.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Email Dogrulama";
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = confirmationLink;

            SmtpClient client = new SmtpClient();
            client.Credentials = new NetworkCredential("muratdemircideneme@outlook.com", "mehmeteren06");
            client.Host = "smtp.outlook.com";
            client.Port = 587;
            client.EnableSsl = true;

            try
            {
                client.Send(mailMessage);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }
    }
}
