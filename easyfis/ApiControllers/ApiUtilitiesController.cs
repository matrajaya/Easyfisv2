using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace easyfis.ApiControllers
{
    public class ApiUtilitiesController : ApiController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // DELETE All Transactions
        [Authorize]
        [HttpDelete]
        [Route("api/utilities/deleteAllTransactions")]
        public HttpResponseMessage deleteAllInventories()
        {
            try
            {
                var Inventories = db.TrnInventories.ToList();
                foreach (var Inventory in Inventories)
                {
                    db.TrnInventories.DeleteOnSubmit(Inventory);
                    db.SubmitChanges();
                }

                var stockTransferItems = db.TrnStockTransferItems.ToList();
                foreach (var stockTransferItem in stockTransferItems)
                {
                    db.TrnStockTransferItems.DeleteOnSubmit(stockTransferItem);
                    db.SubmitChanges();
                }

                var stockTransfers = db.TrnStockTransfers.ToList();
                foreach (var stockTransfer in stockTransfers)
                {
                    db.TrnStockTransfers.DeleteOnSubmit(stockTransfer);
                    db.SubmitChanges();
                }

                var stockOutItems = db.TrnStockOutItems.ToList();
                foreach (var stockOutItem in stockOutItems)
                {
                    db.TrnStockOutItems.DeleteOnSubmit(stockOutItem);
                    db.SubmitChanges();
                }

                var stockOuts = db.TrnStockOuts.ToList();
                foreach (var stockOut in stockOuts)
                {
                    db.TrnStockOuts.DeleteOnSubmit(stockOut);
                    db.SubmitChanges();
                }

                var stockInItems = db.TrnStockInItems.ToList();
                foreach (var stockInItem in stockInItems)
                {
                    db.TrnStockInItems.DeleteOnSubmit(stockInItem);
                    db.SubmitChanges();
                }

                var stockIns = db.TrnStockIns.ToList();
                foreach (var stockIn in stockIns)
                {
                    db.TrnStockIns.DeleteOnSubmit(stockIn);
                    db.SubmitChanges();
                }

                var stockCountItems = db.TrnStockCountItems.ToList();
                foreach (var stockCountItem in stockCountItems)
                {
                    db.TrnStockCountItems.DeleteOnSubmit(stockCountItem);
                    db.SubmitChanges();
                }

                var stockCounts = db.TrnStockCounts.ToList();
                foreach (var stockCount in stockCounts)
                {
                    db.TrnStockCounts.DeleteOnSubmit(stockCount);
                    db.SubmitChanges();
                }

                var journals = db.TrnJournals.ToList();
                foreach (var journal in journals)
                {
                    db.TrnJournals.DeleteOnSubmit(journal);
                    db.SubmitChanges();
                }

                var journalVoucherLines = db.TrnJournalVoucherLines.ToList();
                foreach (var journalVoucherLine in journalVoucherLines)
                {
                    db.TrnJournalVoucherLines.DeleteOnSubmit(journalVoucherLine);
                    db.SubmitChanges();
                }

                var journalVouchers = db.TrnJournalVouchers.ToList();
                foreach (var journalVoucher in journalVouchers)
                {
                    db.TrnJournalVouchers.DeleteOnSubmit(journalVoucher);
                    db.SubmitChanges();
                }

                var collectionLines = db.TrnCollectionLines.ToList();
                foreach (var collectionLine in collectionLines)
                {
                    db.TrnCollectionLines.DeleteOnSubmit(collectionLine);
                    db.SubmitChanges();
                }

                var collections = db.TrnCollections.ToList();
                foreach (var collection in collections)
                {
                    db.TrnCollections.DeleteOnSubmit(collection);
                    db.SubmitChanges();
                }

                var disbursementLines = db.TrnDisbursementLines.ToList();
                foreach (var disbursementLine in disbursementLines)
                {
                    db.TrnDisbursementLines.DeleteOnSubmit(disbursementLine);
                    db.SubmitChanges();
                }

                var disbursements = db.TrnDisbursements.ToList();
                foreach (var disbursement in disbursements)
                {
                    db.TrnDisbursements.DeleteOnSubmit(disbursement);
                    db.SubmitChanges();
                }

                var salesInvoiceItems = db.TrnSalesInvoiceItems.ToList();
                foreach (var salesInvoiceItem in salesInvoiceItems)
                {
                    db.TrnSalesInvoiceItems.DeleteOnSubmit(salesInvoiceItem);
                    db.SubmitChanges();
                }

                var salesInvoices = db.TrnSalesInvoices.ToList();
                foreach (var salesInvoice in salesInvoices)
                {
                    db.TrnSalesInvoices.DeleteOnSubmit(salesInvoice);
                    db.SubmitChanges();
                }

                var receivingReceiptItems = db.TrnReceivingReceiptItems.ToList();
                foreach (var receivingReceiptItem in receivingReceiptItems)
                {
                    db.TrnReceivingReceiptItems.DeleteOnSubmit(receivingReceiptItem);
                    db.SubmitChanges();
                }

                var receivingReceipts = db.TrnReceivingReceipts.ToList();
                foreach (var receivingReceipt in receivingReceipts)
                {
                    db.TrnReceivingReceipts.DeleteOnSubmit(receivingReceipt);
                    db.SubmitChanges();
                }

                var purchaseOrderItems = db.TrnPurchaseOrderItems.ToList();
                foreach (var purchaseOrderItem in purchaseOrderItems)
                {
                    db.TrnPurchaseOrderItems.DeleteOnSubmit(purchaseOrderItem);
                    db.SubmitChanges();
                }

                var purchaseOrders = db.TrnPurchaseOrders.ToList();
                foreach (var purchaseOrder in purchaseOrders)
                {
                    db.TrnPurchaseOrders.DeleteOnSubmit(purchaseOrder);
                    db.SubmitChanges();
                }

                var articleInventories = db.MstArticleInventories.ToList();
                foreach (var articleInventory in articleInventories)
                {
                    db.MstArticleInventories.DeleteOnSubmit(articleInventory);
                    db.SubmitChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }
}
