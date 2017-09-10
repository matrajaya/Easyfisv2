using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class RepStatementOfAccountController : Controller
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

        // ===============================
        // Statement of Account Report PDF
        // ===============================
        [Authorize]
        public ActionResult StatementOfAccount(String DateAsOf, Int32 CompanyId, Int32 BranchId, Int32 CustomerId)
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
            headerPage.AddCell(new PdfPCell(new Phrase("Statement of Account", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
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
                                               && d.CustomerId == Convert.ToInt32(CustomerId)
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
                Decimal totalBalanceAmount = 0;
                Decimal totalCurrentAmount = 0;
                Decimal totalAge30Amount = 0;
                Decimal totalAge60Amount = 0;
                Decimal totalAge90Amount = 0;
                Decimal totalAge120AmountAmount = 0;

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
                    accountTitle.AddCell(new PdfPCell(new Phrase(salesInvoicesGroupedAccount.AccountCode + " - " + salesInvoicesGroupedAccount.Account, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingBottom = 5f });
                    document.Add(accountTitle);

                    // ================================
                    // Sales Invoices Grouped Customers
                    // ================================
                    var salesInvoiceGroupedCustomers = from d in db.TrnSalesInvoices
                                                       where d.MstArticle.MstAccount.Id == salesInvoicesGroupedAccount.AccountId
                                                       && d.SIDate <= Convert.ToDateTime(DateAsOf)
                                                       && d.MstBranch.CompanyId == CompanyId
                                                       && d.BranchId == BranchId
                                                       && d.CustomerId == Convert.ToInt32(CustomerId)
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
                        Decimal accountSubTotalBalanceAmount = 0;
                        Decimal accountSubTotalCurrentAmount = 0;
                        Decimal accountSubTotalAge30Amount = 0;
                        Decimal accountSubTotalAge60Amount = 0;
                        Decimal accountSubTotalAge90Amount = 0;
                        Decimal accountSubTotalAge120AmountAmount = 0;

                        foreach (var salesInvoiceGroupedCustomer in salesInvoiceGroupedCustomers)
                        {
                            // =============
                            // Customer Name
                            // =============
                            PdfPTable supplierName = new PdfPTable(1);
                            float[] widthCellsCustomerName = new float[] { 100f };
                            supplierName.SetWidths(widthCellsCustomerName);
                            supplierName.WidthPercentage = 100;
                            supplierName.AddCell(new PdfPCell(new Phrase(salesInvoiceGroupedCustomer.Customer, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 9f, PaddingBottom = 6f });
                            document.Add(supplierName);

                            // ==================
                            // Sales Invoice Data
                            // ==================
                            PdfPTable data = new PdfPTable(10);
                            float[] widthCellsData = new float[] { 100f, 100f, 120f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                            data.SetWidths(widthCellsData);
                            data.WidthPercentage = 100;
                            data.AddCell(new PdfPCell(new Phrase("SI Number", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            data.AddCell(new PdfPCell(new Phrase("SI Date", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            data.AddCell(new PdfPCell(new Phrase("Document Ref.", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            data.AddCell(new PdfPCell(new Phrase("Due Date", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            data.AddCell(new PdfPCell(new Phrase("Balance", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            data.AddCell(new PdfPCell(new Phrase("Current", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            data.AddCell(new PdfPCell(new Phrase("30 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            data.AddCell(new PdfPCell(new Phrase("60 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            data.AddCell(new PdfPCell(new Phrase("90 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            data.AddCell(new PdfPCell(new Phrase("Over 120 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });

                            // ==================================
                            // Sales Invoice Data (Compute Aging)
                            // ==================================
                            var salesInvoicesComputeAges = from d in db.TrnSalesInvoices
                                                           where d.CustomerId == salesInvoiceGroupedCustomer.CustomerId
                                                           && d.SIDate <= Convert.ToDateTime(DateAsOf)
                                                           && d.MstBranch.CompanyId == CompanyId
                                                           && d.BranchId == BranchId
                                                           && d.CustomerId == Convert.ToInt32(CustomerId)
                                                           && d.BalanceAmount > 0
                                                           && d.IsLocked == true
                                                           select new
                                                           {
                                                               Id = d.Id,
                                                               SINumber = d.SINumber,
                                                               SIDate = d.SIDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture),
                                                               DocumentReference = d.DocumentReference,
                                                               BalanceAmount = d.BalanceAmount,
                                                               DueDate = d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays)).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture),
                                                               NumberOfDaysFromDueDate = Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days,
                                                               CurrentAmount = ComputeAge(0, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                               Age30Amount = ComputeAge(1, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                               Age60Amount = ComputeAge(2, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                               Age90Amount = ComputeAge(3, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                               Age120Amount = ComputeAge(4, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount)
                                                           };

                            if (salesInvoicesComputeAges.Any())
                            {
                                Decimal subTotalBalanceAmount = 0;
                                Decimal subTotalCurrentAmount = 0;
                                Decimal subTotalAge30Amount = 0;
                                Decimal subTotalAge60Amount = 0;
                                Decimal subTotalAge90Amount = 0;
                                Decimal subTotalAge120AmountAmount = 0;

                                foreach (var salesInvoicesWithComputeAge in salesInvoicesComputeAges)
                                {
                                    data.AddCell(new PdfPCell(new Phrase(salesInvoicesWithComputeAge.SINumber, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                    data.AddCell(new PdfPCell(new Phrase(salesInvoicesWithComputeAge.SIDate, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                    data.AddCell(new PdfPCell(new Phrase(salesInvoicesWithComputeAge.DocumentReference, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                    data.AddCell(new PdfPCell(new Phrase(salesInvoicesWithComputeAge.DueDate, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                    data.AddCell(new PdfPCell(new Phrase(salesInvoicesWithComputeAge.BalanceAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                    data.AddCell(new PdfPCell(new Phrase(salesInvoicesWithComputeAge.CurrentAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                    data.AddCell(new PdfPCell(new Phrase(salesInvoicesWithComputeAge.Age30Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                    data.AddCell(new PdfPCell(new Phrase(salesInvoicesWithComputeAge.Age60Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                    data.AddCell(new PdfPCell(new Phrase(salesInvoicesWithComputeAge.Age90Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                    data.AddCell(new PdfPCell(new Phrase(salesInvoicesWithComputeAge.Age120Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });

                                    subTotalBalanceAmount += salesInvoicesWithComputeAge.BalanceAmount;
                                    subTotalCurrentAmount += salesInvoicesWithComputeAge.CurrentAmount;
                                    subTotalAge30Amount += salesInvoicesWithComputeAge.Age30Amount;
                                    subTotalAge60Amount += salesInvoicesWithComputeAge.Age60Amount;
                                    subTotalAge90Amount += salesInvoicesWithComputeAge.Age90Amount;
                                    subTotalAge120AmountAmount += salesInvoicesWithComputeAge.Age120Amount;
                                }

                                // =========
                                // Sub Total
                                // =========
                                data.AddCell(new PdfPCell(new Phrase("Sub Total", fontArial9Bold)) { Colspan = 4, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 10f, PaddingLeft = 10f });
                                data.AddCell(new PdfPCell(new Phrase(subTotalBalanceAmount.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(subTotalCurrentAmount.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(subTotalAge30Amount.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(subTotalAge60Amount.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(subTotalAge90Amount.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });
                                data.AddCell(new PdfPCell(new Phrase(subTotalAge120AmountAmount.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f, PaddingRight = 5f, PaddingLeft = 5f });

                                document.Add(data);

                                accountSubTotalBalanceAmount = accountSubTotalBalanceAmount + subTotalBalanceAmount;
                                accountSubTotalCurrentAmount = accountSubTotalCurrentAmount + subTotalCurrentAmount;
                                accountSubTotalAge30Amount = accountSubTotalAge30Amount + subTotalAge30Amount;
                                accountSubTotalAge60Amount = accountSubTotalAge60Amount + subTotalAge60Amount;
                                accountSubTotalAge90Amount = accountSubTotalAge90Amount + subTotalAge90Amount;
                                accountSubTotalAge120AmountAmount = accountSubTotalAge120AmountAmount + subTotalAge120AmountAmount;
                            }
                        }

                        // ============================
                        // Account Title with Sub Total
                        // ============================
                        PdfPTable accountTitleSubTotal = new PdfPTable(10);
                        float[] widthsCellsAccountTitleSubTotal = new float[] { 100f, 100f, 120f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                        accountTitleSubTotal.SetWidths(widthsCellsAccountTitleSubTotal);
                        accountTitleSubTotal.WidthPercentage = 100;
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(salesInvoicesGroupedAccount.AccountCode + " - " + salesInvoicesGroupedAccount.Account + " Sub Total", fontArial9Bold)) { Colspan = 4, Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 10f, PaddingLeft = 10f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(accountSubTotalBalanceAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(accountSubTotalCurrentAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(accountSubTotalAge30Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(accountSubTotalAge60Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(accountSubTotalAge90Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });
                        accountTitleSubTotal.AddCell(new PdfPCell(new Phrase(accountSubTotalAge120AmountAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 15f, PaddingRight = 5f, PaddingLeft = 5f });

                        totalBalanceAmount = totalBalanceAmount + accountSubTotalBalanceAmount;
                        totalCurrentAmount = totalCurrentAmount + accountSubTotalCurrentAmount;
                        totalAge30Amount = totalAge30Amount + accountSubTotalAge30Amount;
                        totalAge60Amount = totalAge60Amount + accountSubTotalAge60Amount;
                        totalAge90Amount = totalAge90Amount + accountSubTotalAge90Amount;
                        totalAge120AmountAmount = totalAge120AmountAmount + accountSubTotalAge120AmountAmount;

                        document.Add(accountTitleSubTotal);
                    }
                }

                document.Add(line);

                // =====
                // Total
                // =====
                PdfPTable total = new PdfPTable(10);
                float[] widthsCellsTotal = new float[] { 100f, 100f, 120f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                total.SetWidths(widthsCellsTotal);
                total.WidthPercentage = 100;
                total.AddCell(new PdfPCell(new Phrase("Total", fontArial9Bold)) { Colspan = 4, Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 10f, PaddingLeft = 10f });
                total.AddCell(new PdfPCell(new Phrase(totalBalanceAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                total.AddCell(new PdfPCell(new Phrase(totalCurrentAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                total.AddCell(new PdfPCell(new Phrase(totalAge30Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                total.AddCell(new PdfPCell(new Phrase(totalAge60Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                total.AddCell(new PdfPCell(new Phrase(totalAge90Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
                total.AddCell(new PdfPCell(new Phrase(totalAge120AmountAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f, PaddingRight = 5f, PaddingLeft = 5f });
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