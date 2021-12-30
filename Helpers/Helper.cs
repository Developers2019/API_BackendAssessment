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
    }
}