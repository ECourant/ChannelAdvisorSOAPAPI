using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannelAdvisorSOAP.Fulfillment;

namespace ChannelAdvisorSOAP.Pipelines
{
    public sealed class FulfillmentPipeline : IPipeline<FulfillmentService, APICredentials>
    {
        internal FulfillmentPipeline(ThrottleSOAPDelegate ThrottleDelegate, GetAPICredentials GetAPICredentials) : base(ThrottleDelegate, GetAPICredentials)
        {

        }

        public OrderFulfillmentResponse[] GetOrderFulfillmentDetailList(string AccountID, int[] OrderIDList)
        {
            this.Throttle("FulfillmentService.GetOrderFulfillmentDetailList");
            return Service.GetOrderFulfillmentDetailList(AccountID, OrderIDList, new string[] { }).ResultData;
        }
    }
}
