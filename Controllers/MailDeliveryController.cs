using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_BackendAssessment.Models;
using API_BackendAssessment.EmailServices;
using API_BackendAssessment.Helpers;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Threading;
using System.Security.Claims;
using Microsoft.AspNet.Identity.Owin;

namespace API_BackendAssessment.Controllers
{
  
    [RoutePrefix("api/mail")]
    public class MailDeliveryController : ApiController
    {

        [HttpPost]
        [Route("sendemail")]
        public IHttpActionResult SendEmail(EmailViewModel model)
        {
            try
            {
                if (model != null)
                {
                    model.EmailAddressFrom = "mpucukondlazi@gmail.com";
                    var status = EmailServices.EmailServices.SendEmail(model.EmailAddressTo, model.EmailAddressFrom, model.Subject, model.Message, new List<System.Net.Mail.Attachment>());
                    return Ok($"message sent: {Helper.YesNo(status)}");

                }
                return Ok($"message sent: No");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
            


        }
        
        [HttpGet]
        [Route("getlabels")]
        public IHttpActionResult GetLabels()
        {
            try
            {
                var labels = MailDeliveryModel.GetLabels();
                return Ok(labels);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
          
               
        } 

        [HttpGet]
        [Route("getemail")]
        public IHttpActionResult GetEmails()
        {
            try
            {
                var emails = MailDeliveryModel.GetAllEmails();
                return Ok(emails);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
          
               
        } 

        [HttpGet]
        [Route("getemailcount")]
        public IHttpActionResult GetEmailCount()
        {
            try
            {
                var email = "mpucukondlazi@gmail.com";
                var emails = MailDeliveryModel.GetInboxEmails(email).Count;
                return Ok(emails);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
          
               
        } 

        [HttpGet]
        [Route("getinboxemail")]
        public IHttpActionResult GetInboxEmail()
        {
            try
            {
                
                var xx = RequestContext.Principal.Identity.GetUserName();


                var email = "mpucukondlazi@gmail.com";
                List<EmailViewModel> emails = MailDeliveryModel.GetInboxEmails(email);

               
                return Ok(emails);

                
                
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
          
               
        }
        
        [HttpGet]
        [Route("getsentemail")]
        public IHttpActionResult GetSentEmail()
        {
            try
            {
                //var email = "ndlazim1@ukzn.ac.za";
                var email = "mpucukondlazi@gmail.com";

                var emails = MailDeliveryModel.GetSentEmails(email);
                return Ok(emails);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
          
               
        } 

        [HttpGet]
        [Route("reademail/{id}")]
        public IHttpActionResult GetEmailById(int id)
        {
            try
            {
                var emails = MailDeliveryModel.GetAllEmails().Where(x=>x.Email_Id == id).FirstOrDefault();
                return Ok(emails);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
          
               
        }
        
        [HttpGet]
        [Route("gettrashedemail")]
        public IHttpActionResult GetTrashedEmail()
        {
            try
            {
                var email = "mpucukondlazi@gmail.com";
                var emails = MailDeliveryModel.GetTrashedEmails(email);
                return Ok(emails);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
           
               
        }

        [HttpPost]
        [Route("sendtotrash/{id}")]
        public IHttpActionResult SendEmailToTrash(int id)
        {
            try
            {
                var status = MailDeliveryModel.MoveToTrash(id);
                if (status)
                {
                    return Ok($"Moved to trash: {Helper.YesNo(status)}");

                }
                return Ok($"Moved to trash: {Helper.YesNo(status)}");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
           

        }

        [HttpPost]
        [Route("recoveremail/{id}")]
        public IHttpActionResult RecoverEmail(int id)
        {
            try
            {
                var status = MailDeliveryModel.MoveToInbox(id);
                if (status)
                {
                    return Ok($"Moved to Inbox: {Helper.YesNo(status)}");

                }
                return Ok($"Moved to Inbox: {Helper.YesNo(status)}");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
          

        }

        [HttpPost]
        [Route("createlabel")]
        public IHttpActionResult CreateLabel(LabelViewModel label)
        {
            try
            {
                var status = MailDeliveryModel.CreateLabel(label);
                if (status)
                {
                    return Ok($"Label created: {Helper.YesNo(status)}");

                }
                return Ok($"Label created: {Helper.YesNo(status)}");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
           

        }

        [HttpPost]
        [Route("deletelabel/{id}")]
        public IHttpActionResult DeleteLabel(int id)
        {
            try
            {
                var status = MailDeliveryModel.DeleteLabel(id);
                if (status)
                {
                    return Ok($"Label deleted: {Helper.YesNo(status)}");

                }
                return Ok($"Label deleted: {Helper.YesNo(status)}");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
           

        }

        [HttpPost]
        [Route("addlabel/{id}")]
        public IHttpActionResult AddLabelToEmail(int id,int labelId)
        {
            try
            {
                var status = false;
               
                    status = MailDeliveryModel.AddLabelToEmail(id, labelId);
                    if (status)
                    {
                        return Ok($"Label added to email: {Helper.YesNo(status)}");

                    }
                
                return Ok($"Label added to email: {Helper.YesNo(status)}");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
           

        }

        [HttpPost]
        [Route("removelabel/{id}")]
        public IHttpActionResult RemoveLabelFromEmail(int id)
        {
            try
            {
                var status = MailDeliveryModel.RemoveLabelFromEmail(id);
                if (status)
                {
                    return Ok($"Moved to trash: {Helper.YesNo(status)}");

                }
                return Ok($"Moved to trash: {Helper.YesNo(status)}");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
           

        }

        [HttpGet]
        [Route("getemailsbylabel/{id}")]
        public IHttpActionResult GetEmailsByLabel(int id)
        {
            try
            {
                var status = MailDeliveryModel.GetEmailsByLabel(id) ?? new List<object>();
                if (status.Count > 0)
                {
                    return Ok(status);

                }
                return NotFound();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }

           

        }


      
    }
}
