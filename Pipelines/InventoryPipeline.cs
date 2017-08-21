using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChannelAdvisorSOAP.Inventory;
namespace ChannelAdvisorSOAP.Pipelines
{
    public sealed class InventoryPipeline : IPipeline<InventoryService, APICredentials>
    {
        internal InventoryPipeline(ThrottleSOAPDelegate CallDelegate, GetAPICredentials GetAPICredentials) : base(CallDelegate, GetAPICredentials)
        {

        }

        public bool AssignLabelListToInventoryItemList(string AccountID, string[] LabelList, bool CreateLabelIfNotExist, string[] SKUList)
        {
            return this.AssignLabelListToInventoryItemList(AccountID, LabelList, CreateLabelIfNotExist, SKUList, string.Empty);
        }

        public bool AssignLabelListToInventoryItemList(string AccountID, string[] LabelList, bool CreateLabelIfNotExist, string[] SKUList, string AssignReasonDesc)
        {
            try
            {
                this.Throttle("InventoryService.AssignLabelListToInventoryItemList");
                return Service.AssignLabelListToInventoryItemList(AccountID, LabelList, CreateLabelIfNotExist, SKUList, AssignReasonDesc).ResultData;
            }
            catch(Exception e)
            {
                LogNet.Error($"An exception has been thrown while attempting to assign label list [{string.Join(", ", LabelList)}] to inventory item list [{string.Join(", ", SKUList)}]; Create label if it doesn't exist: [{CreateLabelIfNotExist}] with reason: [{AssignReasonDesc}]", e);
                Report(e);
                throw;
            }
        }

        public bool DeleteInventoryItem(string AccountID, string SKU)
        {
            try
            {
                this.Throttle("InventoryService.DeleteInventoryItem");
                return Service.DeleteInventoryItem(AccountID, SKU).ResultData;
            }
            catch(Exception e)
            {
                LogNet.Error($"An exception has been thrown while attempting to delete inventory item [{SKU}]", e);
                Report(e);
                throw;
            }
        }

        public bool[] DeleteUpsellRelationship(string AccountID, string ParentSKU, string[] ChildSKUList)
        {
            try
            {
                this.Throttle("InventoryService.DeleteUpsellRelationship");
                return Service.DeleteUpsellRelationship(AccountID, ParentSKU, ChildSKUList).ResultData;
            }
            catch (Exception e)
            {
                LogNet.Error($"An exception has been thrown while attempting to delete upsell relationship between Parent SKU: [{ParentSKU}] and Child SKUs: [{string.Join(", ", ChildSKUList)}]", e);
                Report(e);
                throw;
            }
        }

        public bool DoesSKUExist(string AccountID, string SKU)
        {
            try
            {
                this.Throttle("InventoryService.DoesSkuExist");
                return Service.DoesSkuExist(AccountID, SKU).ResultData;
            }
            catch (Exception e)
            {
                LogNet.Error($"An exception has been thrown while attempting to check if SKU [{SKU}] exists", e);
                Report(e);
                throw;
            }
        }

        public DoesSkuExistResponse[] DoesSKUExistList(string AccountID, string[] SKUList)
        {
            List<List<string>> Searches = new List<List<string>>();
            List<string> Q = new List<string>();
            foreach (var SKU in SKUList.Where(SKU => !string.IsNullOrWhiteSpace(SKU)))
            {
                if (Q.Count == 500)
                {
                    Searches.Add(Q);
                    Q = new List<string>();
                }
                Q.Add(SKU);
            }
            if (Q.Count > 0)
            {
                Searches.Add(Q);
                Q = new List<string>();
            }
            List<DoesSkuExistResponse> Items = new List<DoesSkuExistResponse>();
            foreach (var Que in Searches)
            {
                try
                {
                    this.Throttle("InventoryService.DoesSkuExistList");
                    foreach (var Item in Service.DoesSkuExistList(AccountID, Que.ToArray()).ResultData)
                        Items.Add(Item);
                }
                catch(Exception e)
                {
                    LogNet.Error($"An exception has been thrown while attempting to check if sku list exists [{string.Join(",", Que)}]", e);
                    Report(e);
                    throw;
                }
            }
            return Items.ToArray();
        }

