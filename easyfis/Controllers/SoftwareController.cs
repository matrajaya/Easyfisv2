using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;


namespace easyfis.Controllers
{
    public class SoftwareController : UserAccountController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        public String pageAccess(String page)
        {
            var userId = (from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d.Id).SingleOrDefault();
            var userForms = from d in db.MstUserForms
                            where d.UserId == userId
                            select new Models.MstUserForm
                            {
                                Id = d.Id,
                                UserId = d.UserId,
                                User = d.MstUser.FullName,
                                FormId = d.FormId,
                                Form = d.SysForm.FormName,
                                Particulars = d.SysForm.Particulars,
                                CanAdd = d.CanAdd,
                                CanEdit = d.CanEdit,
                                CanDelete = d.CanDelete,
                                CanLock = d.CanLock,
                                CanUnlock = d.CanUnlock,
                                CanPrint = d.CanPrint
                            };

            String pageName = page;
            String emptyPageName = "";

            foreach (var userForm in userForms)
            {
                if (pageName.Equals(userForm.Form))
                {
                    emptyPageName = userForm.Form;
                    break;
                }
            }

            return emptyPageName;
        }

        [Authorize]
        public ActionResult Index()
        {
            if (pageAccess("Software").Equals("Software"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult Supplier()
        {
            if (pageAccess("Supplier").Equals("Supplier"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult SupplierDetail()
        {
            if (pageAccess("SupplierDetail").Equals("SupplierDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult PurchaseOrder()
        {
            if (pageAccess("PurchaseOrder").Equals("PurchaseOrder"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult PurchaseOrderDetail()
        {
            if (pageAccess("PurchaseOrderDetail").Equals("PurchaseOrderDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult ReceivingReceipt()
        {
            if (pageAccess("ReceivingReceipt").Equals("ReceivingReceipt"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult ReceivingReceiptDetail()
        {
            if (pageAccess("ReceivingReceiptDetail").Equals("ReceivingReceiptDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult AccountsPayable()
        {
            if (pageAccess("AccountsPayable").Equals("AccountsPayable"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
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
        public ActionResult DisbursementDetailReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult Disbursement()
        {
            if (pageAccess("Disbursement").Equals("Disbursement"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult DisbursementDetail()
        {
            if (pageAccess("DisbursementDetail").Equals("DisbursementDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult Bank()
        {
            if (pageAccess("Bank").Equals("Bank"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult Customer()
        {
            if (pageAccess("Customer").Equals("Customer"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult CustomerDetail()
        {
            if (pageAccess("CustomerDetail").Equals("CustomerDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult Sales()
        {
            if (pageAccess("Sales").Equals("Sales"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult SalesDetail()
        {
            if (pageAccess("SalesDetail").Equals("SalesDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult AccountsReceivable()
        {
            if (pageAccess("AccountsReceivable").Equals("AccountsReceivable"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult AccountsReceivableReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult SalesSummaryReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult SalesDetailReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult CollectionSummaryReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult CollectionDetailReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult Collection()
        {
            if (pageAccess("Collection").Equals("Collection"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult CollectionDetail()
        {
            if (pageAccess("CollectionDetail").Equals("CollectionDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult BankReconciliation()
        {
            if (pageAccess("BankReconciliation").Equals("BankReconciliation"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult Items()
        {
            if (pageAccess("Items").Equals("Items"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult ItemDetail()
        {
            if (pageAccess("ItemDetail").Equals("ItemDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult StockIn()
        {
            if (pageAccess("StockIn").Equals("StockIn"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult StockInDetail()
        {
            if (pageAccess("StockInDetail").Equals("StockInDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult StockOut()
        {
            if (pageAccess("StockOut").Equals("StockOut"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult StockOutDetail()
        {
            if (pageAccess("StockOutDetail").Equals("StockOutDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult InventoryReport()
        {
            if (pageAccess("InventoryReport").Equals("InventoryReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
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
            if (pageAccess("StockTransfer").Equals("StockTransfer"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult StockTransferDetail()
        {
            if (pageAccess("StockTransferDetail").Equals("StockTransferDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult StockCount()
        {
            if (pageAccess("StockCount").Equals("StockCount"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult StockCountDetail()
        {
            if (pageAccess("StockCountDetail").Equals("StockCountDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult ChartOfAccounts()
        {
            if (pageAccess("ChartOfAccounts").Equals("ChartOfAccounts"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult JournalVoucher()
        {
            if (pageAccess("JournalVoucher").Equals("JournalVoucher"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult JournalVoucherDetail()
        {
            if (pageAccess("JournalVoucherDetail").Equals("JournalVoucherDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult Company()
        {
            if (pageAccess("Company").Equals("Company"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult CompanyDetail()
        {
            if (pageAccess("CompanyDetail").Equals("CompanyDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult FinancialStatements()
        {
            if (pageAccess("FinancialStatements").Equals("FinancialStatements"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
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
            if (pageAccess("Users").Equals("Users"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult UsersDetail()
        {
            if (pageAccess("UsersDetail").Equals("UsersDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult SystemTables()
        {
            if (pageAccess("SystemTables").Equals("SystemTables"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult Forbidden()
        {
            return View();
        }
    }
}