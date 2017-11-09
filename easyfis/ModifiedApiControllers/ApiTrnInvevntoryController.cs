using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ModifiedApiControllers
{
    public class ApiTrnInvevntoryController : ApiController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ==================================
        // List Inventory - Receiving Receipt
        // ==================================
        [Authorize, HttpGet, Route("api/inventory/receivingReceipt/list/{RRId}")]
        public List<Entities.TrnInventory> ListInventoryReceivingReceipt(String RRId)
        {
            var inventories = from d in db.TrnInventories
                              where d.RRId == Convert.ToUInt32(RRId)
                              select new Entities.TrnInventory
                              {
                                  InventoryDate = d.InventoryDate.ToShortDateString(),
                                  Branch = d.MstBranch.Branch,
                                  Article = d.MstArticle.Article,
                                  Particulars = d.Particulars,
                                  ArticleInventoryCode = d.MstArticleInventory.InventoryCode,
                                  Quantity = d.Quantity,
                                  ArticleUnit = d.MstArticle.MstUnit.Unit,
                                  Amount = d.Amount
                              };

            return inventories.ToList();
        }

        // ==============================
        // List Inventory - Sales Invoice
        // ==============================
        [Authorize, HttpGet, Route("api/inventory/salesInvoice/list/{SIId}")]
        public List<Entities.TrnInventory> ListInventorySalesInvoice(String SIId)
        {
            var inventories = from d in db.TrnInventories
                              where d.SIId == Convert.ToUInt32(SIId)
                              select new Entities.TrnInventory
                              {
                                  InventoryDate = d.InventoryDate.ToShortDateString(),
                                  Branch = d.MstBranch.Branch,
                                  Article = d.MstArticle.Article,
                                  Particulars = d.Particulars,
                                  ArticleInventoryCode = d.MstArticleInventory.InventoryCode,
                                  Quantity = d.Quantity,
                                  ArticleUnit = d.MstArticle.MstUnit.Unit,
                                  Amount = d.Amount
                              };

            return inventories.ToList();
        }
    }
}