        public ClassificationConfigurationInformation[] GetClassificationConfigurationInformation(string AccountID)
        {
            this.Throttle("InventoryService.GetClassificationConfigurationInformation");
            return Service.GetClassificationConfigurationInformation(AccountID).ResultData;
        }

        public DistributionCenterResponse[] GetDistributionCenterList(string AccountID)
        {
            this.Throttle("InventoryService.GetDistributionCenterList");
            return Service.GetDistributionCenterList(AccountID).ResultData;
        }

        public InventoryItemResponse[] GetFilteredInventoryItemList(string AccountID, InventoryItemCriteria ItemCriteria, InventoryItemDetailLevel DetailLevel)
        {
            this.Throttle("InventoryService.GetFilteredInventoryItemList");
            return Service.GetFilteredInventoryItemList(AccountID, ItemCriteria, DetailLevel, string.Empty, string.Empty).ResultData;
        }
        public InventoryItemResponse[] GetFilteredInventoryItemList(string AccountID, InventoryItemCriteria ItemCriteria, InventoryItemDetailLevel DetailLevel, InventoryItemSortField SortField)
        {
            this.Throttle("InventoryService.GetFilteredInventoryItemList");
            return Service.GetFilteredInventoryItemList(AccountID, ItemCriteria, DetailLevel, SortField.ToString(), string.Empty).ResultData;
        }
        public InventoryItemResponse[] GetFilteredInventoryItemList(string AccountID, InventoryItemCriteria ItemCriteria, InventoryItemDetailLevel DetailLevel, InventoryItemSortField SortField, SortDirection SortDirection)
        {
            this.Throttle("InventoryService.GetFilteredInventoryItemList");
            return Service.GetFilteredInventoryItemList(AccountID, ItemCriteria, DetailLevel, SortField.ToString(), SortDirection.ToString()).ResultData;
        }

        public string[] GetFilteredSKUList(string AccountId, InventoryItemCriteria ItemCriteria)
        {
            this.Throttle("InventoryService.GetFilteredSkuList");
            return Service.GetFilteredSkuList(AccountId, ItemCriteria, string.Empty, string.Empty).ResultData;
        }
        public string[] GetFilteredSKUList(string AccountId, InventoryItemCriteria ItemCriteria, InventoryItemSortField SortField)
        {
            this.Throttle("InventoryService.GetFilteredSkuList");
            return Service.GetFilteredSkuList(AccountId, ItemCriteria, SortField.ToString(), string.Empty).ResultData;
        }
        public string[] GetFilteredSKUList(string AccountId, InventoryItemCriteria ItemCriteria, InventoryItemSortField SortField, SortDirection SortDirection)
        {
            this.Throttle("InventoryService.GetFilteredSkuList");
            return Service.GetFilteredSkuList(AccountId, ItemCriteria, SortField.ToString(), SortDirection.ToString()).ResultData;
        }

        public AttributeInfo[] GetInventoryItemAttributeList(string AccountID, string SKU)
        {
            this.Throttle("InventoryService.GetInventoryItemAttributeList");
            return Service.GetInventoryItemAttributeList(AccountID, SKU).ResultData;
        }

        public ImageInfoResponse[] GetInventoryImageList(string AccountID, string SKU)
        {
            this.Throttle("InventoryService.GetInventoryItemImageList");
            return Service.GetInventoryItemImageList(AccountID, SKU).ResultData;
        }

        public InventoryItemResponse[] GetInventoryItemList(string AccountID, string[] SKUList)
        {
            Queue<List<string>> Searches = new Queue<List<string>>();
            List<string> Q = new List<string>();
            foreach (var SKU in SKUList)
            {
                if (Q.Count == 100)
                {
                    Searches.Enqueue(Q);
                    Q = new List<string>();
                }
                Q.Add(SKU);
            }
            if (Q.Count > 0)
            {
                Searches.Enqueue(Q);
                Q = new List<string>();
            }
            List<InventoryItemResponse> Items = new List<InventoryItemResponse>();
            foreach (var Que in Searches)
            {
                this.Throttle("InventoryService.GetInventoryItemList");
                foreach (var Item in Service.GetInventoryItemList(AccountID, Que.ToArray()).ResultData ?? new InventoryItemResponse[] { })
                    Items.Add(Item);
            }
            return Items.ToArray();
        }

        public QuantityInfoResponse GetInventoryItemQuantityInfo(string AccountID, string SKU)
        {
            this.Throttle("InventoryService.GetInventoryItemQuantityInfo");
            return Service.GetInventoryItemQuantityInfo(AccountID, SKU).ResultData;
        }

