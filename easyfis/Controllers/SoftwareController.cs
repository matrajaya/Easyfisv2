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
        public ActionResult Sales()
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

            var preparedBy = (from d in db.MstUsers where d.Id == preparedByUserId select d.UserName).SingleOrDefault();
            var preparedByFirstName = (from d in db.MstUsers where d.Id == preparedByUserId select d.FirstName).SingleOrDefault();
            var preparedByLastName = (from d in db.MstUsers where d.Id == preparedByUserId select d.LastName).SingleOrDefault();
            var preparedByFullName = preparedByFirstName + " " + preparedByLastName;

            var checkedBy = (from d in db.MstUsers where d.Id == checkedByUserId select d.UserName).SingleOrDefault();
            var checkedByFirstName = (from d in db.MstUsers where d.Id == checkedByUserId select d.FirstName).SingleOrDefault();
            var checkedByLastName = (from d in db.MstUsers where d.Id == checkedByUserId select d.LastName).SingleOrDefault();
            var checkedByFullName = checkedByFirstName + " " + checkedByLastName;

            var approvedBy = (from d in db.MstUsers where d.Id == approvedByUserId select d.UserName).SingleOrDefault();
            var approvedByFirstName = (from d in db.MstUsers where d.Id == approvedByUserId select d.FirstName).SingleOrDefault();
            var approvedByLastName = (from d in db.MstUsers where d.Id == approvedByUserId select d.LastName).SingleOrDefault();
            var approvedByFullName = approvedByFirstName + " " + approvedByLastName;

            // Pauls Work and Layouts in PDF journal
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
      
            document.Open();

            Chunk glue = new Chunk(new iTextSharp.text.pdf.draw.VerticalPositionMark());
            Paragraph company = new Paragraph(companyName);
            Paragraph address = new Paragraph(companyAddress);
            Paragraph contactNo = new Paragraph(companyContactNo);
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            Paragraph particularsLabel = new Paragraph("Particulars:");
            Paragraph particularsValue = new Paragraph(JVParticulars);

            company.Add(new Chunk(glue));
            address.Add(new Chunk(glue));
            contactNo.Add(new Chunk(glue));
            particularsLabel.Add(new Chunk(glue));
            particularsValue.Add(new Chunk(glue));

            company.Add("Journal Voucher");
            address.Add(branchName);
            contactNo.Add(DateTime.Now.ToString());
            particularsLabel.Add("JV Number: " + JVNumber);
            particularsValue.Add("JV Date:   " + JVDate.ToString("MM/dd/yyyy"));

            document.Add(company);
            document.Add(address);
            document.Add(contactNo);
            document.Add(line);
            document.Add(particularsLabel);
            document.Add(particularsValue);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);

            PdfPTable table = new PdfPTable(6);
            table.WidthPercentage = 100;
            PdfPCell cell = new PdfPCell();
            cell.Colspan = 6;
            table.AddCell("Branch");
            table.AddCell("Code");
            table.AddCell("Account");
            table.AddCell("Article");
            table.AddCell("Debit");
            table.AddCell("Credit");

            table.AddCell(new PdfPCell(new Phrase("Main Warehouse")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("1010")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("Cash in Bank")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("BDO")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("100,000.00")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("100,000.00")) { Border = 0 });

            table.AddCell(new PdfPCell(new Phrase("Main Warehouse")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("3000")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("Common Stock")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("Investor")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("100,000.00")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("100,000.00")) { Border = 0 });
            document.Add(table);

            document.Add(line);
            table = new PdfPTable(6);
            table.WidthPercentage = 100;
            cell = new PdfPCell();
            cell.Colspan = 6;

            table.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("Total")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("100,000.00")) { Border = 0 });
            table.AddCell(new PdfPCell(new Phrase("100,000.00")) { Border = 0 });
            document.Add(table);
            document.Add(line);

            Paragraph preparedByUser = new Paragraph("Prepared by: " + preparedByFullName);
            Paragraph checkedByUser = new Paragraph("Checked by: " + checkedByFullName);
            Paragraph approvedByUser = new Paragraph("Approved by: " + approvedByFullName);

            document.Add(preparedByUser);
            document.Add(checkedByUser);
            document.Add(approvedByUser);

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