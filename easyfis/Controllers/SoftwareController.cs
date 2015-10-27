using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
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
        public ActionResult JournalVoucherPDF()
        { 
            MemoryStream workStream = new MemoryStream();
            Document document = new Document();
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();
            document.Add(new Paragraph("Journal Voucher Data"));
            document.Add(new Paragraph(DateTime.Now.ToString()));
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");  
        }

        [Authorize]
        public ActionResult JournalVoucherPDFDownload()
        {
            // Create PDF
            var document = new Document();
            MemoryStream workStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);

            document.Open();
            document.Add(new Paragraph("Journal Voucher Data"));
            document.Add(new Paragraph(DateTime.Now.ToString()));
            document.Close();

            byte[] documentData = workStream.GetBuffer(); // get the generated PDF as raw data

            // write the data to response stream and set appropriate headers:
            Response.AppendHeader("Content-Disposition", "attachment; filename=JournalVoucher.pdf");
            Response.ContentType = "application/pdf";
            Response.BinaryWrite(documentData);
            Response.End();

            return View();
        }

        [Authorize]
        public ActionResult JournalVoucherDetail()
        {
            return View();
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
    }
}