        public DistributionCenterInfoResponse[] GetInventoryItemShippingInfo(string AccountID, string SKU)
        {
            this.Throttle("InventoryService.GetInventoryItemShippingInfo");
            return Service.GetInventoryItemShippingInfo(AccountID, SKU).ResultData;
        }

        public StoreInfo GetInventoryItemStoreInfo(string AccountID, string SKU)
        {
            this.Throttle("InventoryService.GetInventoryItemStoreInfo");
            return Service.GetInventoryItemStoreInfo(AccountID, SKU).ResultData;
        }

        public VariationInfo GetInventoryItemVariationInfo(string AccountID, string SKU)
        {
            this.Throttle("InventoryService.GetInventoryItemVariationInfo");
            return Service.GetInventoryItemVariationInfo(AccountID, SKU).ResultData;
        }

        public int GetInventoryQuantity(string AccountID, string SKU)
        {
            this.Throttle("InventoryService.GetInventoryQuantity");
            return Service.GetInventoryQuantity(AccountID, SKU).ResultData;
        }

        public InventoryQuantityResponse[] GetInventoryQuantityList(string AccountID, string[] SKUList)
        {
            Queue<List<string>> Searches = new Queue<List<string>>();
            List<string> Q = new List<string>();
            foreach (var SKU in SKUList)
            {
                if (Q.Count == 100)
                {
                    Searches.Enqueue(Q);
                    Q = new List<string>();
                }
                Q.Add(SKU);
            }
            if (Q.Count > 0)
            {
                Searches.Enqueue(Q);
                Q = new List<string>();
            }
            List<InventoryQuantityResponse> Items = new List<InventoryQuantityResponse>();
            foreach (var Que in Searches)
            {
                this.Throttle("InventoryService.GetInventoryQuantityList");
                foreach (var Item in Service.GetInventoryQuantityList(AccountID, Que.ToArray()).ResultData)
                    Items.Add(Item);
            }
            return Items.ToArray();
        }

        public InventoryUpsellInfoResponse[] GetUpsellRelationship(string AccountID, string[] ParentSKUList)
        {
            this.Throttle("InventoryService.GetUpsellRelationship");
            return Service.GetUpsellRelationship(AccountID, ParentSKUList).ResultData;
        }

        public bool RemoveLabelListFromInventoryItemList(string AccountID, string[] LabelList, string[] SKUList)
        {
            return this.RemoveLabelListFromInventoryItemList(AccountID, LabelList, SKUList, string.Empty);
        }
        public bool RemoveLabelListFromInventoryItemList(string AccountID, string[] LabelList, string[] SKUList, string RemoveReasonDesc)
        {
            this.Throttle("InventoryService.RemoveLabelListFromInventoryItemList");
            return Service.RemoveLabelListFromInventoryItemList(AccountID, LabelList, SKUList, RemoveReasonDesc).ResultData;
        }

        public bool SynchInventoryItem(string AccountID, InventoryItemSubmit Item)
        {
            this.Throttle("InventoryService.SynchInventoryItem");
            return Service.SynchInventoryItem(AccountID, Item).ResultData;
        }

        public SynchInventoryItemResponse[] SynchInventoryItemList(string AccountID, InventoryItemSubmit[] ItemList)
        {
            this.Throttle("InventoryService.SynchInventoryItemList");
            return Service.SynchInventoryItemList(AccountID, ItemList).ResultData;
        }

        public bool UpdateInventoryItemQuantityAndPrice(string AccountID, InventoryItemQuantityAndPrice ItemQTYAndPrice)
        {
            this.Throttle("InventoryService.UpdateInventoryItemQuantityAndPrice");
            return Service.UpdateInventoryItemQuantityAndPrice(AccountID, ItemQTYAndPrice).ResultData;
        }

        public UpdateInventoryItemResponse[] UpdateInventoryItemQuantityAndPriceList(string AccountID, InventoryItemQuantityAndPrice[] ItemQTYAndPriceList)
        {
            this.Throttle("InventoryService.UpdateInventoryItemQuantityAndPriceList");
            return Service.UpdateInventoryItemQuantityAndPriceList(AccountID, ItemQTYAndPriceList).ResultData;
        }
    }
}
