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
        public string EmailAddressFrom { get; set; }
        public string Url { get; set; }
        public bool? Status { get; set; }
        public string ErrorMsg { get; set; }
        public string Message { get; set; }
    }
}