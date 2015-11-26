using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class SoftwareController : UserAccountController
    {
        // =============
        // GET: SOFTWARE
        // =============
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Supplier()
        {
            return View();
        }

        [Authorize]
        public ActionResult SupplierDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult PurchaseOrder()
        {
            return View();
        }

        [Authorize]
        public ActionResult PurchaseOrderDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReceivingReceipt()
        {
            return View();
        }

        [Authorize]
        public ActionResult ReceivingReceiptDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult AccountsPayable()
        {
            return View();
        }

        [Authorize]
        public ActionResult Disbursement()
        {
            return View();
        }

        [Authorize]
        public ActionResult Bank()
        {
            return View();
        }

        [Authorize]
        public ActionResult Customer()
        {
            return View();
        }

        [Authorize]
        public ActionResult CustomerDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult Sales()
        {
            return View();
        }

        [Authorize]
        public ActionResult SalesDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult AccountsReceivable()
        {
            return View();
        }

        [Authorize]
        public ActionResult Collection()
        {
            return View();
        }

        [Authorize]
        public ActionResult CollectionDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult BankReconciliation()
        {
            return View();
        }

        [Authorize]
        public ActionResult Items()
        {
            return View();
        }

        [Authorize]
        public ActionResult ItemDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockIn()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockInDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockOut()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockOutDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult InventoryReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockTransfer()
        {
            return View();
        }

        [Authorize]
        public ActionResult SystemTables()
        {
            return View();
        }

        [Authorize]
        public ActionResult ChartOfAccounts()
        {
            return View();
        }

        [Authorize]
        public ActionResult JournalVoucher()
        {
            return View();
        }

        [Authorize]
        public ActionResult JournalVoucherDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult JournalVoucherPDF(int JVId)
        {
            Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();
            var journalVoucherId = (from d in db.TrnJournalVouchers where d.Id == JVId select d.Id).SingleOrDefault();

            // ==========================
            // GLOBAL VARIABLES WITH LINQ
            // ==========================

            // Company and Branch Id Filter by Journal Vouchers
            var branchId = (from d in db.TrnJournalVouchers where d.Id == journalVoucherId select d.BranchId).SingleOrDefault();
            var companyId = (from d in db.MstBranches where d.Id == branchId select d.CompanyId).SingleOrDefault();

            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == companyId select d.Company).SingleOrDefault();
            var companyAddress = (from d in db.MstCompanies where d.Id == companyId select d.Address).SingleOrDefault();
            var companyContactNo = (from d in db.MstCompanies where d.Id == companyId select d.ContactNumber).SingleOrDefault();

            // Branch Filter by Comapny Id
            var branchName = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();
            var branchAddress = (from d in db.MstBranches where d.Id == branchId select d.Address).SingleOrDefault();
            var branchContactNo = (from d in db.MstBranches where d.Id == branchId select d.ContactNumber).SingleOrDefault();

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

            // Pauls Work and Layouts in PDF journal
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 15, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 11);

            // table main header
            PdfPTable tableHeader = new PdfPTable(3);
            float[] widthscellsheader = new float[] { 100f, 10f, 100f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Journal Voucher", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(companyAddress, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(branchName, headerDetailFont)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(companyContactNo, headerDetailFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2 });
            document.Add(tableHeader);

            // Horizaontal Line 
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // table header detail under the separator
            PdfPTable tableHeaderDetail = new PdfPTable(3);
            float[] widthscellsheader2 = new float[] { 100f, 70f, 25f };
            tableHeaderDetail.SetWidths(widthscellsheader2);
            tableHeaderDetail.WidthPercentage = 100;
            tableHeaderDetail.WidthPercentage = 100;
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Particulars: ", columnFont)) { PaddingTop = 10f, Border = 0 });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("JV Number: ", columnFont)) { PaddingTop = 10f, Border = 0, HorizontalAlignment = 2 });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase(JVNumber, headerDetailFont)) { PaddingTop = 10f, Border = 0, HorizontalAlignment = 2 });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase(JVParticulars, headerDetailFont)) { Border = 0 });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("JV Date: ", columnFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase(JVDate.ToString("MM/dd/yyyy"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2 });
            document.Add(tableHeaderDetail);

            document.Add(Chunk.NEWLINE);

            // table Cells in header
            PdfPTable tableJournal = new PdfPTable(6);
            float[] widths = new float[] { 75f, 30f, 75f, 75f, 45f, 45f };
            tableJournal.SetWidths(widths);
            tableJournal.WidthPercentage = 100;
            tableJournal.AddCell(new PdfPCell(new Phrase("Branch", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableJournal.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableJournal.AddCell(new PdfPCell(new Phrase("Account", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableJournal.AddCell(new PdfPCell(new Phrase("Article", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableJournal.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableJournal.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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
            
            decimal debitTotal = journals.Sum(d => d.DebitAmount);
            decimal creditTotal = journals.Sum(d => d.CreditAmount);

            foreach (var j in journals)
            {
                var debit = j.DebitAmount.ToString("#,##0.00");
                var credit = j.CreditAmount.ToString("#,##0.00");

                tableJournal.AddCell(new PdfPCell(new Phrase(j.Branch, cellFont)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableJournal.AddCell(new PdfPCell(new Phrase(j.AccountCode, cellFont)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableJournal.AddCell(new PdfPCell(new Phrase(j.Account, cellFont)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableJournal.AddCell(new PdfPCell(new Phrase(j.Article, cellFont)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableJournal.AddCell(new PdfPCell(new Phrase(debit, cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableJournal.AddCell(new PdfPCell(new Phrase(credit, cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
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
            tableTotalDebitCredit.AddCell(new PdfPCell(new Phrase("Total:", columnFont)) { Border = 0, PaddingTop = 15f, HorizontalAlignment = 2 });
            tableTotalDebitCredit.AddCell(new PdfPCell(new Phrase(Convert.ToString(debitTotalCurrency))) { Border = 0, PaddingTop = 15f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
            tableTotalDebitCredit.AddCell(new PdfPCell(new Phrase(Convert.ToString(creditTotalCurrency))) { Border = 0, PaddingTop = 15f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
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
            tableFooter.AddCell(new PdfPCell(new Phrase(preparedBy)) { Border = 0, PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 5f });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
            tableFooter.AddCell(new PdfPCell(new Phrase(checkedBy)) { Border = 0, PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 5f });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
            tableFooter.AddCell(new PdfPCell(new Phrase(approvedBy)) { Border = 0, PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 5f });
            tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", columnFont)) { Border = 1, HorizontalAlignment = 1 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", columnFont)) { Border = 1, HorizontalAlignment = 1 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", columnFont)) { Border = 1, HorizontalAlignment = 1 });
            document.Add(tableFooter);
            
            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult Company()
        {
            return View();
        }

        [Authorize]
        public ActionResult CompanyDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult FinancialStatements()
        {
            return View();
        }

        [Authorize]
        public ActionResult Users()
        {
            return View();
        }

        [Authorize]
        public ActionResult Settings()
        {
            return View();
        }

        public BaseColor ColorGrey { get; set; }
    }
}