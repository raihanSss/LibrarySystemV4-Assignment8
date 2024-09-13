using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystem.Domain;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace LibrarySystem.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task<bool> SendMail(MailData mailData, RegisterModel registerModel)
        {
            var emailMessage = CreateEmailMessage(mailData, registerModel);
            var result = Send(emailMessage);
            return result;

        }

        private MimeMessage CreateEmailMessage(MailData mailData, RegisterModel registerModel)
        {
            
            MimeMessage emailMessage = new MimeMessage();

           
            MailboxAddress emailFrom = new MailboxAddress(_mailSettings.Name, _mailSettings.EmailId);
            emailMessage.From.Add(emailFrom);

            // Menetapkan alamat penerima
            if (mailData.EmailToIds != null && mailData.EmailToIds.Any())
            {
                foreach (var to in mailData.EmailToIds)
                {
                    MailboxAddress emailTo = new MailboxAddress(to, to);
                    emailMessage.To.Add(emailTo);
                }
            }

            
            emailMessage.Subject = mailData.EmailSubject;

           
            string htmlTemplatePath = Path.Combine(Directory.GetCurrentDirectory(), "Template", "Email_Template", "EmailTemplate.html");
            string htmlTemplate = System.IO.File.ReadAllText(htmlTemplatePath);

            string htmlBody = htmlTemplate
                .Replace("{{Name}}", mailData.EmailToName ?? string.Empty)
                .Replace("{{UserName}}", registerModel.UserName ?? string.Empty)
                .Replace("{{Email}}", registerModel.Email ?? string.Empty)
                .Replace("{{Password}}", "Your password"); 

            
            BodyBuilder emailBodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody,
                TextBody = mailData.EmailBody 
            };

            // Menambahkan lampiran jika ada
            if (mailData.Attachments != null && mailData.Attachments.Any())
            {

                byte[] fileBytes;
                foreach (var attachment in mailData.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    emailBodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }
            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            return emailMessage;
        }


        private bool Send(MimeMessage mailMessage)

        {

            using (var client = new SmtpClient())

            {
                try
                {

                    client.Connect(_mailSettings.Host, _mailSettings.Port, true);

                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(_mailSettings.UserName, _mailSettings.Password);

                    client.Send(mailMessage);

                    return true;

                }

                catch (Exception ex)

                {
                    Console.WriteLine(ex);

                    return false;

                }

                finally

                {

                    client.Disconnect(true);

                    client.Dispose();

                }

            }

        }

    }
}
  

