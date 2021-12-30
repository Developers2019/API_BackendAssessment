using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_BackendAssessment.Models
{
    public class EmailLabelViewModel
    {
        public int EmailLabel_Id { get; set; }
        public int? Email_Id { get; set; }
        public int? Label_Id { get; set; }
    }
}