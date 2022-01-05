using API_BackendAssessment.Helpers;
using API_BackendAssessment.Models;
using API_BackendAssessment.Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace API_BackendAssessment.EmailServices
{
    public class EmailServices
    {
        /// <summary>
        /// System email function 
        /// </summary>
        /// <param name="destination">destination</param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachments"></param>
        /// <returns></returns>
        public static bool SendEmail(string destination, string emailFrom,string subject, string body, List<Attachment> attachments)
        {
            Helper.SecurityProtocolTypes();
            bool status;
            try
            {

                SmtpDetail detail = Smtp();
              
                MailMessage message = new MailMessage { From = new MailAddress(emailFrom) };

                List<string> destinations = destination.Split(',').ToList();
                foreach (string dest in destinations)
                {
                    message.To.Add(new MailAddress(dest));
                }

                message.IsBodyHtml = true;
                message.Subject = subject;
                message.Body = body;


                SmtpClient client = new SmtpClient(detail.SmtpHost, (int)detail.SmtpPort);
               

                foreach (Attachment attachment in attachments)
                {
                    message.Attachments.Add(attachment);
                }

                client.Send(message);

                status = true;

                foreach (string dest in destinations)
                {
                    MailDeliveryModel.AddEmail(new EmailViewModel { EmailAddressFrom = emailFrom, EmailAddressTo = dest, Subject = subject, Message = body, Status = status });
                }

                return status;
            }
            catch (Exception ex)
            {
                var message = ex.ToString();
                status = false;
                MailDeliveryModel.AddEmail(new EmailViewModel { EmailAddressFrom = emailFrom, EmailAddressTo = destination, Subject = subject, Message = body, Status = status, ErrorMsg = message });
                return status;
            }

        }

        private static SmtpDetail Smtp()
        {
            try
            {
                using (var db = new MailDeliveryEntities())
                {

                    var smtp = db.SmtpDetails.FirstOrDefault();
                    return smtp;

                }
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}