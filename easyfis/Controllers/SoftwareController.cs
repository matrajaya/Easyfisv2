using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System;
using System.Diagnostics;


namespace easyfis.Controllers
{
    public class SoftwareController : UserAccountController
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ===========
        // Page Access
        // ===========
        public String PageAccess(String page)
        {
            String form = "";

            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
            if (currentUser.Any())
            {
                var userForms = from d in db.MstUserForms
                                where d.UserId == currentUser.FirstOrDefault().Id
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

                foreach (var userForm in userForms)
                {
                    if (page.Equals(userForm.Form))
                    {
                        ViewData.Add("CanAdd", userForm.CanAdd);
                        ViewData.Add("CanEdit", userForm.CanEdit);
                        ViewData.Add("CanDelete", userForm.CanDelete);
                        ViewData.Add("CanLock", userForm.CanLock);
                        ViewData.Add("CanUnlock", userForm.CanUnlock);
                        ViewData.Add("CanPrint", userForm.CanPrint);

                        form = userForm.Form;
                        break;
                    }
                }
            }

            return form;
        }

        // ========================
        // Formas With Detail Pages
        // ========================
        public String AccessToDetail(String page)
        {
            String form = "";

            var currentUser = from d in db.MstUsers where d.UserId == User.Identity.GetUserId() select d;
            if (currentUser.Any())
            {
                var userForms = from d in db.MstUserForms
                                where d.UserId == currentUser.FirstOrDefault().Id
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

                foreach (var userForm in userForms)
                {
                    if (page.Equals(userForm.Form))
                    {
                        form = userForm.Form;
                        break;
                    }
                }
            }

            return form;
        }

        // ==============
        // Forbidden Page
        // ==============
        [Authorize]
        public ActionResult Forbidden()
        {
            return View();
        }

