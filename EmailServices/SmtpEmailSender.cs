using System.Net.Mail;

namespace NewKaratIk.EmailServices
{
    public class SmtpEmailSender: IEmailSender
    {
        private string _host;
        private int _port;

        private string _username;
        private string _password;

        public SmtpEmailSender(string host, int port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient client = new SmtpClient();
            client.Port = _port;
            client.Host = _host;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            client.Credentials = new System.Net.NetworkCredential(_username, _password);
            MailMessage mm = new MailMessage();
            mm = new MailMessage("example@mail.com", email, subject, htmlMessage);
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.IsBodyHtml = true;
            return client.SendMailAsync(mm);
        }
    }
}
