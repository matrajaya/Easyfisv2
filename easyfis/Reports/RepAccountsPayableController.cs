using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class RepAccountsPayableController : Controller
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
        public ActionResult AccountsPayable(String DateAsOf, Int32 CompanyId)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Accounts Payable", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date as of " + DateAsOf, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // RR Accounts
            var receivingReceiptAccounts = from d in db.TrnReceivingReceipts
                                           where d.RRDate <= Convert.ToDateTime(DateAsOf)
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

            Decimal totalBalanceAmount = 0;
            Decimal totalCurrentAmount = 0;
            Decimal totalAge30Amount = 0;
            Decimal totalAge60Amount = 0;
            Decimal totalAge90Amount = 0;
            Decimal totalAge120AmountAmount = 0;

            if (receivingReceiptAccounts.Any())
            {
                foreach (var receivingReceiptAccount in receivingReceiptAccounts)
                {
                    // table RR account header
                    PdfPTable tableReceivingReceiptAccountHeader = new PdfPTable(1);
                    float[] widthCellsTableReceivingReceiptAccountHeader = new float[] { 100f };
                    tableReceivingReceiptAccountHeader.SetWidths(widthCellsTableReceivingReceiptAccountHeader);
                    tableReceivingReceiptAccountHeader.WidthPercentage = 100;
                    tableReceivingReceiptAccountHeader.AddCell(new PdfPCell(new Phrase(receivingReceiptAccount.AccountCode + " - " + receivingReceiptAccount.Account, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    document.Add(tableReceivingReceiptAccountHeader);

                    // RR Suppliers
                    var receivingReceiptArticleSuppliers = from d in db.TrnReceivingReceipts
                                                           where d.MstArticle.MstAccount.Id == receivingReceiptAccount.AccountId
                                                           && d.RRDate <= Convert.ToDateTime(DateAsOf)
                                                           && d.MstBranch.CompanyId == CompanyId
                                                           && d.BalanceAmount > 0
                                                           && d.IsLocked == true
                                                           group d by new
                                                           {
                                                               SupplierId = d.SupplierId,
                                                               Supplier = d.MstArticle.Article
                                                           } into g
                                                           select new Models.TrnReceivingReceipt
                                                           {
                                                               SupplierId = g.Key.SupplierId,
                                                               Supplier = g.Key.Supplier,
                                                               BalanceAmount = g.Sum(d => d.BalanceAmount)
                                                           };

                    Decimal accountSubTotalBalanceAmount = 0;
                    Decimal accountSubTotalCurrentAmount = 0;
                    Decimal accountSubTotalAge30Amount = 0;
                    Decimal accountSubTotalAge60Amount = 0;
                    Decimal accountSubTotalAge90Amount = 0;
                    Decimal accountSubTotalAge120AmountAmount = 0;

                    if (receivingReceiptArticleSuppliers.Any())
                    {
                        foreach (var receivingReceiptArticleSupplier in receivingReceiptArticleSuppliers)
                        {
                            // table RR supplier header
                            PdfPTable tableReceivingReceiptSupplierHeader = new PdfPTable(1);
                            float[] widthCellsTableReceivingReceiptSupplierHeader = new float[] { 100f };
                            tableReceivingReceiptSupplierHeader.SetWidths(widthCellsTableReceivingReceiptSupplierHeader);
                            tableReceivingReceiptSupplierHeader.WidthPercentage = 100;
                            tableReceivingReceiptSupplierHeader.AddCell(new PdfPCell(new Phrase(receivingReceiptArticleSupplier.Supplier, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f });
                            document.Add(tableReceivingReceiptSupplierHeader);

                            // table column header
                            PdfPTable tableData = new PdfPTable(10);
                            float[] widthCellsData = new float[] { 100f, 100f, 120f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                            tableData.SetWidths(widthCellsData);
                            tableData.WidthPercentage = 100;
                            tableData.AddCell(new PdfPCell(new Phrase("RR Number", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("RR Date", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Document Ref.", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Due Date", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Balance", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Current", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("30 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("60 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("90 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Over 120 Days", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });

                            // RR 
                            var receivingReceiptsWithComputeAges = from d in db.TrnReceivingReceipts
                                                                   where d.SupplierId == receivingReceiptArticleSupplier.SupplierId
                                                                   && d.RRDate <= Convert.ToDateTime(DateAsOf)
                                                                   && d.MstBranch.CompanyId == CompanyId
                                                                   && d.BalanceAmount > 0
                                                                   && d.IsLocked == true
                                                                   select new Models.TrnReceivingReceipt
                                                                   {
                                                                       Id = d.Id,
                                                                       RRNumber = d.RRNumber,
                                                                       RRDate = d.RRDate.ToShortDateString(),
                                                                       DocumentReference = d.DocumentReference,
                                                                       BalanceAmount = d.BalanceAmount,
                                                                       DueDate = d.RRDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays)).ToShortDateString(),
                                                                       NumberOfDaysFromDueDate = Convert.ToDateTime(DateAsOf).Subtract(d.RRDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days,
                                                                       CurrentAmount = computeAge(0, Convert.ToDateTime(DateAsOf).Subtract(d.RRDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                       Age30Amount = computeAge(1, Convert.ToDateTime(DateAsOf).Subtract(d.RRDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                       Age60Amount = computeAge(2, Convert.ToDateTime(DateAsOf).Subtract(d.RRDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                       Age90Amount = computeAge(3, Convert.ToDateTime(DateAsOf).Subtract(d.RRDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount),
                                                                       Age120Amount = computeAge(4, Convert.ToDateTime(DateAsOf).Subtract(d.RRDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays))).Days, d.BalanceAmount)
                                                                   };

                            Decimal subTotalBalanceAmount = 0;
                            Decimal subTotalCurrentAmount = 0;
                            Decimal subTotalAge30Amount = 0;
                            Decimal subTotalAge60Amount = 0;
                            Decimal subTotalAge90Amount = 0;
                            Decimal subTotalAge120AmountAmount = 0;

                            if (receivingReceiptsWithComputeAges.Any())
                            {
                                foreach (var receivingReceiptsWithComputeAge in receivingReceiptsWithComputeAges)
                                {
                                    tableData.AddCell(new PdfPCell(new Phrase(receivingReceiptsWithComputeAge.RRNumber, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(receivingReceiptsWithComputeAge.RRDate, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(receivingReceiptsWithComputeAge.DocumentReference, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(receivingReceiptsWithComputeAge.DueDate, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(receivingReceiptsWithComputeAge.BalanceAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(receivingReceiptsWithComputeAge.CurrentAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(receivingReceiptsWithComputeAge.Age30Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(receivingReceiptsWithComputeAge.Age60Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(receivingReceiptsWithComputeAge.Age90Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });
                                    tableData.AddCell(new PdfPCell(new Phrase(receivingReceiptsWithComputeAge.Age120Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 6f });

                                    subTotalBalanceAmount = subTotalBalanceAmount + receivingReceiptsWithComputeAge.BalanceAmount;
                                    subTotalCurrentAmount = subTotalCurrentAmount + receivingReceiptsWithComputeAge.CurrentAmount;
                                    subTotalAge30Amount = subTotalAge30Amount + receivingReceiptsWithComputeAge.Age30Amount;
                                    subTotalAge60Amount = subTotalAge60Amount + receivingReceiptsWithComputeAge.Age60Amount;
                                    subTotalAge90Amount = subTotalAge90Amount + receivingReceiptsWithComputeAge.Age90Amount;
                                    subTotalAge120AmountAmount = subTotalAge120AmountAmount + receivingReceiptsWithComputeAge.Age120Amount;
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

                            // Reserve for Book Balances
                            //tableFooter.AddCell(new PdfPCell(new Phrase("", columnFontItalic)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                            //tableFooter.AddCell(new PdfPCell(new Phrase("", columnFontItalic)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, });
                            //tableFooter.AddCell(new PdfPCell(new Phrase("", columnFontItalic)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                            //tableFooter.AddCell(new PdfPCell(new Phrase("Book Balance", columnFontItalic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            //tableFooter.AddCell(new PdfPCell(new Phrase("0.00", columnFontItalic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            //tableFooter.AddCell(new PdfPCell(new Phrase("Variance", columnFontItalic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            //tableFooter.AddCell(new PdfPCell(new Phrase("0.00", columnFontItalic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            //tableFooter.AddCell(new PdfPCell(new Phrase("0.00", columnFontItalic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            //tableFooter.AddCell(new PdfPCell(new Phrase("0.00", columnFontItalic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                            //tableFooter.AddCell(new PdfPCell(new Phrase("0.00", columnFontItalic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

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
                    PdfPTable tableTotalReceivingReceiptAccountFooter = new PdfPTable(10);
                    float[] widthsCellsReceivingReceiptAccountFooter = new float[] { 10f, 40f, 55f, 315f, 100f, 100f, 100f, 100f, 100f, 100f };
                    tableTotalReceivingReceiptAccountFooter.SetWidths(widthsCellsReceivingReceiptAccountFooter);
                    tableTotalReceivingReceiptAccountFooter.WidthPercentage = 100;
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase(receivingReceiptAccount.AccountCode + " - " + receivingReceiptAccount.Account + " Sub Total", fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalBalanceAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalCurrentAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalAge30Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalAge60Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalAge90Amount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase(accountSubTotalAge120AmountAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Italic)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Italic)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("", fontArial9Italic)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("Book Balance", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("0.00", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("Variance", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("0.00", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("0.00", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("0.00", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableTotalReceivingReceiptAccountFooter.AddCell(new PdfPCell(new Phrase("0.00", fontArial9Italic)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    totalBalanceAmount = totalBalanceAmount + accountSubTotalBalanceAmount;
                    totalCurrentAmount = totalCurrentAmount + accountSubTotalCurrentAmount;
                    totalAge30Amount = totalAge30Amount + accountSubTotalAge30Amount;
                    totalAge60Amount = totalAge60Amount + accountSubTotalAge60Amount;
                    totalAge90Amount = totalAge90Amount + accountSubTotalAge90Amount;
                    totalAge120AmountAmount = totalAge120AmountAmount + accountSubTotalAge120AmountAmount;

                    document.Add(tableTotalReceivingReceiptAccountFooter);
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