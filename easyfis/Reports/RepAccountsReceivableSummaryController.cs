using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class RepAccountsReceivableSummaryController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =============
        // Compute Aging
        // =============
        public Decimal ComputeAge(Int32 Age, Int32 Elapsed, Decimal Amount)
        {
            Decimal returnValue = 0;

            if (Age == 0)
            {
                if (Elapsed < 30)
                {
                    returnValue = Amount;
                }
            }
            else if (Age == 1)
            {
                if (Elapsed >= 30 && Elapsed < 60)
                {
                    returnValue = Amount;
                }
            }
            else if (Age == 2)
            {
                if (Elapsed >= 60 && Elapsed < 90)
                {
                    returnValue = Amount;
                }
            }
            else if (Age == 3)
            {
                if (Elapsed >= 90 && Elapsed < 120)
                {
                    returnValue = Amount;
                }
            }
            else if (Age == 4)
            {
                if (Elapsed >= 120)
                {
                    returnValue = Amount;
                }
            }
            else
            {
                returnValue = 0;
            }

            return returnValue;
        }

        // ======================================
        // Accounst Receivable Summary Report PDF
        // ======================================
        [Authorize]
        public ActionResult AccountsReceivableSummaryReport(String DateAsOf, Int32 CompanyId, Int32 BranchId, Int32 AccountId)
        {

            // ========================
            // PDF settings and Formats
            // ========================
            MemoryStream workStream = new MemoryStream();
            Rectangle rectangle = new Rectangle(PageSize.A3);
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            // ===================
            // Fonts Customization
            // ===================
            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial9Italic = FontFactory.GetFont("Arial", 9, Font.ITALIC);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);

            // ====
            // line
            // ====
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d).FirstOrDefault().Company;
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d).FirstOrDefault().Address;
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d).FirstOrDefault().ContactNumber;

            // =================
            // table main header
            // =================
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Accounts Receivable Summary", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Date as of " + Convert.ToDateTime(DateAsOf).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
            document.Add(line);

            // ===============================
            // Sales Invoices Grouped Accounts
            // ===============================
            var salesInvoicesGroupedAccounts = from d in db.TrnSalesInvoices
                                               where d.SIDate <= Convert.ToDateTime(DateAsOf)
                                               && d.MstBranch.CompanyId == CompanyId
                                               && d.BranchId == BranchId
                                               && d.MstArticle.AccountId == AccountId
                                               && d.BalanceAmount > 0
                                               && d.IsLocked == true
                                               group d by new
                                               {
                                                   AccountId = d.MstArticle.AccountId,
                                                   AccountCode = d.MstArticle.MstAccount.AccountCode,
                                                   Account = d.MstArticle.MstAccount.Account
                                               } into g
                                               select new
                                               {
                                                   AccountId = g.Key.AccountId,
                                                   AccountCode = g.Key.AccountCode,
                                                   Account = g.Key.Account,
                                                   BalanceAmount = g.Sum(d => d.BalanceAmount)
                                               };

            if (salesInvoicesGroupedAccounts.Any())
            {
                Decimal OverAllTotalBalance = 0;
                Decimal OverAllTotalCurrent = 0;
                Decimal OverAllTotal30Days = 0;
                Decimal OverAllTotal60Days = 0;
                Decimal OverAllTotal90Days = 0;
                Decimal OverAllTotalOver120Days = 0;

                foreach (var salesInvoicesGroupedAccount in salesInvoicesGroupedAccounts)
                {
                    // ============
                    // Branch Title 
                    // ============
                    var branch = from d in db.MstBranches where d.Id == BranchId select d;
                    String branchName = "N/A";
                    if (branch.Any())
                    {
                        branchName = branch.FirstOrDefault().Branch;
                    }
                    PdfPTable branchTitle = new PdfPTable(1);
                    float[] widthCellsBranchTitle = new float[] { 100f };
                    branchTitle.SetWidths(widthCellsBranchTitle);
                    branchTitle.WidthPercentage = 100;
                    branchTitle.AddCell(new PdfPCell(new Phrase(branchName, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                    document.Add(branchTitle);

                    // =============
                    // Account Title 
                    // =============
                    PdfPTable accountTitle = new PdfPTable(1);
                    float[] widthCellsAccountTitle = new float[] { 100f };
                    accountTitle.SetWidths(widthCellsAccountTitle);
                    accountTitle.WidthPercentage = 100;
                    accountTitle.AddCell(new PdfPCell(new Phrase(salesInvoicesGroupedAccount.AccountCode + " - " + salesInvoicesGroupedAccount.Account, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingBottom = 14f });
                    document.Add(accountTitle);

                    // ================================
                    // Sales Invoices Grouped Customers
                    // ================================
                    var salesInvoiceGroupedCustomers = from d in db.TrnSalesInvoices
                                                       where d.MstArticle.MstAccount.Id == salesInvoicesGroupedAccount.AccountId
                                                       && d.SIDate <= Convert.ToDateTime(DateAsOf)
                                                       && d.MstBranch.CompanyId == CompanyId
                                                       && d.BranchId == BranchId
                                                       && d.MstArticle.AccountId == AccountId
                                                       && d.BalanceAmount > 0
                                                       && d.IsLocked == true
                                                       group d by new
                                                       {
                                                           CustomerId = d.CustomerId,
                                                           Customer = d.MstArticle.Article
                                                       } into g
                                                       select new
                                                       {
                                                           CustomerId = g.Key.CustomerId,
                                                           Customer = g.Key.Customer,
                                                           BalanceAmount = g.Sum(d => d.BalanceAmount)
                                                       };

                    if (salesInvoiceGroupedCustomers.Any())
                    {
                        // ==================
                        // Sales Invoice Data
                        // ==================
                        PdfPTable data = new PdfPTable(7);
                        float[] widthsCellsData = new float[] { 50f, 15f, 15f, 15f, 15f, 15f, 15f };
                        data.SetWidths(widthsCellsData);
                        data.WidthPercentage = 100;
                        data.AddCell(new PdfPCell(new Phrase("Customer Name", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        data.AddCell(new PdfPCell(new Phrase("Balance", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        data.AddCell(new PdfPCell(new Phrase("Current", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        data.AddCell(new PdfPCell(new Phrase("30 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        data.AddCell(new PdfPCell(new Phrase("60 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        data.AddCell(new PdfPCell(new Phrase("90 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        data.AddCell(new PdfPCell(new Phrase("Over 120 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                        Decimal SubTotalBalance = 0;
                        Decimal SubTotalCurrent = 0;
                        Decimal SubTotal30Days = 0;
                        Decimal SubTotal60Days = 0;
                        Decimal SubTotal90Days = 0;
                        Decimal SubTotalOver120Days = 0;

                        foreach (var salesInvoicesArticleCustomer in salesInvoiceGroupedCustomers.OrderBy(d => d.Customer))
                        {
                            // ==================================
                            // Sales Invoice Data (Compute Aging)
                            // ==================================
                            var salesInvoiceWithComputeAges = from d in db.TrnSalesInvoices
                                                              where d.CustomerId == salesInvoicesArticleCustomer.CustomerId
                                                              && d.MstArticle.MstAccount.Id == salesInvoicesGroupedAccount.AccountId
                                                              && d.SIDate <= Convert.ToDateTime(DateAsOf)
                                                              && d.MstBranch.CompanyId == CompanyId
                                                              && d.BranchId == BranchId
                                                              && d.MstArticle.AccountId == AccountId
                                                              && d.BalanceAmount > 0
                                                              && d.IsLocked == true
                                                              select new Models.TrnSalesInvoice
                                                              {
                                                                  Id = d.Id,
                                                                  Customer = d.MstArticle.Article,
                                                                  BalanceAmount = d.BalanceAmount,
                                                                  DueDate = d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays)).ToShortDateString(),
                                                                  NumberOfDaysFromDueDate = Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days,
                                                                  CurrentAmount = ComputeAge(0, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                  Age30Amount = ComputeAge(1, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                  Age60Amount = ComputeAge(2, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                  Age90Amount = ComputeAge(3, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                  Age120Amount = ComputeAge(4, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount)
                                                              };

                            Decimal totalBalanceAmount = 0;
                            Decimal totalCurrentAmount = 0;
                            Decimal totalAge30Amount = 0;
                            Decimal totalAge60Amount = 0;
                            Decimal totalAge90Amount = 0;
                            Decimal totalAge120AmountAmount = 0;

                            if (salesInvoiceWithComputeAges.Any())
                            {
                                foreach (var salesInvoiceWithComputeAge in salesInvoiceWithComputeAges)
                                {
                                    totalBalanceAmount = totalBalanceAmount + salesInvoiceWithComputeAge.BalanceAmount;
                                    totalCurrentAmount = totalCurrentAmount + salesInvoiceWithComputeAge.CurrentAmount;
                                    totalAge30Amount = totalAge30Amount + salesInvoiceWithComputeAge.Age30Amount;
                                    totalAge60Amount = totalAge60Amount + salesInvoiceWithComputeAge.Age60Amount;
                                    totalAge90Amount = totalAge90Amount + salesInvoiceWithComputeAge.Age90Amount;
                                    totalAge120AmountAmount = totalAge120AmountAmount + salesInvoiceWithComputeAge.Age120Amount;
                                }

                                data.AddCell(new PdfPCell(new Phrase(salesInvoicesArticleCustomer.Customer, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(totalBalanceAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(totalCurrentAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(totalAge30Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(totalAge60Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(totalAge90Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(totalAge120AmountAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });

                                SubTotalBalance = SubTotalBalance + totalBalanceAmount;
                                SubTotalCurrent = SubTotalCurrent + totalCurrentAmount;
                                SubTotal30Days = SubTotal30Days + totalAge30Amount;
                                SubTotal60Days = SubTotal60Days + totalAge60Amount;
                                SubTotal90Days = SubTotal90Days + totalAge90Amount;
                                SubTotalOver120Days = SubTotalOver120Days + totalAge120AmountAmount;
                            }
                        }

                        document.Add(data);

                        // ============================
                        // Account Title with Sub Total
                        // ============================
                        PdfPTable accountTitleSubTotal = new PdfPTable(7);
                        float[] widthsCellsAccountTitleSubTotal = new float[] { 50f, 15f, 15f, 15f, 15f, 15f, 15f };
                        accountTitleSubTotal.SetWidths(widthsCellsAccountTitleSubTotal);
                        accountTitleSubTotal.WidthPercentage = 100;
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(salesInvoicesGroupedAccount.AccountCode + " - " + salesInvoicesGroupedAccount.Account + " Sub Total", fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 10f, PaddingLeft = 10f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(SubTotalBalance.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(SubTotalCurrent.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(SubTotal30Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(SubTotal60Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(SubTotal90Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(SubTotalOver120Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });

                        document.Add(accountTitleSubTotal);

                        OverAllTotalBalance = OverAllTotalBalance + SubTotalBalance;
                        OverAllTotalCurrent = OverAllTotalCurrent + SubTotalCurrent;
                        OverAllTotal30Days = OverAllTotal30Days + SubTotal30Days;
                        OverAllTotal60Days = OverAllTotal60Days + SubTotal60Days;
                        OverAllTotal90Days = OverAllTotal90Days + SubTotal90Days;
                        OverAllTotalOver120Days = OverAllTotalOver120Days + SubTotalOver120Days;
                    }
                }

                document.Add(line);

                // =====
                // Total
                // =====
                PdfPTable total = new PdfPTable(7);
                float[] widthsCellsTotal = new float[] { 50f, 15f, 15f, 15f, 15f, 15f, 15f };
                total.SetWidths(widthsCellsTotal);
                total.WidthPercentage = 100;
                total.AddCell(new PdfPCell(new Phrase("Total", fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 10f, PaddingLeft = 10f });
                total.AddCell(new PdfPCell(new Phrase(OverAllTotalBalance.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                total.AddCell(new PdfPCell(new Phrase(OverAllTotalCurrent.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                total.AddCell(new PdfPCell(new Phrase(OverAllTotal30Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                total.AddCell(new PdfPCell(new Phrase(OverAllTotal60Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                total.AddCell(new PdfPCell(new Phrase(OverAllTotal90Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                total.AddCell(new PdfPCell(new Phrase(OverAllTotalOver120Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                document.Add(total);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}