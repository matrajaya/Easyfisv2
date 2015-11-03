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
        public ActionResult JournalVoucherDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult JournalVoucherPDF()
        {
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;
      
            document.Open();

            Chunk glue = new Chunk(new iTextSharp.text.pdf.draw.VerticalPositionMark());
            Paragraph para = new Paragraph("Sonnets Catering Services Inc.");
            Paragraph para1 = new Paragraph("Tabok Road, Tingub, Mandaue City");
            Paragraph para2 = new Paragraph("239-5972");
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            Paragraph para3 = new Paragraph("Particulars:");
            Paragraph para4 = new Paragraph("NA");

            para.Add(new Chunk(glue));
            para1.Add(new Chunk(glue));
            para2.Add(new Chunk(glue));
            para3.Add(new Chunk(glue));
            para4.Add(new Chunk(glue));

            para.Add("Journal Voucher");
            para1.Add("Main Warehouse - Supply Chain");
            para2.Add(DateTime.Now.ToString());
            para3.Add("JV NO.:      00000000001");
            para4.Add("JV DATE:     10/08/2015");
                                
            document.Add(para);
            document.Add(para1);
            document.Add(para2);
            document.Add(p);
            document.Add(para3);
            document.Add(para4);
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

            document.Add(p);
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
            document.Add(p);

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