        // =====================
        // Dashboard - Main Menu
        // =====================
        [Authorize]
        public ActionResult Index()
        {
            if (PageAccess("Software").Equals("Software"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        // ========
        // Supplier
        // ========
        [Authorize]
        public ActionResult Supplier()
        {
            if (PageAccess("SupplierList").Equals("SupplierList"))
            {
                if (AccessToDetail("SupplierDetail").Equals("SupplierDetail"))
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

        // ===============
        // Supplier Detail
        // ===============
        [Authorize]
        public ActionResult SupplierDetail()
        {
            if (PageAccess("SupplierDetail").Equals("SupplierDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        // ========
        // Customer
        // ========
        [Authorize]
        public ActionResult Customer()
        {
            if (PageAccess("CustomerList").Equals("CustomerList"))
            {
                if (AccessToDetail("CustomerDetail").Equals("CustomerDetail"))
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

        // ===============
        // Customer Detail
        // ===============
        [Authorize]
        public ActionResult CustomerDetail()
        {
            if (PageAccess("CustomerDetail").Equals("CustomerDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        // ====
        // Item
        // ====
        [Authorize]
        public ActionResult Item()
        {
            if (PageAccess("ItemList").Equals("ItemList"))
            {
                if (AccessToDetail("ItemDetail").Equals("ItemDetail"))
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

        // ===========
        // Item Detail
        // ===========
        [Authorize]
        public ActionResult ItemDetail()
        {
            if (PageAccess("ItemDetail").Equals("ItemDetail"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        // =================
        // Chart of Accounts
        // =================
        [Authorize]
        public ActionResult ChartOfAccounts()
        {
            if (PageAccess("CharOfAccounts").Equals("CharOfAccounts"))
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
            if (PageAccess("PurchaseOrderList").Equals("PurchaseOrderList"))
            {
                if (AccessToDetail("PurchaseOrderDetail").Equals("PurchaseOrderDetail"))
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
            if (PageAccess("PurchaseOrderDetail").Equals("PurchaseOrderDetail"))
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
            if (PageAccess("ReceivingReceiptList").Equals("ReceivingReceiptList"))
            {
                if (AccessToDetail("ReceivingReceiptDetail").Equals("ReceivingReceiptDetail"))
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
            if (PageAccess("ReceivingReceiptDetail").Equals("ReceivingReceiptDetail"))
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
            if (PageAccess("AccountsPayableReport").Equals("AccountsPayableReport"))
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
            if (PageAccess("ViewAccountsPayableReport").Equals("ViewAccountsPayableReport"))
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
            if (PageAccess("ViewPurchaseSummaryReport").Equals("ViewPurchaseSummaryReport"))
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
            if (PageAccess("ViewPurchaseDetailReport").Equals("ViewPurchaseDetailReport"))
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
            if (PageAccess("ViewReceivingReceiptSummaryReport").Equals("ViewReceivingReceiptSummaryReport"))
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
            if (PageAccess("ViewReceivingReceiptDetailReport").Equals("ViewReceivingReceiptDetailReport"))
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
            if (PageAccess("ViewDisbursementSummaryReport").Equals("ViewDisbursementSummaryReport"))
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
            if (PageAccess("ViewDisbursementDetailReport").Equals("ViewDisbursementDetailReport"))
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
            if (PageAccess("DisbursementList").Equals("DisbursementList"))
            {
                if (AccessToDetail("DisbursementDetail").Equals("DisbursementDetail"))
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
            if (PageAccess("DisbursementDetail").Equals("DisbursementDetail"))
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
            if (PageAccess("BankList").Equals("BankList"))
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
            if (PageAccess("SalesInvoiceList").Equals("SalesInvoiceList"))
            {
                if (AccessToDetail("SalesInvoiceDetail").Equals("SalesInvoiceDetail"))
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
            if (PageAccess("SalesInvoiceDetail").Equals("SalesInvoiceDetail"))
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
            if (PageAccess("AccountsReceivableReport").Equals("AccountsReceivableReport"))
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
            if (PageAccess("ViewAccountsReceivableReport").Equals("ViewAccountsReceivableReport"))
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
            if (PageAccess("ViewSalesSummaryReport").Equals("ViewSalesSummaryReport"))
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
            if (PageAccess("ViewSalesDetailReport").Equals("ViewSalesDetailReport"))
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
            if (PageAccess("ViewCollectionSummaryReport").Equals("ViewCollectionSummaryReport"))
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
            if (PageAccess("ViewCollectionDetailReport").Equals("ViewCollectionDetailReport"))
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
            if (PageAccess("ViewReceivingReceiptBook").Equals("ViewReceivingReceiptBook"))
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
            if (PageAccess("ViewDisbursementBook").Equals("ViewDisbursementBook"))
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
            if (PageAccess("ViewSalesBook").Equals("ViewSalesBook"))
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
            if (PageAccess("ViewCollectionBook").Equals("ViewCollectionBook"))
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
            if (PageAccess("ViewStockInBook").Equals("ViewStockInBook"))
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
            if (PageAccess("ViewStockOutBook").Equals("ViewStockOutBook"))
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
            if (PageAccess("ViewStockTransferBook").Equals("ViewStockTransferBook"))
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
            if (PageAccess("ViewJournalVoucherBook").Equals("ViewJournalVoucherBook"))
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
            if (PageAccess("CollectionList").Equals("CollectionList"))
            {
                if (AccessToDetail("CollectionDetail").Equals("CollectionDetail"))
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
            if (PageAccess("CollectionDetail").Equals("CollectionDetail"))
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
            if (PageAccess("BankReconciliation").Equals("BankReconciliation"))
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
            if (PageAccess("StockInList").Equals("StockInList"))
            {
                if (AccessToDetail("StockInDetail").Equals("StockInDetail"))
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
            if (PageAccess("StockInDetail").Equals("StockInDetail"))
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
            if (PageAccess("StockOutList").Equals("StockOutList"))
            {
                if (AccessToDetail("StockOutDetail").Equals("StockOutDetail"))
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
            if (PageAccess("StockOutDetail").Equals("StockOutDetail"))
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
            if (PageAccess("ViewInventoryReport").Equals("ViewInventoryReport"))
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
            if (PageAccess("InventoryReport").Equals("InventoryReport"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Forbidden", "Software");
            }
        }

        [Authorize]
        public ActionResult StockCard()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockTransfer()
        {
            if (PageAccess("StockTransferList").Equals("StockTransferList"))
            {
                if (AccessToDetail("StockTransferDetail").Equals("StockTransferDetail"))
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
            if (PageAccess("StockTransferDetail").Equals("StockTransferDetail"))
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
            if (PageAccess("StockCountList").Equals("StockCountList"))
            {
                if (AccessToDetail("StockCountDetail").Equals("StockCountDetail"))
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
            if (PageAccess("StockCountDetail").Equals("StockCountDetail"))
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
            if (PageAccess("JournalVoucherList").Equals("JournalVoucherList"))
            {
                if (AccessToDetail("JournalVoucherDetail").Equals("JournalVoucherDetail"))
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
            if (PageAccess("JournalVoucherDetail").Equals("JournalVoucherDetail"))
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
            if (PageAccess("CompanyList").Equals("CompanyList"))
            {
                if (AccessToDetail("CompanyDetail").Equals("CompanyDetail"))
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
            if (PageAccess("CompanyDetail").Equals("CompanyDetail"))
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
            if (PageAccess("FinancialStatementReport").Equals("FinancialStatementReport"))
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
            if (PageAccess("ViewTrialBalance").Equals("ViewTrialBalance"))
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
            if (PageAccess("ViewAccountLedger").Equals("ViewAccountLedger"))
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
            if (PageAccess("UserList").Equals("UserList"))
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
            if (PageAccess("UserDetail").Equals("UserDetail"))
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
            if (PageAccess("SystemTables").Equals("SystemTables"))
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

        [Authorize]
        public ActionResult StockInDetailReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockOutDetailReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockTransferDetailReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult ItemList()
        {
            return View();
        }

        [Authorize]
        public ActionResult ItemComponentList()
        {
            return View();
        }

        [Authorize]
        public ActionResult PhysicalCountSheet()
        {
            return View();
        }
    }
}