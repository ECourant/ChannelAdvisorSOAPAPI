using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannelAdvisorSOAP.Admin;
namespace ChannelAdvisorSOAP.Pipelines
{
    public sealed class AdminPipeline : IPipeline<AdminService, APICredentials>
    { 
        internal AdminPipeline(ThrottleSOAPDelegate CallDelegate, GetAPICredentials GetAPICredentials) : base(CallDelegate, GetAPICredentials)
        {
             
        }

        public AuthorizationResponse[] GetAuthorizationList()
        {
            return this.GetAuthorizationList(-1);
        }
        public AuthorizationResponse[] GetAuthorizationList(int LocalID)
        {
            this.Throttle("AdminService.GetAuthorizationList");
            return Service.GetAuthorizationList(LocalID < 0 ? string.Empty : LocalID.ToString()).ResultData ?? new AuthorizationResponse[] { };
        }
        public bool RequestAccess(int LocalID)
        {
            this.Throttle("AdminService.RequestAccess");
            return Service.RequestAccess(LocalID).ResultData;
        }
        public string Ping()
        {
            this.Throttle("AdminService.Ping");
            return Service.Ping().ResultData;
        }
    }
}
