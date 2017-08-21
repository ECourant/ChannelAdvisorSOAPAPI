using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelAdvisorSOAP
{
    public abstract class IPipeline<TService, TCredentials> : AErrorChild where TService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {
        protected log4net.ILog LogNet { get; set; }

        protected TService Service { get; set; }

        internal IPipeline(ThrottleSOAPDelegate CallDelegate, GetAPICredentials GetAPICredentials)
        {
            this.LogNet = log4net.LogManager.GetLogger(this.GetType());
            this.Throttle = CallDelegate;
            this.GetAPICredentials = GetAPICredentials;
            this.Service = Activator.CreateInstance<TService>();
            ((dynamic)Service).APICredentialsValue = this.GetAPICredentials(typeof(TCredentials));
        }




        internal ThrottleSOAPDelegate Throttle { get; private set; }
        private GetAPICredentials GetAPICredentials { get; set; }
    }
}
