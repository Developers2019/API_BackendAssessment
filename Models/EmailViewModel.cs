using API_BackendAssessment.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_BackendAssessment.Models
{
    public class EmailViewModel
    {
        public int Email_Id { get; set; }
        public string  EmailAddressTo { get; set; }
        public string [] EmailAddressToMany { get; set; }
        public string Subject { get; set; }
        public int? FolderId { get; set; }
        public DateTime? CapturedDate { get; set; }

        public string HowLongAgo { get 
            {
                return Helper.HowLongAgo(CapturedDate.GetValueOrDefault());
            } 
        } 

        public string Description { get 
            {
                return $"{Subject} - {Helper.LimitText(Message,65)}";
            } 
        }  
        public string SentTo 
        { 
            get 
        
            {
                return $"To: {EmailAddressTo}";
            } 
        }  

        public string SentFrom 
        { 
            get 
        
            {
                return $"From: {EmailAddressFrom}";
            } 
        } 
        public string DisplayDate { get 
            {
                return CapturedDate.GetValueOrDefault().ToString("dddd, dd MMMM yyyy");
            } 
        } 

        public string HTMLBody { get 
        {
                return Helper.ConvertToHTML(Message);
            } 
        }
        public string LabelName { get; set; }
        public string EmailAddressFrom { get; set; }
        public string Url { get; set; }
        public bool? Status { get; set; }
        public string ErrorMsg { get; set; }
        public string Message { get; set; }
    }
}