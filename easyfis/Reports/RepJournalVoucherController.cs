using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepJournalVoucherController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Item List
        [Authorize]
        public ActionResult JournalVoucher(Int32 JVId)
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
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);

            // line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // Company Detail
            var companyName = (from d in db.MstBranches where d.Id == currentBranchId() select d.MstCompany.Company).SingleOrDefault();
            var address = (from d in db.MstBranches where d.Id == currentBranchId() select d.MstCompany.Address).SingleOrDefault();
            var contactNo = (from d in db.MstBranches where d.Id == currentBranchId() select d.MstCompany.ContactNumber).SingleOrDefault();
            var branch = (from d in db.MstBranches where d.Id == currentBranchId() select d.Branch).SingleOrDefault();

            // table main header
            PdfPTable tableHeaderPage = new PdfPTable(2);
            float[] widthsCellsheaderPage = new float[] { 100f, 75f };
            tableHeaderPage.SetWidths(widthsCellsheaderPage);
            tableHeaderPage.WidthPercentage = 100;
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Journal Voucher", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(branch, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var journalVoucherId = (from d in db.TrnJournalVouchers where d.Id == JVId select d.Id).SingleOrDefault();

            // JV Date and other Details for Journal Voucher
            var JVNumber = (from d in db.TrnJournalVouchers where d.Id == JVId select d.JVNumber).SingleOrDefault();
            var JVDate = (from d in db.TrnJournalVouchers where d.Id == JVId select d.JVDate).SingleOrDefault();
            var JVParticulars = (from d in db.TrnJournalVouchers where d.Id == JVId select d.Particulars).SingleOrDefault();

            // Users Signature Data
            var preparedByUserId = (from d in db.TrnJournalVouchers where d.Id == journalVoucherId select d.PreparedById).SingleOrDefault();
            var checkedByUserId = (from d in db.TrnJournalVouchers where d.Id == journalVoucherId select d.CheckedById).SingleOrDefault();
            var approvedByUserId = (from d in db.TrnJournalVouchers where d.Id == journalVoucherId select d.ApprovedById).SingleOrDefault();

            var preparedBy = (from d in db.MstUsers where d.Id == preparedByUserId select d.FullName).SingleOrDefault();
            var checkedBy = (from d in db.MstUsers where d.Id == checkedByUserId select d.FullName).SingleOrDefault();
            var approvedBy = (from d in db.MstUsers where d.Id == approvedByUserId select d.FullName).SingleOrDefault();

            // table header detail under the separator
            PdfPTable tableHeaderDetail = new PdfPTable(3);
            float[] widthscellsheader2 = new float[] { 100f, 70f, 25f };
            tableHeaderDetail.SetWidths(widthscellsheader2);
            tableHeaderDetail.WidthPercentage = 100;
            tableHeaderDetail.WidthPercentage = 100;
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Particulars: ", fontArial11Bold)) { PaddingTop = 10f, Border = 0 });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("JV Number: ", fontArial11Bold)) { PaddingTop = 10f, Border = 0, HorizontalAlignment = 2 });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase(JVNumber, fontArial11)) { PaddingTop = 10f, Border = 0, HorizontalAlignment = 2 });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase(JVParticulars, fontArial11)) { Border = 0 });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("JV Date: ", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase(JVDate.ToString("MM/dd/yyyy"), fontArial11)) { Border = 0, HorizontalAlignment = 2 });
            document.Add(tableHeaderDetail);

            document.Add(Chunk.NEWLINE);

            // table Cells in header
            PdfPTable tableJournal = new PdfPTable(6);
            float[] widths = new float[] { 75f, 30f, 75f, 75f, 45f, 45f };
            tableJournal.SetWidths(widths);
            tableJournal.WidthPercentage = 100;
            tableJournal.AddCell(new PdfPCell(new Phrase("Branch", fontArial10Bold)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableJournal.AddCell(new PdfPCell(new Phrase("Code", fontArial10Bold)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableJournal.AddCell(new PdfPCell(new Phrase("Account", fontArial10Bold)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableJournal.AddCell(new PdfPCell(new Phrase("Article", fontArial10Bold)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableJournal.AddCell(new PdfPCell(new Phrase("Debit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableJournal.AddCell(new PdfPCell(new Phrase("Credit", fontArial10Bold)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

            var journals = from d in db.TrnJournals
                           where d.JVId == JVId
                           select new Models.TrnJournal
                           {
                               Id = d.Id,
                               JournalDate = d.JournalDate.ToShortDateString(),
                               BranchId = d.BranchId,
                               Branch = d.MstBranch.Branch,
                               BranchCode = d.MstBranch.BranchCode,
                               AccountId = d.AccountId,
                               Account = d.MstAccount.Account,
                               AccountCode = d.MstAccount.AccountCode,
                               ArticleId = d.ArticleId,
                               Article = d.MstArticle.Article,
                               Particulars = d.Particulars,
                               DebitAmount = d.DebitAmount,
                               CreditAmount = d.CreditAmount,
                               ORId = d.ORId,
                               CVId = d.CVId,
                               JVId = d.JVId,
                               RRId = d.RRId,
                               SIId = d.SIId,
                               INId = d.INId,
                               OTId = d.OTId,
                               STId = d.STId,
                               DocumentReference = d.DocumentReference,
                               APRRId = d.APRRId,
                               ARSIId = d.ARSIId,
                           };

            decimal debitTotal;
            decimal creditTotal;

            if (!journals.Any())
            {
                debitTotal = 0;
                creditTotal = 0;
            }
            else
            {
                debitTotal = journals.Sum(d => d.DebitAmount);
                creditTotal = journals.Sum(d => d.CreditAmount);
            }

            foreach (var j in journals)
            {
                var debit = j.DebitAmount.ToString("#,##0.00");
                var credit = j.CreditAmount.ToString("#,##0.00");

                tableJournal.AddCell(new PdfPCell(new Phrase(j.Branch, fontArial9)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableJournal.AddCell(new PdfPCell(new Phrase(j.AccountCode, fontArial9)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableJournal.AddCell(new PdfPCell(new Phrase(j.Account, fontArial9)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableJournal.AddCell(new PdfPCell(new Phrase(j.Article, fontArial9)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableJournal.AddCell(new PdfPCell(new Phrase(debit, fontArial9)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableJournal.AddCell(new PdfPCell(new Phrase(credit, fontArial9)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
            }

            document.Add(tableJournal);

            var debitTotalCurrency = debitTotal.ToString("#,##0.00");
            var creditTotalCurrency = creditTotal.ToString("#,##0.00");

            PdfPTable tableTotalDebitCredit = new PdfPTable(6);
            float[] widthsCells = new float[] { 75f, 30f, 75f, 75f, 45f, 45f };
            tableTotalDebitCredit.SetWidths(widthsCells);
            tableTotalDebitCredit.WidthPercentage = 100;
            tableTotalDebitCredit.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f });
            tableTotalDebitCredit.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f });
            tableTotalDebitCredit.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f });
            tableTotalDebitCredit.AddCell(new PdfPCell(new Phrase("Total", fontArial10Bold)) { Border = 0, PaddingTop = 15f, HorizontalAlignment = 2 });
            tableTotalDebitCredit.AddCell(new PdfPCell(new Phrase(Convert.ToString(debitTotalCurrency), fontArial9)) { Border = 0, PaddingTop = 15f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
            tableTotalDebitCredit.AddCell(new PdfPCell(new Phrase(Convert.ToString(creditTotalCurrency), fontArial9)) { Border = 0, PaddingTop = 15f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
            document.Add(tableTotalDebitCredit);

            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);

            // Table for Footer
            PdfPTable tableFooter = new PdfPTable(5);
            tableFooter.WidthPercentage = 100;
            float[] widthsCells2 = new float[] { 100f, 20f, 100f, 20f, 100f };
            tableFooter.SetWidths(widthsCells2);

            tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", fontArial11Bold)) { Border = 0, HorizontalAlignment = 0 });

            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

            tableFooter.AddCell(new PdfPCell(new Phrase(preparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
            tableFooter.AddCell(new PdfPCell(new Phrase(checkedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
            tableFooter.AddCell(new PdfPCell(new Phrase(approvedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });

            document.Add(tableFooter);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}