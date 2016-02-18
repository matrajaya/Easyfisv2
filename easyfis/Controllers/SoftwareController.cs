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
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

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
        public ActionResult SupplierPDF(Int32 SupplierId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
        public ActionResult PurchaseOrderPDF(Int32 POId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
        public ActionResult ReceivingReceiptPDF(Int32 RRId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult AccountsPayable()
        {
            return View();
        }

        [Authorize]
        public ActionResult AccountsPayablePDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult AccountsPayableVoucherPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult AccountsPayableInputVATSummaryReportPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult AccountsPayableWithholdingTaxDetailReportPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult AccountsPayableWithholdingTaxSummaryReportPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult AccountsPayableForm2037PDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult Disbursement()
        {
            return View();
        }

        [Authorize]
        public ActionResult DisbursementDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult DisbursementPDF(Int32 DisbursementId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
        public ActionResult CustomerPDF(Int32 CustomerId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
        public ActionResult SalesPDF(Int32 SalesId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult AccountsReceivable()
        {
            return View();
        }

        [Authorize]
        public ActionResult AccountsReceivablePDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult AccountsReceivableSummaryPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult AccountsReceivableStatementOfAccountPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
        public ActionResult CollectionPDF(Int32 CollectonId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
        public ActionResult ItemPDF(Int32 ItemId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
        public ActionResult StockInPDF(Int32 StockInId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
        public ActionResult StockOutPDF(Int32 StockOutId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult InventoryReport()
        {
            return View();
        }

        [Authorize]
        public ActionResult InventoryReportPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // article Inventories
            var articleInventories = from d in db.MstArticleInventories
                                     where d.TrnInventories.Any(i => i.InventoryDate >= Convert.ToDateTime(StartDate))
                                     && d.TrnInventories.Any(i => i.InventoryDate <= Convert.ToDateTime(EndDate))
                                     && d.MstBranch.CompanyId == CompanyId
                                     && d.MstArticle.IsInventory == true
                                     && d.Quantity >= 0
                                     select new Models.MstArticleInventory
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         ArticleId = d.ArticleId,
                                         Article = d.MstArticle.Article,
                                         InventoryCode = d.InventoryCode,
                                         Quantity = d.Quantity,
                                         Cost = d.Cost,
                                         Amount = d.Amount,
                                         Particulars = d.Particulars,
                                         ManualArticleCode = d.MstArticle.ManualArticleCode,
                                         UnitId = d.MstArticle.MstUnit.Id,
                                         Unit = d.MstArticle.MstUnit.Unit
                                     };

            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 9);

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Inventory", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

            document.Add(tableHeader);

            PdfPTable tableHeaderDetail = new PdfPTable(12);
            PdfPCell Cell = new PdfPCell();
            float[] widthscellsheader2 = new float[] { 20f, 30f, 15f, 16f, 16f, 16f, 16f, 16f, 16f, 16f, 16f, 16f };
            tableHeaderDetail.SetWidths(widthscellsheader2);
            tableHeaderDetail.WidthPercentage = 100;
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Item", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Unit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Cost", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
            PdfPCell header = (new PdfPCell(new Phrase("Quantity", columnFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            header.Colspan = 4;
            tableHeaderDetail.AddCell(header);
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Total Amount", columnFont)) { HorizontalAlignment = 1, PaddingTop = 3f, Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
            PdfPCell headerColspan = (new PdfPCell(new Phrase("Quantity", columnFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            headerColspan.Colspan = 2;
            tableHeaderDetail.AddCell(headerColspan);
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Variance Amount", columnFont)) { HorizontalAlignment = 1, PaddingTop = 3f, Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Beg", columnFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("In", columnFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Out", columnFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("End", columnFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Count", columnFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Variance", columnFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

            Decimal totalAmount = 0;
            Decimal count = 0;
            Decimal quantityVariance = 0;
            Decimal varianceAmount = 0;

            Decimal totalTotalAmount = 0;
            Decimal totalVarianceAmount = 0;

            foreach (var articleInventory in articleInventories)
            {
                totalTotalAmount = totalTotalAmount + (articleInventory.Cost * articleInventory.Quantity);
                quantityVariance = articleInventory.Quantity - count;
                totalVarianceAmount = totalVarianceAmount + (articleInventory.Cost * quantityVariance);
            }

            foreach (var articleInventory in articleInventories)
            {
                totalAmount = articleInventory.Cost * articleInventory.Quantity;
                count = 0;
                quantityVariance = articleInventory.Quantity - count;
                varianceAmount = articleInventory.Cost * quantityVariance;

                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(articleInventory.ManualArticleCode, cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(articleInventory.Article, cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(articleInventory.Unit, cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(articleInventory.Cost.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("0.00", cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("0.00", cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("0.00", cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(articleInventory.Quantity.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("0.00", cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(quantityVariance.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(varianceAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
            }

            document.Add(tableHeaderDetail);

            document.Add(Chunk.NEWLINE);

            PdfPTable tableFooter = new PdfPTable(12);
            float[] widthscellsfooter = new float[] { 20f, 30f, 15f, 16f, 16f, 16f, 16f, 16f, 16f, 16f, 16f, 16f };
            tableFooter.SetWidths(widthscellsfooter);
            tableFooter.WidthPercentage = 100;
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase("Total:", columnFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f });
            tableFooter.AddCell(new PdfPCell(new Phrase(totalTotalAmount.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 11f });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooter.AddCell(new PdfPCell(new Phrase(totalVarianceAmount.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 11f });
            document.Add(tableFooter);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult InventoryReportStockCardPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult InventoryReportStockInDetailPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult InventoryReportStockOutDetailPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult InventoryReportStockTransferDetailPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult InventoryReportItemListPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult InventoryReportItemComponentListPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult InventoryReportPhysicalCountSheetPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }


        [Authorize]
        public ActionResult InventoryReportFixedAssetsPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult StockTransfer()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockTransferDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockTransferPDF(Int32 StockTransferId)
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
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
        public ActionResult JournalVoucherPDF(Int32 JVId)
        {
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

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
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
        public ActionResult CompanyPDF(Int32 BranchId, Int32 CompanyId)
        {
            var branchName = (from d in db.MstBranches where d.Id == BranchId select d.Branch).SingleOrDefault();

            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var companyAddress = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var companyContactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();
            var companyTIN = (from d in db.MstCompanies where d.Id == CompanyId select d.TaxNumber).SingleOrDefault();

            // memorystrems
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 11);

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase("Company Information Sheet", headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase(branchName, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2 });
            document.Add(tableHeader);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // table main header
            PdfPTable tableCompanyDetail = new PdfPTable(2);
            float[] widthscellsCompanyDetail = new float[] { 20f, 100f };
            tableCompanyDetail.SetWidths(widthscellsCompanyDetail);
            tableCompanyDetail.WidthPercentage = 100;
            tableCompanyDetail.AddCell(new PdfPCell(new Phrase("Company: ", columnFont)) { Border = 0, PaddingTop = 15f });
            tableCompanyDetail.AddCell(new PdfPCell(new Phrase(companyName, cellFont)) { Border = 0, PaddingTop = 15f });
            tableCompanyDetail.AddCell(new PdfPCell(new Phrase("Address: ", columnFont)) { Border = 0, PaddingTop = 5f });
            tableCompanyDetail.AddCell(new PdfPCell(new Phrase(companyAddress, cellFont)) { Border = 0, PaddingTop = 5f });
            tableCompanyDetail.AddCell(new PdfPCell(new Phrase("Contact Number: ", columnFont)) { Border = 0, PaddingTop = 5f });
            tableCompanyDetail.AddCell(new PdfPCell(new Phrase(companyContactNo, cellFont)) { Border = 0, PaddingTop = 5f });
            tableCompanyDetail.AddCell(new PdfPCell(new Phrase("TIN: ", columnFont)) { Border = 0, PaddingTop = 5f });
            tableCompanyDetail.AddCell(new PdfPCell(new Phrase(companyTIN, cellFont)) { Border = 0, PaddingTop = 5f });
            document.Add(tableCompanyDetail);

            document.Add(Chunk.NEWLINE);

            // table Cells in branches
            PdfPTable tableBranches = new PdfPTable(5);
            float[] widthsOfTableBranches = new float[] { 30f, 60f, 75f, 50f, 50f };
            tableBranches.SetWidths(widthsOfTableBranches);
            tableBranches.WidthPercentage = 100;
            tableBranches.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableBranches.AddCell(new PdfPCell(new Phrase("Branch", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableBranches.AddCell(new PdfPCell(new Phrase("Address", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableBranches.AddCell(new PdfPCell(new Phrase("Contact No.", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableBranches.AddCell(new PdfPCell(new Phrase("TIN", columnFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

            var branches = from d in db.MstBranches
                           where d.CompanyId == CompanyId
                           select new Models.MstBranch
                           {
                               Id = d.Id,
                               CompanyId = d.CompanyId,
                               Company = d.MstCompany.Company,
                               BranchCode = d.BranchCode,
                               Branch = d.Branch,
                               Address = d.Address,
                               ContactNumber = d.ContactNumber,
                               TaxNumber = d.TaxNumber,
                               IsLocked = d.IsLocked,
                               CreatedById = d.CreatedById,
                               CreatedBy = d.MstUser.FullName,
                               CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                               UpdatedById = d.UpdatedById,
                               UpdatedBy = d.MstUser1.FullName,
                               UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                           };

            foreach (var b in branches)
            {
                tableBranches.AddCell(new PdfPCell(new Phrase(b.BranchCode, cellFont)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBranches.AddCell(new PdfPCell(new Phrase(b.Branch, cellFont)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBranches.AddCell(new PdfPCell(new Phrase(b.Address, cellFont)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBranches.AddCell(new PdfPCell(new Phrase(b.ContactNumber, cellFont)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableBranches.AddCell(new PdfPCell(new Phrase(b.TaxNumber, cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
            }

            document.Add(tableBranches);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatements()
        {
            return View();
        }

        [Authorize]
        public ActionResult FinancialStatementBalanceSheetPDF(String DateAsOf, Int32 CompanyId)
        {
            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Balance Sheet", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date as of " + DateAsOf, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // table Balance Sheet
            PdfPTable tableBalanceSheet = new PdfPTable(3);
            float[] widthCellsTableBalanceSheet = new float[] { 50f, 100f, 50f };
            tableBalanceSheet.SetWidths(widthCellsTableBalanceSheet);
            tableBalanceSheet.WidthPercentage = 100;
            PdfPCell headerSubCategoryColspan = (new PdfPCell(new Phrase("Account Type - Sub Categoy", cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
            headerSubCategoryColspan.Colspan = 3;
            tableBalanceSheet.AddCell(headerSubCategoryColspan);

            // =========
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("Account Type", cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("0.00", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("0000", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("Account", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("0.00", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

            // =========
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("Account Type", cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("0.00", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("0000", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("Account", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("0.00", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

            // =========
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("Account Type", cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("0.00", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("0000", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("Account", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
            tableBalanceSheet.AddCell(new PdfPCell(new Phrase("0.00", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });


            document.Add(tableBalanceSheet);

            document.Add(line);

            // table Balance Sheet
            PdfPTable tableBalanceSheetFooter = new PdfPTable(4);
            float[] widthCellsTableBalanceSheetFooter = new float[] { 20f, 20f, 150f, 30f };
            tableBalanceSheetFooter.SetWidths(widthCellsTableBalanceSheetFooter);
            tableBalanceSheetFooter.WidthPercentage = 100;

            // =========
            tableBalanceSheetFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableBalanceSheetFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            tableBalanceSheetFooter.AddCell(new PdfPCell(new Phrase("Total Current Assets", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableBalanceSheetFooter.AddCell(new PdfPCell(new Phrase("0.00", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

            // =========
            tableBalanceSheetFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableBalanceSheetFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            tableBalanceSheetFooter.AddCell(new PdfPCell(new Phrase("Total Asset", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableBalanceSheetFooter.AddCell(new PdfPCell(new Phrase("0.00", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

            document.Add(tableBalanceSheetFooter);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementIncomeStatementPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementCashFlowIndirectPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementTrialBalancePDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementAccountLedgerPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementReceivingReceiptBookPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementDisbursementBookPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementSalesBookPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementCollectionBookPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementStockInBookPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementStockOutBookPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementStockTransferBookPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementJournalVoucherBookPDF()
        {
            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult Users()
        {
            return View();
        }

        [Authorize]
        public ActionResult UsersDetail()
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