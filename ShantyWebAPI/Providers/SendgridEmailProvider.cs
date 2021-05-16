using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ShantyWebAPI.Providers
{
    public class SendgridEmailProvider
    {
        //Send email with attachments
        public void Send(string from, string fromUserName, string to, string toUserName, string subject, string plainText, string html)
        {
            var apiKey = Environment.GetEnvironmentVariable("CUSTOMSENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var fromBuilder = new EmailAddress(from, fromUserName);
            var subjectBuilder = subject;
            var toBuilder = new EmailAddress(to, toUserName);
            var plainTextContent = plainText;
            var htmlContent = html;
            var msg = MailHelper.CreateSingleEmail(fromBuilder, toBuilder, subjectBuilder, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        //Send email without attachments
        public void Send(string from, string fromUserName, string to, string toUserName, string subject, string plainText, string html, string attachmentFileName, byte[] attachmentContent)
        {
            var apiKey = Environment.GetEnvironmentVariable("CUSTOMSENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var fromBuilder = new EmailAddress(from, fromUserName);
            var subjectBuilder = subject;
            var toBuilder = new EmailAddress(to, toUserName);
            var plainTextContent = plainText;
            var htmlContent = html;
            var attachment = new Attachment();
            attachment.Content = Convert.ToBase64String(attachmentContent);
            attachment.Filename = attachmentFileName + ".pdf";
            attachment.Type = "pdf";
            attachment.Disposition = "attachment";
            var msg = MailHelper.CreateSingleEmail(fromBuilder, toBuilder, subjectBuilder, plainTextContent, htmlContent);
            msg.AddAttachment(attachment);
            var response = client.SendEmailAsync(msg);
        }
    }
}
