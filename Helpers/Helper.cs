using API_BackendAssessment.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace API_BackendAssessment.Helpers
{
    public class Helper
    {
        public static SecurityProtocolType SecurityProtocolTypes()
        {

            SecurityProtocolType protocolType = ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            return protocolType;
        }

        public static string HowLongAgo(DateTime startDate)
        {
            DateTime now = DateTime.Now;
            TimeSpan elapsed = now.Subtract(startDate);

            double daysAgo = elapsed.TotalDays;
            string result = $"{daysAgo:0} days ago";

            return result;
        }

        public static string YesNo(bool answer)
        {
            return answer ? "Yes" : "No";
        }

        public static string LimitText(string text, int length)
        {
            if (!string.IsNullOrEmpty(text) && text.Length > length)
            {
                string limit = text.Substring(0, length);
                return $"{limit}...";
            }

            return text;
        } 

        public static string ConvertToHTML(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                var htmlString = HttpUtility.HtmlEncode(text);
                return htmlString;
            }

            return text;
        }

        

    }
}