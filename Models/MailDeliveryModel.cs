using API_BackendAssessment.Models.DataAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API_BackendAssessment.Models
{
    public class MailDeliveryModel
    {

        public static List<EmailViewModel> GetAllEmails()
        {
            try
            {
                using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
                {
                    string sql = "sp_GetAllEmails";
                    var emails = con.Query<EmailViewModel>(sql, new { }, commandType: CommandType.StoredProcedure).ToList();

                    return emails;

                }
            }
            catch (Exception)
            {
                //ActivityLog.AddErrorLog(ex.ToString());
                return null;
            }
        }

        public static List<EmailViewModel> GetInboxEmails(string emailTo)
        {

            return GetAllEmails().Where(x => x.EmailAddressTo.Contains(emailTo)).OrderByDescending(x=>x.CapturedDate).ToList();
            
        } 

        public static List<EmailViewModel> GetSentEmails(string emailFrom)
        {
            return GetAllEmails().Where(x => x.EmailAddressFrom.Contains(emailFrom)).OrderByDescending(x => x.CapturedDate).ToList();

        } 
        public static List<EmailViewModel> GetTrashedEmails(string emailTo)
        {
            
            return GetAllEmails().Where(x => x.FolderId == Convert.ToInt32(eNums.EmailFolders.Trash) && x.EmailAddressTo.Contains(emailTo)).OrderByDescending(x => x.CapturedDate).ToList();

        }

        public static List<EmailViewModel> GetAllEmails(string emailTo)
        {

            return GetAllEmails().Where(x => !x.EmailAddressTo.Contains(emailTo)).OrderByDescending(x => x.CapturedDate).ToList();

        }

        public static void AddEmail(EmailViewModel model)
        {

            using (MailDeliveryEntities db = new MailDeliveryEntities())
            {
                var email = new Email
                {
                    EmailAddressFrom = model.EmailAddressFrom,
                    EmailAddressTo = model.EmailAddressTo,
                    FolderId = Convert.ToInt32(eNums.EmailFolders.Inbox),
                    Subject = model.Subject,
                    CapturedDate = DateTime.Now,
                    Message = model.Message,
                    Url = HttpContext.Current.Request.Url.AbsoluteUri,
                    Status = model.Status
                };

                db.Emails.Add(email);
                db.SaveChanges();
            }
            
        }

        public static bool MoveToTrash(int id)
        {
            try
            {
                using (MailDeliveryEntities db = new MailDeliveryEntities())
                {
                    var currentEmail = db.Emails.Find(id);

                    if (currentEmail != null)
                    {
                        currentEmail.FolderId = Convert.ToInt32(eNums.EmailFolders.Trash);
                        db.Entry(currentEmail).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {

                return false;

            }

        } 
        public static bool MoveToInbox(int id)
        {
            try
            {
                using (MailDeliveryEntities db = new MailDeliveryEntities())
                {
                    var currentEmail = db.Emails.Find(id);

                    if (currentEmail != null)
                    {
                        currentEmail.FolderId = Convert.ToInt32(eNums.EmailFolders.Inbox);
                        db.Entry(currentEmail).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();

                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {

                return false;

            }

        }


        
        public static bool CreateLabel(LabelViewModel model)
        {
            using (MailDeliveryEntities db = new MailDeliveryEntities())
            {

                var label = new Label
                {
                    LabelName = model.LabelName
                };
                var obj = db.Labels.Add(label);
                db.SaveChanges();

                if (obj != null)
                {
                    return true;

                }
                return false;
            }
        } 
        public static bool DeleteLabel(int id)
        {
            using (MailDeliveryEntities db = new MailDeliveryEntities())
            {

                var currentLabel = db.Labels.Find(id);
                if (currentLabel != null)
                {
                    db.Labels.Remove(currentLabel);
                    db.SaveChanges();

                    return true;
                }
                return false;
               
            }
        }



        public static bool AddLabelToEmail(int id, int labelId)
        {
            try
            {
                using (MailDeliveryEntities db = new MailDeliveryEntities())
                {
                    var currentEmail = db.Emails.Where(x => x.Email_Id == id).FirstOrDefault();
                    var label = db.Labels.Where(x => x.Label_Id == labelId).FirstOrDefault();

                    if (currentEmail != null && label!=null)
                    {
                        var emailLabel = new EmailLabel
                        {
                            Email_Id = currentEmail.Email_Id,
                            Label_Id = labelId

                        };
                        db.EmailLabels.Add(emailLabel);
                        db.SaveChanges();

                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {

                return false;

            }

        }

        public static bool RemoveLabelFromEmail(int id)
        {
            try
            {
                using (MailDeliveryEntities db = new MailDeliveryEntities())
                {
                    var currentEmail = db.EmailLabels.Find(id);
                    

                    if (currentEmail!=null)
                    {
                        db.EmailLabels.Remove(currentEmail);
                        db.SaveChanges();

                        return true;
                    }
                    return false;
                }
            }
            catch (Exception)
            {

                return false;

            }

        }
       

        public static List<object> GetEmailsByLabel(int id)
        {
            try
            {
                using (MailDeliveryEntities db = new MailDeliveryEntities())
                {
                    IEnumerable<object> emails = from x in GetAllEmails()
                                                 join y in db.EmailLabels on x.Email_Id equals y.Email_Id
                                                 join z in db.Labels on y.Label_Id equals z.Label_Id
                                                 where y.Label_Id == id
                                                 select new { x.EmailAddressFrom, x.EmailAddressTo, x.Subject, x.Message, z.LabelName };


                    return emails.ToList();
                }

                    
            }
            catch (Exception)
            {
                return null;
            }
        }

      

    }
}