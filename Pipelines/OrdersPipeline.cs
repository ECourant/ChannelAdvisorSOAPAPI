using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannelAdvisorSOAP.Orders;
namespace ChannelAdvisorSOAP.Pipelines
{
    public sealed class OrdersPipeline : IPipeline<OrderService, APICredentials>
    {
        internal OrdersPipeline(ThrottleSOAPDelegate CallDelegate, GetAPICredentials GetAPICredentials) : base(CallDelegate, GetAPICredentials)
        {

        }

        public OrderResponseItem[] GetOrderList(string AccountID, OrderCriteria OrderCriteria)
        {
            this.Throttle("OrderService.GetOrderList");
            return Service.GetOrderList(AccountID, OrderCriteria).ResultData;
        }

        public OrderUpdateResponse[] UpdateOrderList(string AccountID, OrderUpdateSubmit[] OrderList)
        {
            this.Throttle("OrderService.UpdateOrderList");
            return Service.UpdateOrderList(AccountID, OrderList).ResultData;
        }
    }
}
