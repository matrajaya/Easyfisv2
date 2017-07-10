using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace easyfis.Entities
{
    public class TrnInventory
    {
        public Int32 Id { get; set; }
        public Int32 BranchId { get; set; }
        public String Branch { get; set; }
        public List<Entities.MstBranch> BranchList { get; set; }
        public String InventoryDate { get; set; }
        public Int32 ArticleId { get; set; }
        public String Article { get; set; }
        public List<Entities.MstArticle> ArticleList { get; set; }
        public Int32 ArticleInventoryId { get; set; }
        public String ArticleInventoryCode { get; set; }
        public List<Entities.MstArticleInventory> ArticleInventoryList { get; set; }
        public Int32 RRId { get; set; }
        public String RRNumber { get; set; }
        public String RRDate { get; set; }
        public List<Entities.TrnReceivingReceipt> RRList { get; set; }
        public Int32 SIId { get; set; }
        public String SINumber { get; set; }
        public String SIDate { get; set; }
        public List<Entities.TrnSalesInvoice> SIList { get; set; }
        public Int32 INId { get; set; }
        public String INNumber { get; set; }
        public String INDate { get; set; }
        public List<Entities.TrnStockIn> INList { get; set; }
        public Int32 OTId { get; set; }
        public String OTNumber { get; set; }
        public String OTDate { get; set; }
        public List<Entities.TrnStockOut> OTList { get; set; }
        public Int32 STId { get; set; }
        public String STNumber { get; set; }
        public String STDate { get; set; }
        public List<Entities.TrnStockTransfer> STList { get; set; }
        public Decimal QuantityIn { get; set; }
        public Decimal QuantityOut { get; set; }
        public Decimal Quantity { get; set; }
        public Decimal Amount { get; set; }
        public String Particulars { get; set; }
    }
}