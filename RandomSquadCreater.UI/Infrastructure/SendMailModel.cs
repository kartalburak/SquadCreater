using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace RandomSquadCreater.UI.Infrastructure
{
    public class SendMailModel
    {


        public bool SendMails(string from,string password,string to,string subject,string message)
        {


            MailAddress mailAddress = new MailAddress(from);

            SmtpClient client = new SmtpClient
            {
                Port = Convert.ToInt32(587),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                EnableSsl = true,
                Timeout = 10000,
                Credentials = new NetworkCredential(from, password)
            };

            MailMessage mail = new MailMessage(from: mailAddress,
                to: new MailAddress(to))
            {
                Subject = subject,
                BodyEncoding = Encoding.UTF8,
                Body = message,
                IsBodyHtml = true,
                DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
            };
            //if (FlowInput.AttachmentFilePaths.Any())
            //{
            //    mail.Attachments.Add(new Attachment(FlowInput.AttachmentFilePaths.First()));
            //}
            try
            {
                client.Send(mail);
                return true;
                //FlowInput.Reconciliation.EmailStatusTypeEnum = EmailStatusType.Success;
                //FlowInput.Reconciliation.EmailStatusDescription = "Mail Gönderildi.";
            }
            catch (Exception ex)
            {
                return false;
                //FlowInput.Reconciliation.EmailStatusTypeEnum = EmailStatusType.Failed;
                //FlowInput.Reconciliation.EmailStatusDescription = ex.Message;
                //FlowOutput.BaseMessages.Add(new BaseMessage(MessageType.Error, ex.Message, 508));
            }
            finally { mail.Dispose(); }


        }



    }
}