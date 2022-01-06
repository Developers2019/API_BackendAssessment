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
        #region Folders
        /// <summary>
        /// Gets all the emails
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets emails from the inbox
        /// </summary>
        /// <param name="emailTo"></param>
        /// <returns></returns>
        public static List<EmailViewModel> GetInboxEmails(string emailTo)
        {

            return GetAllEmails().Where(x => x.EmailAddressTo.Contains(emailTo) && x.FolderId == Convert.ToInt32(eNums.EmailFolders.Inbox)).OrderByDescending(x => x.CapturedDate).ToList();

        }

        /// <summary>
        /// Gets all sent emails
        /// </summary>
        /// <param name="emailFrom"></param>
        /// <returns></returns>
        public static List<EmailViewModel> GetSentEmails(string emailFrom)
        {
            return GetAllEmails().Where(x => x.EmailAddressFrom.Contains(emailFrom)).OrderByDescending(x => x.CapturedDate).ToList();

        }

        /// <summary>
        /// Gets all deleted emails
        /// </summary>
        /// <param name="emailTo"></param>
        /// <returns></returns>
        public static List<EmailViewModel> GetTrashedEmails(string emailTo)
        {

            return GetAllEmails().Where(x => x.FolderId == Convert.ToInt32(eNums.EmailFolders.Trash) && x.EmailAddressTo.Contains(emailTo)).OrderByDescending(x => x.CapturedDate).ToList();

        }

        /// <summary>
        /// Create an new email record
        /// </summary>
        /// <param name="model"></param>
        public static void AddEmail(EmailViewModel model)
        {
            try
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
            catch (Exception)
            {

                throw;
            }



        }

        /// <summary>
        /// Delete emails from the inbox
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Recover a deleted email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="emailTo"></param>
        /// <returns></returns>
        public static bool MoveToInbox(int id, string emailTo)
        {
            try
            {
                using (MailDeliveryEntities db = new MailDeliveryEntities())
                {
                    var currentEmail = db.Emails.Find(id);

                    if (currentEmail != null)
                    {
                        if (currentEmail.EmailAddressTo.Contains(emailTo))
                        {
                            currentEmail.FolderId = Convert.ToInt32(eNums.EmailFolders.Inbox);

                        }
                        else
                        {
                            currentEmail.FolderId = Convert.ToInt32(eNums.EmailFolders.Sent);

                        }


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
        #endregion

        #region Labels

        /// <summary>
        /// Create a new label
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static bool CreateLabel(LabelViewModel model)
        {
            try
            {
                using (MailDeliveryEntities db = new MailDeliveryEntities())
                {

                    var obj = db.Labels.Add(new Label { LabelName = model.LabelName, CapturedDate = DateTime.Now });
                    db.SaveChanges();

                    if (obj != null)
                    {
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

        /// <summary>
        /// Get a list of emails
        /// </summary>
        /// <returns></returns>
        public static List<LabelViewModel> GetLabels()
        {
            try
            {
                using (MailDeliveryEntities db = new MailDeliveryEntities())
                {
                    List<LabelViewModel> labels = db.Labels.Select(x => new LabelViewModel() { Label_Id = x.Label_Id, LabelName = x.LabelName }).ToList();
                    List<LabelViewModel> orderedList = labels.OrderBy(x => x.LabelName).ToList();

                    return orderedList;


                }
            }
            catch (Exception)
            {

                return null;
            }

        }

        /// <summary>
        /// Delete a label
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Add a label to an email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="labelId"></param>
        /// <returns></returns>
        public static bool AddLabelToEmail(int id, int labelId)
        {
            try
            {
                using (MailDeliveryEntities db = new MailDeliveryEntities())
                {
                    var currentEmail = db.Emails.Where(x => x.Email_Id == id).FirstOrDefault();
                    var label = db.Labels.Where(x => x.Label_Id == labelId).FirstOrDefault();

                    if (currentEmail != null && label != null)
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

        /// <summary>
        /// Remove a label from an email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="label_id"></param>
        /// <returns></returns>
        public static bool RemoveLabelFromEmail(int id, int label_id)
        {
            try
            {
                using (MailDeliveryEntities db = new MailDeliveryEntities())
                {
                    var currentEmail = db.EmailLabels.Where(x => x.Email_Id == id && x.Label_Id == label_id).FirstOrDefault();


                    if (currentEmail != null)
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

        /// <summary>
        /// Get an email by the label
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
                                                 select new { x.Email_Id, x.EmailAddressFrom, x.EmailAddressTo, z.LabelName, x.HowLongAgo, x.Description };


                    return emails.ToList();
                }


            }
            catch (Exception)
            {
                return null;
            }
        } 
        #endregion



    }
}