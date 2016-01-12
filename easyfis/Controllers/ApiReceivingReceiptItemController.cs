using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.Controllers
{
    public class ApiReceivingReceiptItemController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===========================
        // LIST Receiving Receipt Item
        // ===========================
        [Route("api/listReceivingReceiptItem")]
        public List<Models.TrnReceivingReceiptItem> Get()
        {
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            RRId = d.RRId,
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            POId = d.POId,
                                            PO = d.TrnPurchaseOrder.PONumber,
                                            ItemId = d.ItemId,
                                            Item = d.MstArticle.Article,
                                            ItemCode = d.MstArticle.ManualArticleCode,
                                            Particulars = d.Particulars,
                                            UnitId = d.UnitId,
                                            Unit = d.MstUnit.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            VATId = d.VATId,
                                            VAT = d.MstTaxType.TaxType,
                                            VATPercentage = d.VATPercentage,
                                            VATAmount = d.VATAmount,
                                            WTAXId = d.WTAXId,
                                            WTAX = d.MstTaxType1.TaxType,
                                            WTAXPercentage = d.WTAXPercentage,
                                            WTAXAmount = d.WTAXAmount,
                                            BranchId = d.BranchId,
                                            Branch = d.MstBranch.Branch,
                                            BaseUnitId = d.BaseUnitId,
                                            BaseUnit = d.MstUnit1.Unit,
                                            BaseQuantity = d.BaseQuantity,
                                            BaseCost = d.BaseCost
                                        };
            return receivingReceiptItems.ToList();
        }

        // ===================================
        // GET Receiving Receipt Item by RR Id
        // ===================================
        [Route("api/listReceivingReceiptItemByRRId/{RRId}")]
        public List<Models.TrnReceivingReceiptItem> GetRRLinesByRRId(String RRId)
        {
            var RR_Id = Convert.ToInt32(RRId);
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.RRId == RR_Id
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            RRId = d.RRId,
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            POId = d.POId,
                                            PO = d.TrnPurchaseOrder.PONumber,
                                            ItemId = d.ItemId,
                                            Item = d.MstArticle.Article,
                                            ItemCode = d.MstArticle.ManualArticleCode,
                                            Particulars = d.Particulars,
                                            UnitId = d.UnitId,
                                            Unit = d.MstUnit.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            VATId = d.VATId,
                                            VAT = d.MstTaxType.TaxType,
                                            VATPercentage = d.VATPercentage,
                                            VATAmount = d.VATAmount,
                                            WTAXId = d.WTAXId,
                                            WTAX = d.MstTaxType1.TaxType,
                                            WTAXPercentage = d.WTAXPercentage,
                                            WTAXAmount = d.WTAXAmount,
                                            BranchId = d.BranchId,
                                            Branch = d.MstBranch.Branch,
                                            BaseUnitId = d.BaseUnitId,
                                            BaseUnit = d.MstUnit1.Unit,
                                            BaseQuantity = d.BaseQuantity,
                                            BaseCost = d.BaseCost
                                        };
            return receivingReceiptItems.ToList();
        }

        // ==========================
        // ADD Receving Retrieve Item
        // ==========================
        [Route("api/addReceivingReceiptItem")]
        public int Post(Models.TrnReceivingReceiptItem receivingReceiptItem)
        {
            try
            {
                Data.TrnReceivingReceiptItem newReceivingReceiptItem = new Data.TrnReceivingReceiptItem();

                newReceivingReceiptItem.RRId = receivingReceiptItem.RRId;
                newReceivingReceiptItem.POId = receivingReceiptItem.POId;
                newReceivingReceiptItem.ItemId = receivingReceiptItem.ItemId;
                newReceivingReceiptItem.Particulars = receivingReceiptItem.Particulars;
                newReceivingReceiptItem.UnitId = receivingReceiptItem.UnitId;
                newReceivingReceiptItem.Quantity = receivingReceiptItem.Quantity;
                newReceivingReceiptItem.Cost = receivingReceiptItem.Cost;
                newReceivingReceiptItem.Amount = receivingReceiptItem.Amount;
                newReceivingReceiptItem.VATId = receivingReceiptItem.VATId;
                newReceivingReceiptItem.VATPercentage = receivingReceiptItem.VATPercentage;
                newReceivingReceiptItem.VATAmount = receivingReceiptItem.VATAmount;
                newReceivingReceiptItem.WTAXId = receivingReceiptItem.WTAXId;
                newReceivingReceiptItem.WTAXPercentage = receivingReceiptItem.WTAXPercentage;
                newReceivingReceiptItem.WTAXAmount = receivingReceiptItem.WTAXAmount;
                newReceivingReceiptItem.BranchId = receivingReceiptItem.BranchId;
                newReceivingReceiptItem.BaseUnitId = receivingReceiptItem.BaseUnitId;
                newReceivingReceiptItem.BaseQuantity = receivingReceiptItem.BaseQuantity;
                newReceivingReceiptItem.BaseCost = receivingReceiptItem.BaseCost;

                db.TrnReceivingReceiptItems.InsertOnSubmit(newReceivingReceiptItem);
                db.SubmitChanges();

                return newReceivingReceiptItem.Id;

            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
                return 0;
            }
        }

        // =============================
        // UPDATE Receving Retrieve Item
        // =============================
        [Route("api/updateReceivingReceiptItem/{id}")]
        public HttpResponseMessage Put(String id, Models.TrnReceivingReceiptItem receivingReceiptItem)
        {
            try
            {
                var receivingReceiptItemId = Convert.ToInt32(id);
                var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.Id == receivingReceiptItemId select d;

                if (receivingReceiptItems.Any())
                {
                    var updateReceivingReceiptItem = receivingReceiptItems.FirstOrDefault();

                    updateReceivingReceiptItem.RRId = receivingReceiptItem.RRId;
                    updateReceivingReceiptItem.POId = receivingReceiptItem.POId;
                    updateReceivingReceiptItem.ItemId = receivingReceiptItem.ItemId;
                    updateReceivingReceiptItem.Particulars = receivingReceiptItem.Particulars;
                    updateReceivingReceiptItem.UnitId = receivingReceiptItem.UnitId;
                    updateReceivingReceiptItem.Quantity = receivingReceiptItem.Quantity;
                    updateReceivingReceiptItem.Cost = receivingReceiptItem.Cost;
                    updateReceivingReceiptItem.Amount = receivingReceiptItem.Amount;
                    updateReceivingReceiptItem.VATId = receivingReceiptItem.VATId;
                    updateReceivingReceiptItem.VATPercentage = receivingReceiptItem.VATPercentage;
                    updateReceivingReceiptItem.VATAmount = receivingReceiptItem.VATAmount;
                    updateReceivingReceiptItem.WTAXId = receivingReceiptItem.WTAXId;
                    updateReceivingReceiptItem.WTAXPercentage = receivingReceiptItem.WTAXPercentage;
                    updateReceivingReceiptItem.WTAXAmount = receivingReceiptItem.WTAXAmount;
                    updateReceivingReceiptItem.BranchId = receivingReceiptItem.BranchId;
                    updateReceivingReceiptItem.BaseUnitId = receivingReceiptItem.BaseUnitId;
                    updateReceivingReceiptItem.BaseQuantity = receivingReceiptItem.BaseQuantity;
                    updateReceivingReceiptItem.BaseCost = receivingReceiptItem.BaseCost;

                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // =============================
        // DELETE Receving Retrieve Item
        // =============================
        [Route("api/deleteReceivingReceiptItem/{id}")]
        public HttpResponseMessage Delete(String id)
        {
            try
            {
                var receivingReceiptItemId = Convert.ToInt32(id);
                var receivingReceiptItems = from d in db.TrnReceivingReceiptItems where d.Id == receivingReceiptItemId select d;

                if (receivingReceiptItems.Any())
                {
                    db.TrnReceivingReceiptItems.DeleteOnSubmit(receivingReceiptItems.First());
                    db.SubmitChanges();

                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
