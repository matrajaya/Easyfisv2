using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class RepAccountsReceivableSummaryController : Controller
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

        // PDF Accounts Receivable Summary Report
        [Authorize]
        public ActionResult AccountsReceivableSummary(String DateAsOf, Int32 CompanyId)
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
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);

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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Accounts Receivable Summary", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date as of " + DateAsOf, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // SI for Accounts
            var salesInvoiceAccounts = from d in db.TrnSalesInvoices
                                       where d.SIDate <= Convert.ToDateTime(DateAsOf)
                                       && d.MstBranch.CompanyId == CompanyId
                                       && d.BalanceAmount > 0
                                       && d.IsLocked == true
                                       group d by new
                                       {
                                           AccountId = d.MstArticle.AccountId,
                                           AccountCode = d.MstArticle.MstAccount.AccountCode,
                                           Account = d.MstArticle.MstAccount.Account
                                       } into g
                                       select new Models.TrnReceivingReceipt
                                       {
                                           AccountId = g.Key.AccountId,
                                           AccountCode = g.Key.AccountCode,
                                           Account = g.Key.Account,
                                           BalanceAmount = g.Sum(d => d.BalanceAmount)
                                       };

            Decimal OverAllTotalBalance = 0;
            Decimal OverAllTotalCurrent = 0;
            Decimal OverAllTotal30Days = 0;
            Decimal OverAllTotal60Days = 0;
            Decimal OverAllTotal90Days = 0;
            Decimal OverAllTotalOver120Days = 0;

            if (salesInvoiceAccounts.Any())
            {
                foreach (var salesInvoiceAccount in salesInvoiceAccounts)
                {
                    // table SI account header
                    PdfPTable tableSalesInvoiceAccountHeader = new PdfPTable(1);
                    float[] widthCellsTableSalesInvoiceAccountHeader = new float[] { 100f };
                    tableSalesInvoiceAccountHeader.SetWidths(widthCellsTableSalesInvoiceAccountHeader);
                    tableSalesInvoiceAccountHeader.WidthPercentage = 100;
                    tableSalesInvoiceAccountHeader.AddCell(new PdfPCell(new Phrase(salesInvoiceAccount.AccountCode + " - " + salesInvoiceAccount.Account, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    document.Add(tableSalesInvoiceAccountHeader);

                    // SI Customer
                    var salesInvoicesArticleCustomers = from d in db.TrnSalesInvoices
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

                    if (salesInvoicesArticleCustomers.Any())
                    {
                        // table Data
                        PdfPTable tableData = new PdfPTable(7);
                        float[] widthsCellsData = new float[] { 50f, 15f, 15f, 15f, 15f, 15f, 15f };
                        tableData.SetWidths(widthsCellsData);
                        tableData.WidthPercentage = 100;
                        tableData.AddCell(new PdfPCell(new Phrase("Customer Name", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableData.AddCell(new PdfPCell(new Phrase("Balance", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableData.AddCell(new PdfPCell(new Phrase("Current", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableData.AddCell(new PdfPCell(new Phrase("30 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableData.AddCell(new PdfPCell(new Phrase("60 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableData.AddCell(new PdfPCell(new Phrase("90 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableData.AddCell(new PdfPCell(new Phrase("Over 120 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                        Decimal SubTotalBalance = 0;
                        Decimal SubTotalCurrent = 0;
                        Decimal SubTotal30Days = 0;
                        Decimal SubTotal60Days = 0;
                        Decimal SubTotal90Days = 0;
                        Decimal SubTotalOver120Days = 0;

                        foreach (var salesInvoicesArticleCustomer in salesInvoicesArticleCustomers)
                        {
                            // Sales Invoice with compute ages
                            var salesInvoiceWithComputeAges = from d in db.TrnSalesInvoices
                                                              where d.CustomerId == salesInvoicesArticleCustomer.CustomerId
                                                              && d.SIDate <= Convert.ToDateTime(DateAsOf)
                                                              && d.MstBranch.CompanyId == CompanyId
                                                              && d.BalanceAmount > 0
                                                              && d.IsLocked == true
                                                              select new Models.TrnSalesInvoice
                                                               {
                                                                   Id = d.Id,
                                                                   Customer = d.MstArticle.Article,
                                                                   BalanceAmount = d.BalanceAmount,
                                                                   DueDate = d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays)).ToShortDateString(),
                                                                   NumberOfDaysFromDueDate = Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days,
                                                                   CurrentAmount = computeAge(0, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                   Age30Amount = computeAge(1, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                   Age60Amount = computeAge(2, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                   Age90Amount = computeAge(3, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                   Age120Amount = computeAge(4, Convert.ToDateTime(DateAsOf).Subtract(d.SIDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount)
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

                                tableData.AddCell(new PdfPCell(new Phrase(salesInvoicesArticleCustomer.Customer, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f });
                                tableData.AddCell(new PdfPCell(new Phrase(totalBalanceAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                tableData.AddCell(new PdfPCell(new Phrase(totalCurrentAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                tableData.AddCell(new PdfPCell(new Phrase(totalAge30Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                tableData.AddCell(new PdfPCell(new Phrase(totalAge60Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                tableData.AddCell(new PdfPCell(new Phrase(totalAge90Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                tableData.AddCell(new PdfPCell(new Phrase(totalAge120AmountAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });

                                SubTotalBalance = SubTotalBalance + totalBalanceAmount;
                                SubTotalCurrent = SubTotalCurrent + totalCurrentAmount;
                                SubTotal30Days = SubTotal30Days + totalAge30Amount;
                                SubTotal60Days = SubTotal60Days + totalAge60Amount;
                                SubTotal90Days = SubTotal90Days + totalAge90Amount;
                                SubTotalOver120Days = SubTotalOver120Days + totalAge120AmountAmount;
                            }
                        }

                        document.Add(tableData);

                        // Sales invoice account total footer
                        PdfPTable tableSalesInvoiceAccountTotalFooter = new PdfPTable(7);
                        float[] widthsCellsSalesInvoiceAccountTotalFooter = new float[] { 50f, 15f, 15f, 15f, 15f, 15f, 15f };
                        tableSalesInvoiceAccountTotalFooter.SetWidths(widthsCellsSalesInvoiceAccountTotalFooter);
                        tableSalesInvoiceAccountTotalFooter.WidthPercentage = 100;
                        tableSalesInvoiceAccountTotalFooter.AddCell(new PdfPCell(new Phrase(salesInvoiceAccount.AccountCode + " - " + salesInvoiceAccount.Account + " Sub Total", fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                        tableSalesInvoiceAccountTotalFooter.AddCell(new PdfPCell(new Phrase(SubTotalBalance.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                        tableSalesInvoiceAccountTotalFooter.AddCell(new PdfPCell(new Phrase(SubTotalCurrent.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                        tableSalesInvoiceAccountTotalFooter.AddCell(new PdfPCell(new Phrase(SubTotal30Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                        tableSalesInvoiceAccountTotalFooter.AddCell(new PdfPCell(new Phrase(SubTotal60Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                        tableSalesInvoiceAccountTotalFooter.AddCell(new PdfPCell(new Phrase(SubTotal90Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                        tableSalesInvoiceAccountTotalFooter.AddCell(new PdfPCell(new Phrase(SubTotalOver120Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                        document.Add(tableSalesInvoiceAccountTotalFooter);
                        document.Add(Chunk.NEWLINE);

                        OverAllTotalBalance = OverAllTotalBalance + SubTotalBalance;
                        OverAllTotalCurrent = OverAllTotalCurrent + SubTotalCurrent;
                        OverAllTotal30Days = OverAllTotal30Days + SubTotal30Days;
                        OverAllTotal60Days = OverAllTotal60Days + SubTotal60Days;
                        OverAllTotal90Days = OverAllTotal90Days + SubTotal90Days;
                        OverAllTotalOver120Days = OverAllTotalOver120Days + SubTotalOver120Days;
                    }
                }

                document.Add(Chunk.NEWLINE);
                document.Add(line);

                // table Footer Total
                PdfPTable tableTotalFooter = new PdfPTable(7);
                float[] widthsCellsfooter3 = new float[] { 50f, 15f, 15f, 15f, 15f, 15f, 15f };
                tableTotalFooter.SetWidths(widthsCellsfooter3);
                tableTotalFooter.WidthPercentage = 100;
                tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(OverAllTotalBalance.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(OverAllTotalCurrent.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(OverAllTotal30Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(OverAllTotal60Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(OverAllTotal90Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(OverAllTotalOver120Days.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
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