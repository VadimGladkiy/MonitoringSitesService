using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;


namespace MonitoringSitesService
{
    public class StatusController: ApiController
    {
        private static bool _serviceLaunched;
        [HttpGet]
        public static void setServiceStatus(bool status)
        {
            _serviceLaunched = status;
        }
        /*
        public bool GetCheckStatus()
        {
            return _serviceLaunched;
        }*/
        public StatusController()
        {
            
            
        }
        public string GetString()
        {
            return "Hello from StatusController";
        }

    }
}
