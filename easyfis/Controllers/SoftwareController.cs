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
        public ActionResult ReceivingReceipt()
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
        public ActionResult StockOut()
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

            document.Open();

            BaseFont bfTimes = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font times = new Font(bfTimes, 20, Font.BOLD);

            BaseFont boldOnly = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font boldFont = new Font(boldOnly, 14, Font.BOLD);

            //Chunk glue = new Chunk(new iTextSharp.text.pdf.draw.VerticalPositionMark());
            //Paragraph companyAndJV = new Paragraph(companyName, times);
            //Paragraph addressAndBranch = new Paragraph(companyAddress);
            //Paragraph contactNoAndDateNow = new Paragraph(companyContactNo);
            //Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            //Paragraph particularsLabel = new Paragraph("Particulars:", boldFont);
            //Paragraph particularsValue = new Paragraph(JVParticulars, boldFont);

            //companyAndJV.Add(new Chunk(glue));
            //addressAndBranch.Add(new Chunk(glue));
            //contactNoAndDateNow.Add(new Chunk(glue));
            //particularsLabel.Add(new Chunk(glue));
            //particularsValue.Add(new Chunk(glue));

            //companyAndJV.Add("Journal Voucher");
            //addressAndBranch.Add(branchName);
            //contactNoAndDateNow.Add("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"));
            //particularsLabel.Add("JV Number:   " + JVNumber);
            //particularsValue.Add("JV Date:     " + JVDate.ToString("MM/dd/yyyy"));

            //document.Add(companyAndJV);
            //document.Add(addressAndBranch);
            //document.Add(contactNoAndDateNow);
            //document.Add(line);
            //document.Add(particularsLabel);
            //document.Add(particularsValue);
            //document.Add(Chunk.NEWLINE);

            BaseFont boldOnlyCell = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false);
            Font boldFontCell = new Font(boldOnly, 13, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(3);
            float[] widthscellsheader = new float[] { 100f, 10f, 100f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, times)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Journal Voucher", times)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(companyAddress)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(branchName)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(companyContactNo)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"))) { Border = 0, HorizontalAlignment = 2 });
            document.Add(tableHeader);

            document.Add(line);

            // table header 2 under the separator
            PdfPTable tableHeader2 = new PdfPTable(3);
            float[] widthscellsheader2 = new float[] { 100f, 70f, 25f };
            tableHeader2.SetWidths(widthscellsheader2);
            tableHeader2.WidthPercentage = 100;
            tableHeader2.WidthPercentage = 100;
            tableHeader2.AddCell(new PdfPCell(new Phrase("Particulars: ", boldFont)) { PaddingTop = 10f, Border = 0 });
            tableHeader2.AddCell(new PdfPCell(new Phrase("JV Number: ", boldFont)) { PaddingTop = 10f, Border = 0, HorizontalAlignment = 2 });
            tableHeader2.AddCell(new PdfPCell(new Phrase(JVNumber)) { PaddingTop = 10f, Border = 0, HorizontalAlignment = 2 });
            tableHeader2.AddCell(new PdfPCell(new Phrase(JVParticulars)) { Border = 0 });
            tableHeader2.AddCell(new PdfPCell(new Phrase("JV Date: ", boldFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader2.AddCell(new PdfPCell(new Phrase(JVDate.ToString("MM/dd/yyyy"))) { Border = 0, HorizontalAlignment = 2 });
            document.Add(tableHeader2);

            document.Add(Chunk.NEWLINE);

            PdfPTable table = new PdfPTable(6);
            float[] widths = new float[] { 100f, 40f, 60f, 50f, 45f, 45f };
            table.SetWidths(widths);
            table.WidthPercentage = 100;

            // table Cells in header
            table.AddCell(new PdfPCell(new Phrase("Branch", boldFontCell)) { HorizontalAlignment = 1, PaddingBottom = 10f, BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Code", boldFontCell)) { HorizontalAlignment = 1, PaddingBottom = 10f, BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Account", boldFontCell)) { HorizontalAlignment = 1, PaddingBottom = 10f, BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Article", boldFontCell)) { HorizontalAlignment = 1, PaddingBottom = 10f, BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Debit", boldFontCell)) { HorizontalAlignment = 1, PaddingBottom = 10f, BackgroundColor = BaseColor.LIGHT_GRAY });
            table.AddCell(new PdfPCell(new Phrase("Credit", boldFontCell)) { HorizontalAlignment = 1, PaddingBottom = 10f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                table.AddCell(new PdfPCell(new Phrase(j.Branch)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                table.AddCell(new PdfPCell(new Phrase(j.AccountCode)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                table.AddCell(new PdfPCell(new Phrase(j.Account)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                table.AddCell(new PdfPCell(new Phrase(j.Article)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                table.AddCell(new PdfPCell(new Phrase(debit)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                table.AddCell(new PdfPCell(new Phrase(credit)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
            }

            document.Add(table);

            var debitTotalCurrency = debitTotal.ToString("#,##0.00");
            var creditTotalCurrency = creditTotal.ToString("#,##0.00");

            PdfPTable table2 = new PdfPTable(6);
            float[] widthsCells = new float[] { 100f, 40f, 60f, 50f, 45f, 45f };
            table2.SetWidths(widthsCells);
            table2.WidthPercentage = 100;

            table2.WidthPercentage = 100;
            table2.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f });
            table2.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f });
            table2.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f });
            table2.AddCell(new PdfPCell(new Phrase("Total:", boldFontCell)) { Border = 0, PaddingTop = 15f, HorizontalAlignment = 2 });
            table2.AddCell(new PdfPCell(new Phrase(Convert.ToString(debitTotalCurrency))) { Border = 0, PaddingTop = 15f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
            table2.AddCell(new PdfPCell(new Phrase(Convert.ToString(creditTotalCurrency))) { Border = 0, PaddingTop = 15f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, HorizontalAlignment = 2 });
            document.Add(table2);

            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);

            // Table for Footer
            PdfPTable table3 = new PdfPTable(5);
            table3.WidthPercentage = 100;
            float[] widthsCells2 = new float[] { 100f, 20f, 100f, 20f, 100f };
            table3.SetWidths(widthsCells2);

            table3.AddCell(new PdfPCell(new Phrase(preparedBy)) { Border = 0, PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 5f });
            table3.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
            table3.AddCell(new PdfPCell(new Phrase(checkedBy)) { Border = 0, PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 5f });
            table3.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
            table3.AddCell(new PdfPCell(new Phrase(approvedBy)) { Border = 0, PaddingTop = 10f, HorizontalAlignment = 1, PaddingBottom = 5f });

            table3.AddCell(new PdfPCell(new Phrase("Prepared by:", boldFontCell)) { Border = 1, HorizontalAlignment = 1 });
            table3.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            table3.AddCell(new PdfPCell(new Phrase("Checked by:", boldFontCell)) { Border = 1, HorizontalAlignment = 1 });
            table3.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            table3.AddCell(new PdfPCell(new Phrase("Approved by:", boldFontCell)) { Border = 1, HorizontalAlignment = 1 });
            document.Add(table3);

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        //[Authorize]
        //public ActionResult JournalVoucherPDFDownload()
        //{
        //    // Create PDF
        //    var document = new Document();
        //    MemoryStream workStream = new MemoryStream();
        //    PdfWriter writer = PdfWriter.GetInstance(document, workStream);

        //    document.Open();
        //    document.Add(new Paragraph("Journal Voucher Data"));
        //    document.Add(new Paragraph(DateTime.Now.ToString()));
        //    document.Close();

        //    byte[] documentData = workStream.GetBuffer(); // get the generated PDF as raw data

        //    // write the data to response stream and set appropriate headers:
        //    Response.AppendHeader("Content-Disposition", "attachment; filename=JournalVoucher.pdf");
        //    Response.ContentType = "application/pdf";
        //    Response.BinaryWrite(documentData);
        //    Response.End();

        //    return View();
        //}

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