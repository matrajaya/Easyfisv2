using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepAccountLedgerController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // PDF Collection Summary Report
        [Authorize]
        public ActionResult AccountLedger(String StartDate, String EndDate, Int32 CompanyId, Int32 AccountId)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Account Ledger", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var journalAccounts = from d in db.MstAccounts
                                  where d.Id == AccountId
                                  select new Models.TrnJournal
                                  {
                                      AccountCode = d.AccountCode,
                                      Account = d.Account
                                  };

            if (journalAccounts.Any())
            {
                // table branch account
                PdfPTable tableAccountHeader = new PdfPTable(1);
                float[] widthCellsTableBAccountHeader = new float[] { 100f };
                tableAccountHeader.SetWidths(widthCellsTableBAccountHeader);
                tableAccountHeader.WidthPercentage = 100;

                foreach (var journalAccount in journalAccounts)
                {
                    tableAccountHeader.AddCell(new PdfPCell(new Phrase(journalAccount.AccountCode + " - " + journalAccount.Account, fontArial11Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                    document.Add(tableAccountHeader);
                }

                PdfPTable tableHeaderDetail = new PdfPTable(7);
                PdfPCell Cell = new PdfPCell();
                float[] widthscellsheader2 = new float[] { 60f, 125f, 130f, 150f, 100f, 100f, 100f };
                tableHeaderDetail.SetWidths(widthscellsheader2);
                tableHeaderDetail.WidthPercentage = 100;
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Date", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Document Reference", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Particulars", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                var journals = from d in db.TrnJournals
                               where d.JournalDate >= Convert.ToDateTime(StartDate)
                               && d.JournalDate <= Convert.ToDateTime(EndDate)
                               && d.MstBranch.CompanyId == CompanyId
                               && d.AccountId == AccountId
                               select new Models.TrnJournal
                               {
                                   JournalDate = d.JournalDate.ToShortDateString(),
                                   DocumentReference = d.DocumentReference,
                                   Article = d.MstArticle.Article,
                                   Particulars = d.Particulars,
                                   DebitAmount = d.DebitAmount,
                                   CreditAmount = d.CreditAmount
                               };

                Decimal totalDebitAmount = 0;
                Decimal totalCreditAmount = 0;
                Decimal totalBalance = 0;
                Decimal balance = 0;
                foreach (var journal in journals)
                {
                    balance = journal.DebitAmount - journal.CreditAmount;

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.JournalDate, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DocumentReference, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Particulars, fontArial10)) { HorizontalAlignment = 0, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                    totalDebitAmount = totalDebitAmount + journal.DebitAmount;
                    totalCreditAmount = totalCreditAmount + journal.CreditAmount;
                    totalBalance = totalBalance + balance;
                }

                document.Add(tableHeaderDetail);

                // table Balance Sheet footer in Asset CAtegory
                PdfPTable tableFooter = new PdfPTable(7);
                float[] widthCellstableFooter = new float[] { 60f, 125f, 130f, 150f, 100f, 100f, 100f };
                tableFooter.SetWidths(widthCellstableFooter);
                tableFooter.WidthPercentage = 100;

                tableFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableFooter.AddCell(new PdfPCell(new Phrase("Totals", fontArial10Bold)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableFooter.AddCell(new PdfPCell(new Phrase(totalDebitAmount.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(totalCreditAmount.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableFooter.AddCell(new PdfPCell(new Phrase(totalBalance.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                document.Add(tableFooter);
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