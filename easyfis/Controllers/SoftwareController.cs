using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class SoftwareController : UserAccountController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Supplier()
        {
            return View();
        }

        [Authorize]
        public ActionResult SupplierDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult PurchaseOrder()
        {
            return View();
        }

        [Authorize]
        public ActionResult PurchaseOrderDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReceivingReceipt()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReceivingReceiptDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult AccountsPayable()
        {
            return View();
        }

        [Authorize]
        public ActionResult AccountsPayableReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult PurchaseSummaryReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult PurchaseDetailReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReceivingReceiptSummaryReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReceivingReceiptDetailReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult DisbursementSummaryReport()
        {
            return View();
        } 

        [Authorize]
        public ActionResult Disbursement()
        {
            return View();
        }

        [Authorize]
        public ActionResult DisbursementDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult Bank()
        {
            return View();
        }

        [Authorize]
        public ActionResult Customer()
        {
            return View();
        }

        [Authorize]
        public ActionResult CustomerDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult Sales()
        {
            return View();
        }

        [Authorize]
        public ActionResult SalesDetail()
        {
            return View();
        }
      
        [Authorize]
        public ActionResult AccountsReceivable()
        {
            return View();
        }

        [Authorize]
        public ActionResult AccountsReceivableReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult Collection()
        {
            return View();
        }

        [Authorize]
        public ActionResult CollectionDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult BankReconciliation()
        {
            return View();
        }

        [Authorize]
        public ActionResult Items()
        {
            return View();
        }

        [Authorize]
        public ActionResult ItemDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockIn()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockInDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockOut()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockOutDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult InventoryReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult Inventory()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockCard()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockTransfer()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockTransferDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockCount()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockCountDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChartOfAccounts()
        {
            return View();
        }

        [Authorize]
        public ActionResult JournalVoucher()
        {
            return View();
        }

        [Authorize]
        public ActionResult JournalVoucherDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult Company()
        {
            return View();
        }

        [Authorize]
        public ActionResult CompanyDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult FinancialStatements()
        {
            return View();
        }

        [Authorize]
        public ActionResult TrialBalance()
        {
            return View();
        }

        [Authorize]
        public ActionResult AccountLedger()
        {
            return View();
        }

        [Authorize]
        public ActionResult Users()
        {
            return View();
        }

        [Authorize]
        public ActionResult UsersDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult SystemTables()
        {
            return View();
        }
    }
}