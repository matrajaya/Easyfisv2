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
            if (pageAccess("SupplierList").Equals("SupplierList"))
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
            if (pageAccess("PurchaseOrderList").Equals("PurchaseOrderList"))
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
            if (pageAccess("ReceivingReceiptList").Equals("ReceivingReceiptList"))
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
        public ActionResult AccountsPayableReport()
        {
            if (pageAccess("ViewAccountsPayableReport").Equals("ViewAccountsPayableReport"))
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
            if (pageAccess("ViewPurchaseSummaryReport").Equals("ViewPurchaseSummaryReport"))
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
            if (pageAccess("ViewPurchaseDetailReport").Equals("ViewPurchaseDetailReport"))
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
            if (pageAccess("ViewReceivingReceiptSummaryReport").Equals("ViewReceivingReceiptSummaryReport"))
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
            if (pageAccess("ViewReceivingReceiptDetailReport").Equals("ViewReceivingReceiptDetailReport"))
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
            if (pageAccess("ViewDisbursementSummaryReport").Equals("ViewDisbursementSummaryReport"))
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
            if (pageAccess("ViewDisbursementDetailReport").Equals("ViewDisbursementDetailReport"))
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
            if (pageAccess("DisbursementList").Equals("DisbursementList"))
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
            if (pageAccess("BankList").Equals("BankList"))
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
            if (pageAccess("CustomerList").Equals("CustomerList"))
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
            if (pageAccess("SalesInvoiceList").Equals("SalesInvoiceList"))
            {
                if (accessToDetail("SalesInvoiceDetail").Equals("SalesInvoiceDetail"))
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
            if (pageAccess("SalesInvoiceDetail").Equals("SalesInvoiceDetail"))
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
        public ActionResult AccountsReceivableReport()
        {
            if (pageAccess("ViewAccountsReceivableReport").Equals("ViewAccountsReceivableReport"))
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
            if (pageAccess("ViewSalesSummaryReport").Equals("ViewSalesSummaryReport"))
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
            if (pageAccess("ViewSalesDetailReport").Equals("ViewSalesDetailReport"))
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
            if (pageAccess("ViewCollectionSummaryReport").Equals("ViewCollectionSummaryReport"))
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
            if (pageAccess("ViewCollectionDetailReport").Equals("ViewCollectionDetailReport"))
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
            if (pageAccess("ViewReceivingReceiptBook").Equals("ViewReceivingReceiptBook"))
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
            if (pageAccess("ViewDisbursementBook").Equals("ViewDisbursementBook"))
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
            if (pageAccess("ViewSalesBook").Equals("ViewSalesBook"))
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
            if (pageAccess("ViewCollectionBook").Equals("ViewCollectionBook"))
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
            if (pageAccess("ViewStockInBook").Equals("ViewStockInBook"))
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
            if (pageAccess("ViewStockOutBook").Equals("ViewStockOutBook"))
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
            if (pageAccess("ViewStockTransferBook").Equals("ViewStockTransferBook"))
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
            if (pageAccess("ViewJournalVoucherBook").Equals("ViewJournalVoucherBook"))
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
            if (pageAccess("CollectionList").Equals("CollectionList"))
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
        public ActionResult Item()
        {
            if (pageAccess("ItemList").Equals("ItemList"))
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
            if (pageAccess("StockInList").Equals("StockInList"))
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
            if (pageAccess("StockOutList").Equals("StockOutList"))
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
            if (pageAccess("StockTransferList").Equals("StockTransferList"))
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
            if (pageAccess("StockCountList").Equals("StockCountList"))
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
            if (pageAccess("CharOfAccounts").Equals("CharOfAccounts"))
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
            if (pageAccess("JournalVoucherList").Equals("JournalVoucherList"))
            {
                if (accessToDetail("JournalVoucherDetail").Equals("JournalVoucherDetail"))
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
            if (pageAccess("CompanyList").Equals("CompanyList"))
            {
                if (accessToDetail("CompanyDetail").Equals("CompanyDetail"))
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
            if (pageAccess("FinancialStatementReport").Equals("FinancialStatementReport"))
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
            if (pageAccess("ViewTrialBalance").Equals("ViewTrialBalance"))
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
            if (pageAccess("ViewAccountLedger").Equals("ViewAccountLedger"))
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
            if (pageAccess("UserList").Equals("UserList"))
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
            if (pageAccess("UserDetail").Equals("UserDetail"))
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

        // POS Integration Reports
        // ================================
        [Authorize]
        public ActionResult SalesDetailReportVATSales()
        {
            return View();
        }

        [Authorize]
        public ActionResult SalesSummaryReportSalesNo()
        {
            return View();
        }

        [Authorize]
        public ActionResult CancelledSalesSummaryReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult SeniorCitizenSalesSummaryReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult TopSellingItemReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult SalesSummaryReportAllFields()
        {
            return View();
        }

        [Authorize]
        public ActionResult HourlyTopSellingItemsReport()
        {
            return View();
        }

        // charts 

        [Authorize]
        public ActionResult ChartMonthlySalesTrend()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChartMonthlySalesComparison()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChartYearlySalesTrendComparison()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChartHourlySalesComparison()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChartHourlySalesComparisonAmount()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChartCategorySoldComparison()
        {
            return View();
        }
    }
}