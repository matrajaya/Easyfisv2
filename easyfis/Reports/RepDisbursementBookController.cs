using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepDisbursementBookController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // PDF Collection Summary Report
        [Authorize]
        public ActionResult DisbursementBook(String StartDate, String EndDate, Int32 CompanyId)
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
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);

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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Disbursement Book", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var journalsDocumentReferences = from d in db.TrnJournals
                                             where d.JournalDate >= Convert.ToDateTime(StartDate)
                                             && d.JournalDate <= Convert.ToDateTime(EndDate)
                                             && d.MstBranch.CompanyId == CompanyId
                                             && d.CVId != null
                                             group d by new
                                             {
                                                 DocumentReference = d.DocumentReference
                                             } into g
                                             select new Models.TrnJournal
                                             {
                                                 DocumentReference = g.Key.DocumentReference
                                             };

            if (journalsDocumentReferences.Any())
            {
                Decimal TotalDebitAmount = 0;
                Decimal TotalCreditAmount = 0;
                Decimal TotalBalance = 0;
                foreach (var journalsDocumentReference in journalsDocumentReferences)
                {

                    // table branch account
                    PdfPTable tableDocRefHeader = new PdfPTable(1);
                    float[] widthCellsTableDocRefHeader = new float[] { 100f };
                    tableDocRefHeader.SetWidths(widthCellsTableDocRefHeader);
                    tableDocRefHeader.WidthPercentage = 100;

                    tableDocRefHeader.AddCell(new PdfPCell(new Phrase(journalsDocumentReference.DocumentReference, fontArial11Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                    document.Add(tableDocRefHeader);

                    PdfPTable tableHeaderDetail = new PdfPTable(6);
                    PdfPCell Cell = new PdfPCell();
                    float[] widthscellsheader2 = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                    tableHeaderDetail.SetWidths(widthscellsheader2);
                    tableHeaderDetail.WidthPercentage = 100;
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Code", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    var journals = from d in db.TrnJournals
                                   where d.DocumentReference == journalsDocumentReference.DocumentReference
                                   select new Models.TrnJournal
                                   {
                                       AccountCode = d.MstAccount.AccountCode,
                                       Account = d.MstAccount.Account,
                                       Article = d.MstArticle.Article,
                                       Particulars = d.Particulars,
                                       DebitAmount = d.DebitAmount,
                                       CreditAmount = d.CreditAmount
                                   };

                    Decimal subTotalDebitAmount = 0;
                    Decimal subTotalCreditAmount = 0;
                    Decimal subTotalBalance = 0;
                    Decimal balance = 0;

                    foreach (var journal in journals)
                    {
                        balance = journal.DebitAmount - journal.CreditAmount;

                        tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                        tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                        subTotalDebitAmount = subTotalDebitAmount + journal.DebitAmount;
                        subTotalCreditAmount = subTotalCreditAmount + journal.CreditAmount;
                        subTotalBalance = subTotalBalance + balance;
                    }

                    document.Add(tableHeaderDetail);

                    // table Balance Sheet footer in Asset CAtegory
                    PdfPTable tableSubTotalFooter = new PdfPTable(6);
                    float[] widthCellstableSubTotalFooter = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                    tableSubTotalFooter.SetWidths(widthCellstableSubTotalFooter);
                    tableSubTotalFooter.WidthPercentage = 100;

                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", fontArial10Bold)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalDebitAmount.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalCreditAmount.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalBalance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

                    document.Add(tableSubTotalFooter);
                    document.Add(Chunk.NEWLINE);

                    TotalDebitAmount = TotalDebitAmount + subTotalDebitAmount;
                    TotalCreditAmount = TotalCreditAmount + subTotalCreditAmount;
                    TotalBalance = TotalBalance + subTotalBalance;
                }

                document.Add(line);
                // table Balance Sheet footer in Asset CAtegory
                PdfPTable tableTotalFooter = new PdfPTable(6);
                float[] widthCellstableTotalFooter = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                tableTotalFooter.SetWidths(widthCellstableTotalFooter);
                tableTotalFooter.WidthPercentage = 100;

                tableTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", fontArial10Bold)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalDebitAmount.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalCreditAmount.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalBalance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

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