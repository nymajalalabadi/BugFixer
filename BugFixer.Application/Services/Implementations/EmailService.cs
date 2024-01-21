using BugFixer.Application.Services.Interfaces;
using BugFixer.domain.InterFaces;
using System.Net.Mail;

namespace BugFixer.Application.Services.Implementations
{
    public class EmailService : IEmailService
    {
        #region consractor

        private readonly ISiteSettingRepository _siteSettingRepository;

        public EmailService(ISiteSettingRepository siteSettingRepository)
        {
                _siteSettingRepository = siteSettingRepository;
        }

        #endregion

        public async void SendEmail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress("nymasteam@gmail.com", "BugFixer");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            SmtpServer.Port = 587;
            SmtpServer.Credentials = new System.Net.NetworkCredential("nymasteam@gmail.com", "qjymwzfmsycwpzza");
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            //var defaultSiteEmail = await _siteSettingRepository.GetDefaultEmail();

            //MailMessage mail = new MailMessage();
            //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            //mail.From = new MailAddress(defaultSiteEmail.From, defaultSiteEmail.DisplayName);
            //mail.To.Add(to);
            //mail.Subject = subject;
            //mail.Body = body;
            //mail.IsBodyHtml = true;

            //if (defaultSiteEmail.Port != 0)
            //{
            //    SmtpServer.Port = defaultSiteEmail.Port;
            //    SmtpServer.EnableSsl = defaultSiteEmail.EnableSSL;
            //}

            //SmtpServer.Credentials = new System.Net.NetworkCredential(defaultSiteEmail.From, defaultSiteEmail.Password);
            //SmtpServer.Send(mail);
        }

    }
}
