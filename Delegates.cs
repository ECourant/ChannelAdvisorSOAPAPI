using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelAdvisorSOAP
{
    internal delegate void ThrottleSOAPDelegate(string Function);
    internal delegate dynamic GetAPICredentials(Type CredentialsType);
}
