using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class RepAccountsReceivableController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // Compute age
        public Decimal computeAge(Int32 Age, Int32 Elapsed, Decimal Amount)
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

        // PDF Accounts Payable
        [Authorize]
        public ActionResult AccountsReceivable(String DateAsOf, Int32 CompanyId)
        {
            // PDF settings
            MemoryStream workStream = new MemoryStream();
            Rectangle rectangle = new Rectangle(PageSize.A3);
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial9Italic = FontFactory.GetFont("Arial", 9, Font.ITALIC);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);

            // line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // table main header
            PdfPTable tableHeaderPage = new PdfPTable(2);
            float[] widthsCellsheaderPage = new float[] { 100f, 75f };
            tableHeaderPage.SetWidths(widthsCellsheaderPage);
            tableHeaderPage.WidthPercentage = 100;
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Accounts Receivable", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date as of " + DateAsOf, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // RR for Accounts
            var salesInvoiceAccounts = from d in db.TrnSalesInvoices
                                       where d.SIDate <= Convert.ToDateTime(DateAsOf)
                                       && d.MstBranch.CompanyId == CompanyId
                                       && d.BalanceAmount > 0
                                       group d by new
                                       {
                                           AccountId = d.MstArticle.AccountId,
                                           AccountCode = d.MstArticle.MstAccount.AccountCode,
                                           Account = d.MstArticle.MstAccount.Account
                                       } into g
                                       select new Models.TrnSalesInvoice
                                       {
                                           AccountId = g.Key.AccountId,
                                           AccountCode = g.Key.AccountCode,
                                           Account = g.Key.Account,
                                           BalanceAmount = g.Sum(d => d.BalanceAmount)
                                       };

            Decimal totalBalanceAmount = 0;
            Decimal totalCurrentAmount = 0;
            Decimal totalAge30Amount = 0;
            Decimal totalAge60Amount = 0;
            Decimal totalAge90Amount = 0;
            Decimal totalAge120AmountAmount = 0;

            if (salesInvoiceAccounts.Any())
            {
                foreach (var salesInvoiceAccount in salesInvoiceAccounts)
                {
                    // table SI for account header
                    PdfPTable tableSalesInvoiceAccountHeader = new PdfPTable(1);
                    float[] widthCellsTableSalesInvoiceAccountHeader = new float[] { 100f };
                    tableSalesInvoiceAccountHeader.SetWidths(widthCellsTableSalesInvoiceAccountHeader);
                    tableSalesInvoiceAccountHeader.WidthPercentage = 100;
                    tableSalesInvoiceAccountHeader.AddCell(new PdfPCell(new Phrase(salesInvoiceAccount.AccountCode + " - " + salesInvoiceAccount.Account, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    document.Add(tableSalesInvoiceAccountHeader);

                    // sales invoice for customers
                    var salesInvoiceArticleCustomers = from d in db.TrnSalesInvoices
                                                       where d.MstArticle.MstAccount.Id == salesInvoiceAccount.AccountId
                                                       && d.SIDate <= Convert.ToDateTime(DateAsOf)
                                                       && d.MstBranch.CompanyId == CompanyId
                                                       && d.BalanceAmount > 0
                                                       && d.IsLocked == true
                                                       group d by new
                                                       {
                                                           CustomerId = d.CustomerId,
                                                           Customer = d.MstArticle.Article
                                                       } into g
                                                       select new Models.TrnSalesInvoice
                                                       {
                                                           CustomerId = g.Key.CustomerId,
                                                           Customer = g.Key.Customer,
                                                           BalanceAmount = g.Sum(d => d.BalanceAmount)
                                                       };

                    Decimal accountSubTotalBalanceAmount = 0;
                    Decimal accountSubTotalCurrentAmount = 0;
                    Decimal accountSubTotalAge30Amount = 0;
                    Decimal accountSubTotalAge60Amount = 0;
                    Decimal accountSubTotalAge90Amount = 0;
                    Decimal accountSubTotalAge120AmountAmount = 0;

                    if (salesInvoiceArticleCustomers.Any())
                    {
                        foreach (var salesInvoiceArticleCustomer in salesInvoiceArticleCustomers)
                        {
                            // table SI Customer header
                            PdfPTable tableSalesInvoiceCustomerHeader = new PdfPTable(1);
                            float[] widthCellsTableSalesInvoiceCustomerHeader = new float[] { 100f };
                            tableSalesInvoiceCustomerHeader.SetWidths(widthCellsTableSalesInvoiceCustomerHeader);
                            tableSalesInvoiceCustomerHeader.WidthPercentage = 100;
                            tableSalesInvoiceCustomerHeader.AddCell(new PdfPCell(new Phrase(salesInvoiceArticleCustomer.Customer, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f });
                            document.Add(tableSalesInvoiceCustomerHeader);

                            // table SI Data
                            PdfPTable tableData = new PdfPTable(10);
                            float[] widthsCellsData = new float[] { 100f, 100f, 120f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                            tableData.SetWidths(widthsCellsData);
                            tableData.WidthPercentage = 100;
                            tableData.AddCell(new PdfPCell(new Phrase("SI Number", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("SI Date", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Document Ref.", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Due Date", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Balance", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Current", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("30 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("60 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("90 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Over 120 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });

                            // SI
                            var salesInvoicessWithComputeAges = from d in db.TrnSalesInvoices
                                                                where d.CustomerId == salesInvoiceArticleCustomer.CustomerId
                                                                && d.SIDate <= Convert.ToDateTime(DateAsOf)
                                                                && d.MstBranch.CompanyId == CompanyId
                                                                && d.BalanceAmount > 0
                                                                && d.IsLocked == true
                                                                select new Models.TrnSalesInvoice
                                                                {
                                                                    Id = d.Id,
                                                                    SINumber = d.SINumber,
                                                                    SIDate = d.SIDate.ToShortDateString(),
                                                                    DocumentReference = d.DocumentReference,
                                                                    BalanceAmount = d.BalanceAmount,

                                                                    DueDate = d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays)).ToShortDateString(),
                                                                    NumberOfDaysFromDueDate = Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days,
                                                                    CurrentAmount = computeAge(0, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                    Age30Amount = computeAge(1, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                    Age60Amount = computeAge(2, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                    Age90Amount = computeAge(3, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                    Age120Amount = computeAge(4, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount)
                                                                };

                            Decimal subTotalBalanceAmount = 0;
                            Decimal subTotalCurrentAmount = 0;
                            Decimal subTotalAge30Amount = 0;
                            Decimal subTotalAge60Amount = 0;
                            Decimal subTotalAge90Amount = 0;
                            Decimal subTotalAge120AmountAmount = 0;

                            if (salesInvoicessWithComputeAges.Any())
                            {
                                foreach (var salesInvoicessWithComputeAge in salesInvoicessWithComputeAges)
                                {
                                    tableData.AddCell(new PdfPCell(new Phrase(salesInvoicessWithComputeAge.SINumber, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(salesInvoicessWithComputeAge.SIDate, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(salesInvoicessWithComputeAge.DocumentReference, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(salesInvoicessWithComputeAge.DueDate, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(salesInvoicessWithComputeAge.BalanceAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(salesInvoicessWithComputeAge.CurrentAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(salesInvoicessWithComputeAge.Age30Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(salesInvoicessWithComputeAge.Age60Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(salesInvoicessWithComputeAge.Age90Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(salesInvoicessWithComputeAge.Age120Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });

                                    subTotalBalanceAmount = subTotalBalanceAmount + salesInvoicessWithComputeAge.BalanceAmount;
                                    subTotalCurrentAmount = subTotalCurrentAmount + salesInvoicessWithComputeAge.CurrentAmount;
                                    subTotalAge30Amount = subTotalAge30Amount + salesInvoicessWithComputeAge.Age30Amount;
                                    subTotalAge60Amount = subTotalAge60Amount + salesInvoicessWithComputeAge.Age60Amount;
                                    subTotalAge90Amount = subTotalAge90Amount + salesInvoicessWithComputeAge.Age90Amount;
                                    subTotalAge120AmountAmount = subTotalAge120AmountAmount + salesInvoicessWithComputeAge.Age120Amount;
                                }
                            }

                            document.Add(tableData);

                            // table Sub Total Footer
                            PdfPTable tableSubTotalFooter = new PdfPTable(10);
                            float[] widthsCellsSubTotalFooter = new float[] { 100f, 100f, 120f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                            tableSubTotalFooter.SetWidths(widthsCellsSubTotalFooter);
                            tableSubTotalFooter.WidthPercentage = 100;
                            tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                            tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, });
                            tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                            tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalBalanceAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalCurrentAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalAge30Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalAge60Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalAge90Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalAge120AmountAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            document.Add(tableSubTotalFooter);

                            accountSubTotalBalanceAmount = accountSubTotalBalanceAmount + subTotalBalanceAmount;
                            accountSubTotalCurrentAmount = accountSubTotalCurrentAmount + subTotalCurrentAmount;
                            accountSubTotalAge30Amount = accountSubTotalAge30Amount + subTotalAge30Amount;
                            accountSubTotalAge60Amount = accountSubTotalAge60Amount + subTotalAge60Amount;
                            accountSubTotalAge90Amount = accountSubTotalAge90Amount + subTotalAge90Amount;
                            accountSubTotalAge120AmountAmount = accountSubTotalAge120AmountAmount + subTotalAge120AmountAmount;

                        }
                    }

                    document.Add(Chunk.NEWLINE);
                    document.Add(line);

                    // Table Account Footer Total
                    PdfPTable tableTotalSalesInvoiceAccountFooter = new PdfPTable(10);
                    float[] widthsCellsTotalSalesInvoiceAccountFooter = new float[] { 10f, 40f, 55f, 315f, 100f, 100f, 100f, 100f, 100f, 100f };
                    tableTotalSalesInvoiceAccountFooter.SetWidths(widthsCellsTotalSalesInvoiceAccountFooter);
                    tableTotalSalesInvoiceAccountFooter.WidthPercentage = 100;
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase(salesInvoiceAccount.AccountCode + " - " + salesInvoiceAccount.Account + " Sub Total", fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalBalanceAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalCurrentAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalAge30Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalAge60Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalAge90Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalAge120AmountAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Italic)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Italic)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Italic)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("Book Balance", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("0.00", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("Variance", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("0.00", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("0.00", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("0.00", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalSalesInvoiceAccountFooter.AddCell(new PdfPCell(new Phrase("0.00", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    totalBalanceAmount = totalBalanceAmount + accountSubTotalBalanceAmount;
                    totalCurrentAmount = totalCurrentAmount + accountSubTotalCurrentAmount;
                    totalAge30Amount = totalAge30Amount + accountSubTotalAge30Amount;
                    totalAge60Amount = totalAge60Amount + accountSubTotalAge60Amount;
                    totalAge90Amount = totalAge90Amount + accountSubTotalAge90Amount;
                    totalAge120AmountAmount = totalAge120AmountAmount + accountSubTotalAge120AmountAmount;

                    document.Add(tableTotalSalesInvoiceAccountFooter);
                    document.Add(Chunk.NEWLINE);
                }

                document.Add(Chunk.NEWLINE);
                document.Add(line);

                // Table Total Footer
                PdfPTable tableTotalFooter = new PdfPTable(10);
                float[] widthsCellsTotalFooter = new float[] { 100f, 100f, 120f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                tableTotalFooter.SetWidths(widthsCellsTotalFooter);
                tableTotalFooter.WidthPercentage = 100;
                tableTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(totalBalanceAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(totalCurrentAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(totalAge30Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(totalAge60Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(totalAge90Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(totalAge120AmountAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                document.Add(tableTotalFooter);
            }

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}