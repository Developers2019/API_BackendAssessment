using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_BackendAssessment.Models;
using API_BackendAssessment.EmailServices;
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
                    var status = EmailServices.EmailServices.SendEmail(model.EmailAddressToMany, model.EmailAddressFrom, model.Subject, model.Message, new List<System.Net.Mail.Attachment>());
                    return Ok($"message sent: {YesNo(status)}");

                }
                return Ok($"message sent: No");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
            


        }
        
        [HttpGet]
        [Route("getemail")]
        public IHttpActionResult GetEmail()
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
                    return Ok($"Moved to trash: {YesNo(status)}");

                }
                return Ok($"Moved to trash: {YesNo(status)}");
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
                    return Ok($"Moved to Inbox: {YesNo(status)}");

                }
                return Ok($"Moved to Inbox: {YesNo(status)}");
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
                    return Ok($"Label created: {YesNo(status)}");

                }
                return Ok($"Label created: {YesNo(status)}");
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
                    return Ok($"Label deleted: {YesNo(status)}");

                }
                return Ok($"Label deleted: {YesNo(status)}");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
           

        }

        [HttpPost]
        [Route("addlabel/{id}/{labelId}")]
        public IHttpActionResult AddLabelToEmail(int id,int labelId)
        {
            try
            {
                var status = MailDeliveryModel.AddLabelToEmail(id, labelId);
                if (status)
                {
                    return Ok($"Label added to email: {YesNo(status)}");

                }
                return Ok($"Label added to email: {YesNo(status)}");
            }
            catch (Exception ex)
            {

                return BadRequest(ex.ToString());
            }
           

        }

        [HttpPost]
        [Route("removelabel/{id}")]
        public IHttpActionResult RemoveLabelToEmail(int id)
        {
            try
            {
                var status = MailDeliveryModel.RemoveLabelFromEmail(id);
                if (status)
                {
                    return Ok($"Moved to trash: {YesNo(status)}");

                }
                return Ok($"Moved to trash: {YesNo(status)}");
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


        public string YesNo(bool answer)
        {
            return answer ? "Yes" : "No";
        }
    }
}
