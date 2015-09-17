using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class SoftwareController : UserAccountController
    {
        // GET: Software
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Supplier()
        {
            return View();
        }
        public ActionResult PurchaseOrder()
        {
            return View();
        }
        public ActionResult ReceivingReceipt()
        {
            return View();
        }
        public ActionResult AccountsPayable()
        {
            return View();
        }
        public ActionResult Disbursement()
        {
            return View();
        }
        public ActionResult Bank()
        {
            return View();
        }
        public ActionResult Customer()
        {
            return View();
        }
        public ActionResult Sales()
        {
            return View();
        }
        public ActionResult AccountsReceivable()
        {
            return View();
        }
        public ActionResult Collection()
        {
            return View();
        }
        public ActionResult BankReconciliation()
        {
            return View();
        }
        public ActionResult Items()
        {
            return View();
        }
        public ActionResult StockIn()
        {
            return View();
        }
        public ActionResult StockOut()
        {
            return View();
        }
        public ActionResult InventoryReport()
        {
            return View();
        }
        public ActionResult StockTransfer()
        {
            return View();
        }
        public ActionResult SystemTables()
        {
            return View();
        }
        public ActionResult ChartOfAccounts()
        {
            return View();
        }
        public ActionResult JournalVoucher()
        {
            return View();
        }
        public ActionResult Company()
        {
            return View();
        }
        public ActionResult FinancialStatements()
        {
            return View();
        }
        public ActionResult Users()
        {
            return View();
        }
        public ActionResult Settings()
        {
            return View();
        }
    }
}