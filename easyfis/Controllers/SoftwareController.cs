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
                    ViewData.Add("CanAdd", userForm.CanAdd);
                    ViewData.Add("CanEdit", userForm.CanEdit);
                    ViewData.Add("CanDelete", userForm.CanDelete);
                    ViewData.Add("CanLock", userForm.CanLock);
                    ViewData.Add("CanUnlock", userForm.CanUnlock);
                    ViewData.Add("CanPrint", userForm.CanPrint);

                    emptyPageName = userForm.Form;
                    break;
                }
            }

            return emptyPageName;
        }
        
        public String accessToDetail(String page)
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
        public ActionResult Forbidden()
        {
            return View();
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
                if (accessToDetail("SupplierDetail").Equals("SupplierDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }
               
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
                if (accessToDetail("PurchaseOrderDetail").Equals("PurchaseOrderDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }

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
                if (accessToDetail("ReceivingReceiptDetail").Equals("ReceivingReceiptDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }

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
            if (pageAccess("AccountsPayableReport").Equals("AccountsPayableReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult PurchaseSummaryReport()
        {
            if (pageAccess("PurchaseSummaryReport").Equals("PurchaseSummaryReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult PurchaseDetailReport()
        {
            if (pageAccess("PurchaseDetailReport").Equals("PurchaseDetailReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult ReceivingReceiptSummaryReport()
        {
            if (pageAccess("ReceivingReceiptSummaryReport").Equals("ReceivingReceiptSummaryReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult ReceivingReceiptDetailReport()
        {
            if (pageAccess("ReceivingReceiptDetailReport").Equals("ReceivingReceiptDetailReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult DisbursementSummaryReport()
        {
            if (pageAccess("DisbursementSummaryReport").Equals("DisbursementSummaryReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult DisbursementDetailReport()
        {
            if (pageAccess("DisbursementDetailReport").Equals("DisbursementDetailReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult Disbursement()
        {
            if (pageAccess("Disbursement").Equals("Disbursement"))
            {
                if (accessToDetail("DisbursementDetail").Equals("DisbursementDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }
               
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
                if (accessToDetail("CustomerDetail").Equals("CustomerDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }

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
                if (accessToDetail("SalesDetail").Equals("SalesDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }

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
            if (pageAccess("AccountsReceivableReport").Equals("AccountsReceivableReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult SalesSummaryReport()
        {
            if (pageAccess("SalesSummaryReport").Equals("SalesSummaryReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult SalesDetailReport()
        {
            if (pageAccess("SalesDetailReport").Equals("SalesDetailReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult CollectionSummaryReport()
        {
            if (pageAccess("CollectionSummaryReport").Equals("CollectionSummaryReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult CollectionDetailReport()
        {
            if (pageAccess("CollectionDetailReport").Equals("CollectionDetailReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult ReceivingReceiptBook()
        {
            if (pageAccess("ReceivingReceiptBook").Equals("ReceivingReceiptBook"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult DisbursementBook()
        {
            if (pageAccess("DisbursementBook").Equals("DisbursementBook"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult SalesBook()
        {
            if (pageAccess("SalesBook").Equals("SalesBook"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult CollectionBook()
        {
            if (pageAccess("CollectionBook").Equals("CollectionBook"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult StockInBook()
        {
            if (pageAccess("StockInBook").Equals("StockInBook"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        
        }

        [Authorize]
        public ActionResult StockOutBook()
        {
            if (pageAccess("StockOutBook").Equals("StockOutBook"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult StockTransferBook()
        {
            if (pageAccess("StockTransferBook").Equals("StockTransferBook"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult JournalVoucherBook()
        {
            if (pageAccess("JournalVoucherBook").Equals("JournalVoucherBook"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult Collection()
        {
            if (pageAccess("Collection").Equals("Collection"))
            {
                if (accessToDetail("CollectionDetail").Equals("CollectionDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }

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
                if (accessToDetail("ItemDetail").Equals("ItemDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }

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
                if (accessToDetail("StockInDetail").Equals("StockInDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }

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
                if (accessToDetail("StockOutDetail").Equals("StockOutDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }

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
                if (accessToDetail("StockTransferDetail").Equals("StockTransferDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }

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
                if (accessToDetail("StockCountDetail").Equals("StockCountDetail"))
                {
                    ViewData.Add("CanAccessToDetailPage", "True");
                }
                else
                {
                    ViewData.Add("CanAccessToDetailPage", "False");
                }

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
            if (pageAccess("TrialBalance").Equals("TrialBalance"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult AccountLedger()
        {
            if (pageAccess("AccountLedger").Equals("AccountLedger"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
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
    }
}