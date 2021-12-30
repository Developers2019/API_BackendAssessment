using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_BackendAssessment.Models
{
    public class LabelViewModel
    {
        public int Label_Id { get; set; }
        public string LabelName { get; set; }
        public DateTime? CapturedDate { get; set; }
    }
}