using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelAdvisorSOAP
{
    internal static class ThrottleDefinitions
    {
        internal static Dictionary<string, ThrottleBucket> Timings = new Dictionary<string, ThrottleBucket>()
        {
            { "AdminService.GetAuthorizationList", new ThrottleBucket(100, "AdminService.GetAuthorizationList") },
            { "AdminService.RequestAccess", new ThrottleBucket(100, "AdminService.RequestAccess") },
            { "InventoryService.AssignLabelListToInventoryItemList", new ThrottleBucket(10000, "InventoryService.AssignLabelListToInventoryItemList") },
            { "InventoryService.DoesSkuExist", new ThrottleBucket(25000, "InventoryService.DoesSkuExist") },
            { "InventoryService.GetFilteredInventoryItemList", new ThrottleBucket(5000, "InventoryService.GetFilteredInventoryItemList") },
            { "InventoryService.GetFilteredSkuList", new ThrottleBucket(5000, "InventoryService.GetFilteredSkuList") },
            { "InventoryService.GetInventoryItemQuantityInfo", new ThrottleBucket(10000, "InventoryService.GetInventoryItemQuantityInfo") },
            { "InventoryService.SynchInventoryItem", new ThrottleBucket(250000, "InventoryService.SynchInventoryItem") },
            { "InventoryService.SynchInventoryItemList", new ThrottleBucket(10000, "InventoryService.SynchInventoryItemList") },
            { "InventoryService.UpdateInventoryItemQuantityAndPrice", new ThrottleBucket(500000, "InventoryService.UpdateInventoryItemQuantityAndPrice") },
            { "InventoryService.UpdateInventoryItemQuantityAndPriceList", new ThrottleBucket(20000, "InventoryService.UpdateInventoryItemQuantityAndPriceList") },
            { "ListingService.WithdrawListings", new ThrottleBucket(2500, "ListingService.WithdrawListings") },
            { "OrderService.GetOrderList", new ThrottleBucket(1000, "OrderService.GetOrderList") },
        };
    }
}
