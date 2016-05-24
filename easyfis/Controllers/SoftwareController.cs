using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace easyfis.Controllers
{
    public class SoftwareController : UserAccountController
    {
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

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
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 11);

            // table main header
            PdfPTable tableHeader = new PdfPTable(1);
            float[] widthscellsheader = new float[] { 100f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase("Supplier Information Sheet", headerFont)) { Border = 0, HorizontalAlignment = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("General Admin", headerDetailFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10 });
            document.Add(tableHeader);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            PdfPTable tableHeader2 = new PdfPTable(1);
            float[] widthscellsheader2 = new float[] { 100f };
            tableHeader2.SetWidths(widthscellsheader2);
            tableHeader2.WidthPercentage = 100;
            tableHeader2.AddCell(new PdfPCell(new Phrase("", headerFont)) { Border = 0, HorizontalAlignment = 0 });
            tableHeader2.AddCell(new PdfPCell(new Phrase("", headerDetailFont)) { Border = 0, HorizontalAlignment = 0 });
            document.Add(tableHeader2);

            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);

            Paragraph line2 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line2);

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
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 9);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Purchase Order", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(branch, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    document.Add(line);

                    var purchaseOrderHeaders = from d in db.TrnPurchaseOrders
                                               where d.Id == POId
                                               select new Models.TrnPurchaseOrder
                                               {
                                                   Id = d.Id,
                                                   BranchId = d.BranchId,
                                                   Branch = d.MstBranch.Branch,
                                                   PONumber = d.PONumber,
                                                   PODate = d.PODate.ToShortDateString(),
                                                   SupplierId = d.SupplierId,
                                                   Supplier = d.MstArticle.Article,
                                                   TermId = d.TermId,
                                                   Term = d.MstTerm.Term,
                                                   ManualRequestNumber = d.ManualRequestNumber,
                                                   ManualPONumber = d.ManualPONumber,
                                                   DateNeeded = d.DateNeeded.ToShortDateString(),
                                                   Remarks = d.Remarks,
                                                   IsClose = d.IsClose,
                                                   RequestedById = d.RequestedById,
                                                   RequestedBy = d.MstUser4.FullName,
                                                   PreparedById = d.PreparedById,
                                                   PreparedBy = d.MstUser3.FullName,
                                                   CheckedById = d.CheckedById,
                                                   CheckedBy = d.MstUser1.FullName,
                                                   ApprovedById = d.ApprovedById,
                                                   ApprovedBy = d.MstUser.FullName
                                               };

                    String Supplier = "", Term = "", DateNeeded = "", Remarks = "";
                    String PONumber = "", PODate = "", RequestNo = "";
                    String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                    foreach (var purchaseOrderHeader in purchaseOrderHeaders)
                    {
                        Supplier = purchaseOrderHeader.Supplier;
                        Term = purchaseOrderHeader.Term;
                        DateNeeded = purchaseOrderHeader.DateNeeded;
                        Remarks = purchaseOrderHeader.Remarks;
                        PONumber = purchaseOrderHeader.PONumber;
                        PODate = purchaseOrderHeader.PODate;
                        RequestNo = purchaseOrderHeader.ManualRequestNumber;
                        PreparedBy = purchaseOrderHeader.PreparedBy;
                        CheckedBy = purchaseOrderHeader.CheckedBy;
                        ApprovedBy = purchaseOrderHeader.ApprovedBy;
                    }

                    PdfPTable tableSubHeader = new PdfPTable(4);
                    float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                    tableSubHeader.SetWidths(widthscellsSubheader);
                    tableSubHeader.WidthPercentage = 100;
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Supplier: ", columnFont)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Supplier, cellFont2)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("PO Number: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(PONumber, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Term: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Term, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("PO date: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(PODate, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Date Needed: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(DateNeeded, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Request No: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(RequestNo, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Remarks: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Remarks, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    document.Add(tableSubHeader);
                    document.Add(Chunk.NEWLINE);

                    var purchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                             where d.POId == POId
                                             select new Models.TrnPurchaseOrderItem
                                             {
                                                 Id = d.Id,
                                                 POId = d.POId,
                                                 PO = d.TrnPurchaseOrder.PONumber,
                                                 ItemId = d.ItemId,
                                                 Item = d.MstArticle.Article,
                                                 ItemCode = d.MstArticle.ManualArticleCode,
                                                 Particulars = d.Particulars,
                                                 UnitId = d.UnitId,
                                                 Unit = d.MstUnit.Unit,
                                                 Quantity = d.Quantity,
                                                 Cost = d.Cost,
                                                 Amount = d.Amount
                                             };

                    PdfPTable tablePOLines = new PdfPTable(6);
                    float[] widthscellsPOLines = new float[] { 50f, 60f, 130f, 150f, 100f, 100f };
                    tablePOLines.SetWidths(widthscellsPOLines);
                    tablePOLines.WidthPercentage = 100;

                    tablePOLines.AddCell(new PdfPCell(new Phrase("Quantity", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tablePOLines.AddCell(new PdfPCell(new Phrase("Unit", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tablePOLines.AddCell(new PdfPCell(new Phrase("Item", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tablePOLines.AddCell(new PdfPCell(new Phrase("Particulars", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tablePOLines.AddCell(new PdfPCell(new Phrase("Price", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tablePOLines.AddCell(new PdfPCell(new Phrase("Amount", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    Decimal TotalAmount = 0;
                    foreach (var purchaseOrderItem in purchaseOrderItems)
                    {
                        tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Quantity.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Unit, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Item, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Particulars, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Price.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tablePOLines.AddCell(new PdfPCell(new Phrase(purchaseOrderItem.Amount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        TotalAmount = TotalAmount + purchaseOrderItem.Amount;
                    }

                    document.Add(tablePOLines);

                    document.Add(Chunk.NEWLINE);

                    PdfPTable tablePOLineTotalAmount = new PdfPTable(6);
                    float[] widthscellsPOLinesTotalAmount = new float[] { 50f, 60f, 130f, 150f, 100f, 100f };
                    tablePOLineTotalAmount.SetWidths(widthscellsPOLinesTotalAmount);
                    tablePOLineTotalAmount.WidthPercentage = 100;

                    tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    document.Add(tablePOLineTotalAmount);

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
                    tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });

                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                    tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    document.Add(tableFooter);

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
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
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 9);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Receiving Receipt", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(branch, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    document.Add(line);

                    var receivingReceiptHeaders = from d in db.TrnReceivingReceipts
                                                  where d.Id == RRId
                                                  select new Models.TrnReceivingReceipt
                                                  {
                                                      Id = d.Id,
                                                      BranchId = d.BranchId,
                                                      Branch = d.MstBranch.Branch,
                                                      RRDate = d.RRDate.ToShortDateString(),
                                                      RRNumber = d.RRNumber,
                                                      SupplierId = d.SupplierId,
                                                      Supplier = d.MstArticle.Article,
                                                      TermId = d.TermId,
                                                      Term = d.MstTerm.Term,
                                                      DueDate = d.RRDate.AddDays(Convert.ToInt32(d.MstTerm.NumberOfDays)).ToShortDateString(),
                                                      DocumentReference = d.DocumentReference,
                                                      ManualRRNumber = d.ManualRRNumber,
                                                      Remarks = d.Remarks,
                                                      Amount = d.Amount,
                                                      WTaxAmount = d.WTaxAmount,
                                                      PaidAmount = d.PaidAmount,
                                                      AdjustmentAmount = d.AdjustmentAmount,
                                                      BalanceAmount = d.BalanceAmount,
                                                      ReceivedById = d.ReceivedById,
                                                      ReceivedBy = d.MstUser4.FullName,
                                                      PreparedById = d.PreparedById,
                                                      PreparedBy = d.MstUser3.FullName,
                                                      CheckedById = d.CheckedById,
                                                      CheckedBy = d.MstUser1.FullName,
                                                      ApprovedById = d.ApprovedById,
                                                      ApprovedBy = d.MstUser.FullName,
                                                      IsLocked = d.IsLocked,
                                                      CreatedById = d.CreatedById,
                                                      CreatedBy = d.MstUser2.FullName,
                                                      CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                                      UpdatedById = d.UpdatedById,
                                                      UpdatedBy = d.MstUser5.FullName,
                                                      UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                                  };

                    String Supplier = "", Term = "", DueDate = "", Remarks = "";
                    String RRNumber = "", RRDate = "", DocumentRef = "";
                    String PreparedBy = "", CheckedBy = "", ApprovedBy = "", ReceivedBy = "";

                    foreach (var receivingReceiptHeader in receivingReceiptHeaders)
                    {
                        Supplier = receivingReceiptHeader.Supplier;
                        Term = receivingReceiptHeader.Term;
                        DueDate = receivingReceiptHeader.DueDate;
                        Remarks = receivingReceiptHeader.Remarks;
                        RRNumber = receivingReceiptHeader.RRNumber;
                        RRDate = receivingReceiptHeader.RRDate;
                        DocumentRef = receivingReceiptHeader.DocumentReference;
                        PreparedBy = receivingReceiptHeader.PreparedBy;
                        CheckedBy = receivingReceiptHeader.CheckedBy;
                        ApprovedBy = receivingReceiptHeader.ApprovedBy;
                        ReceivedBy = receivingReceiptHeader.ReceivedBy;
                    }

                    PdfPTable tableSubHeader = new PdfPTable(4);
                    float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                    tableSubHeader.SetWidths(widthscellsSubheader);
                    tableSubHeader.WidthPercentage = 100;
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Supplier: ", columnFont)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Supplier, cellFont2)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("RR Number: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(RRNumber, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Term: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Term, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("RR date: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(RRDate, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Due Date: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(DueDate, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Document Ref: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(DocumentRef, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Remarks: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Remarks, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    document.Add(tableSubHeader);
                    document.Add(Chunk.NEWLINE);

                    var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                                where d.RRId == RRId
                                                select new Models.TrnReceivingReceiptItem
                                                {
                                                    Id = d.Id,
                                                    RRId = d.RRId,
                                                    RR = d.TrnReceivingReceipt.RRNumber,
                                                    POId = d.POId,
                                                    PO = d.TrnPurchaseOrder.PONumber,
                                                    ItemId = d.ItemId,
                                                    Item = d.MstArticle.Article,
                                                    ItemCode = d.MstArticle.ManualArticleCode,
                                                    Particulars = d.Particulars,
                                                    UnitId = d.UnitId,
                                                    Unit = d.MstUnit.Unit,
                                                    Quantity = d.Quantity,
                                                    Cost = d.Cost,
                                                    Amount = d.Amount,
                                                    VATId = d.VATId,
                                                    VAT = d.MstTaxType.TaxType,
                                                    VATPercentage = d.VATPercentage,
                                                    VATAmount = d.VATAmount,
                                                    WTAXId = d.WTAXId,
                                                    WTAX = d.MstTaxType1.TaxType,
                                                    WTAXPercentage = d.WTAXPercentage,
                                                    WTAXAmount = d.WTAXAmount,
                                                    BranchId = d.BranchId,
                                                    Branch = d.MstBranch.Branch,
                                                    BaseUnitId = d.BaseUnitId,
                                                    BaseUnit = d.MstUnit1.Unit,
                                                    BaseQuantity = d.BaseQuantity,
                                                    BaseCost = d.BaseCost
                                                };

                    PdfPTable tableRRLines = new PdfPTable(6);
                    float[] widthscellsRRLines = new float[] { 50f, 60f, 150f, 130f, 100f, 100f };
                    tableRRLines.SetWidths(widthscellsRRLines);
                    tableRRLines.WidthPercentage = 100;

                    tableRRLines.AddCell(new PdfPCell(new Phrase("Quantity", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableRRLines.AddCell(new PdfPCell(new Phrase("Unit", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableRRLines.AddCell(new PdfPCell(new Phrase("Item", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableRRLines.AddCell(new PdfPCell(new Phrase("Branch", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableRRLines.AddCell(new PdfPCell(new Phrase("Price", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableRRLines.AddCell(new PdfPCell(new Phrase("Amount", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    Decimal TotalAmount = 0;
                    foreach (var receivingReceiptItem in receivingReceiptItems)
                    {
                        tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Quantity.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Unit, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Item, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Branch, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Price.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableRRLines.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        TotalAmount = TotalAmount + receivingReceiptItem.Amount;
                    }

                    document.Add(tableRRLines);

                    document.Add(Chunk.NEWLINE);

                    PdfPTable tableRRLineTotalAmount = new PdfPTable(6);
                    float[] widthscellsRRLinesTotalAmount = new float[] { 50f, 60f, 150f, 130f, 100f, 100f };
                    tableRRLineTotalAmount.SetWidths(widthscellsRRLinesTotalAmount);
                    tableRRLineTotalAmount.WidthPercentage = 100;

                    tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    document.Add(tableRRLineTotalAmount);

                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);

                    // Table for Footer
                    PdfPTable tableFooter = new PdfPTable(7);
                    tableFooter.WidthPercentage = 100;
                    float[] widthsCells2 = new float[] { 100f, 20f, 100f, 20f, 100f, 20f, 100f };
                    tableFooter.SetWidths(widthsCells2);

                    tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Received by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });

                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                    tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(ReceivedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });

                    document.Add(tableFooter);

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
        }

        [Authorize]
        public ActionResult AccountsPayable()
        {
            return View();
        }

        public Decimal ComputeAge(Int32 Age, Int32 Elapsed, Decimal Amount)
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
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 9);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Disbursement", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(branch, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    document.Add(line);

                    var disbursementHeaders = from d in db.TrnDisbursements
                                              where d.Id == DisbursementId
                                              select new Models.TrnDisbursement
                                              {
                                                  Id = d.Id,
                                                  BranchId = d.BranchId,
                                                  Branch = d.MstBranch.Branch,
                                                  CVNumber = d.CVNumber,
                                                  CVDate = d.CVDate.ToShortDateString(),
                                                  SupplierId = d.SupplierId,
                                                  Supplier = d.MstArticle1.Article,
                                                  Payee = d.Payee,
                                                  PayTypeId = d.PayTypeId,
                                                  PayType = d.MstPayType.PayType,
                                                  BankId = d.BankId,
                                                  Bank = d.MstArticle.Article,
                                                  ManualCVNumber = d.ManualCVNumber,
                                                  Particulars = d.Particulars,
                                                  CheckNumber = d.CheckNumber,
                                                  CheckDate = d.CheckDate.ToShortDateString(),
                                                  Amount = d.Amount,
                                                  IsCrossCheck = d.IsCrossCheck,
                                                  IsClear = d.IsClear,
                                                  PreparedById = d.PreparedById,
                                                  PreparedBy = d.MstUser3.FullName,
                                                  CheckedById = d.CheckedById,
                                                  CheckedBy = d.MstUser1.FullName,
                                                  ApprovedById = d.ApprovedById,
                                                  ApprovedBy = d.MstUser.FullName,
                                                  IsLocked = d.IsLocked,
                                                  CreatedById = d.CreatedById,
                                                  CreatedBy = d.MstUser2.FullName,
                                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                                  UpdatedById = d.UpdatedById,
                                                  UpdatedBy = d.MstUser4.FullName,
                                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                              };

                    String Payee = "", Bank = "", Particulars = "", CheckNo = "";
                    String CVNumber = "", CVDate = "", CheckDate = "";
                    String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                    foreach (var disbursementHeader in disbursementHeaders)
                    {
                        Payee = disbursementHeader.Payee;
                        Bank = disbursementHeader.Bank;
                        CheckNo = disbursementHeader.CheckNumber;
                        CheckDate = disbursementHeader.CheckDate;
                        Particulars = disbursementHeader.Particulars;
                        CVNumber = disbursementHeader.CVNumber;
                        CVDate = disbursementHeader.CVDate;
                        PreparedBy = disbursementHeader.PreparedBy;
                        CheckedBy = disbursementHeader.CheckedBy;
                        ApprovedBy = disbursementHeader.ApprovedBy;
                    }

                    PdfPTable tableSubHeader = new PdfPTable(4);
                    float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                    tableSubHeader.SetWidths(widthscellsSubheader);
                    tableSubHeader.WidthPercentage = 100;
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Payee: ", columnFont)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Payee, cellFont2)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("CV Number: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(CVNumber, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Bank: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Bank, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("CV date: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(CVDate, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Check No: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(CheckNo, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Check Date: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(CheckDate, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });


                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Particulars: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Particulars, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    document.Add(tableSubHeader);
                    document.Add(Chunk.NEWLINE);

                    var disbursementLines = from d in db.TrnDisbursementLines
                                            where d.CVId == DisbursementId
                                            select new Models.TrnDisbursementLine
                                            {
                                                Id = d.Id,
                                                CVId = d.CVId,
                                                CV = d.TrnDisbursement.CVNumber,
                                                BranchId = d.BranchId,
                                                Branch = d.MstBranch.Branch,
                                                AccountId = d.AccountId,
                                                Account = d.MstAccount.Account,
                                                ArticleId = d.ArticleId,
                                                Article = d.MstArticle.Article,
                                                RRId = d.RRId,
                                                RR = d.TrnReceivingReceipt.RRNumber,
                                                Particulars = d.Particulars,
                                                Amount = d.Amount
                                            };

                    PdfPTable tableCVLines = new PdfPTable(6);
                    float[] widthscellsCVLines = new float[] { 120f, 120f, 130f, 150f, 100f, 100f };
                    tableCVLines.SetWidths(widthscellsCVLines);
                    tableCVLines.WidthPercentage = 100;

                    tableCVLines.AddCell(new PdfPCell(new Phrase("Branch", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableCVLines.AddCell(new PdfPCell(new Phrase("Account", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableCVLines.AddCell(new PdfPCell(new Phrase("Article", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableCVLines.AddCell(new PdfPCell(new Phrase("Particulars", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableCVLines.AddCell(new PdfPCell(new Phrase("RR Number", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableCVLines.AddCell(new PdfPCell(new Phrase("Amount", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    Decimal TotalAmount = 0;

                    foreach (var disbursementLine in disbursementLines)
                    {
                        tableCVLines.AddCell(new PdfPCell(new Phrase(disbursementLine.Branch, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableCVLines.AddCell(new PdfPCell(new Phrase(disbursementLine.Account, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableCVLines.AddCell(new PdfPCell(new Phrase(disbursementLine.Article, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableCVLines.AddCell(new PdfPCell(new Phrase(disbursementLine.Particulars, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableCVLines.AddCell(new PdfPCell(new Phrase(disbursementLine.RR, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableCVLines.AddCell(new PdfPCell(new Phrase(disbursementLine.Amount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        TotalAmount = TotalAmount + disbursementLine.Amount;
                    }

                    document.Add(tableCVLines);

                    document.Add(Chunk.NEWLINE);

                    PdfPTable tableCVLineTotalAmount = new PdfPTable(6);
                    float[] widthscellsCVLinesTotalAmount = new float[] { 120f, 120f, 130f, 150f, 100f, 100f };
                    tableCVLineTotalAmount.SetWidths(widthscellsCVLinesTotalAmount);
                    tableCVLineTotalAmount.WidthPercentage = 100;

                    tableCVLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableCVLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    document.Add(tableCVLineTotalAmount);

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
                    tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });

                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                    tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    document.Add(tableFooter);

                    document.Add(Chunk.NEWLINE);

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
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
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 11);

            // table main header
            PdfPTable tableHeader = new PdfPTable(1);
            float[] widthscellsheader = new float[] { 100f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase("Customer Information Sheet", headerFont)) { Border = 0, HorizontalAlignment = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("General Admin", headerDetailFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10 });
            document.Add(tableHeader);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            PdfPTable tableHeader2 = new PdfPTable(1);
            float[] widthscellsheader2 = new float[] { 100f };
            tableHeader2.SetWidths(widthscellsheader2);
            tableHeader2.WidthPercentage = 100;
            tableHeader2.AddCell(new PdfPCell(new Phrase("", headerFont)) { Border = 0, HorizontalAlignment = 0 });
            tableHeader2.AddCell(new PdfPCell(new Phrase("", headerDetailFont)) { Border = 0, HorizontalAlignment = 0 });
            document.Add(tableHeader2);

            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);
            document.Add(Chunk.NEWLINE);

            Paragraph line2 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line2);

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
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 9);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Sales", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(branch, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    document.Add(line);

                    var salesInvoiceHeaders = from d in db.TrnSalesInvoices
                                              where d.Id == SalesId
                                              select new Models.TrnSalesInvoice
                                              {
                                                  Id = d.Id,
                                                  BranchId = d.BranchId,
                                                  Branch = d.MstBranch.Branch,
                                                  SINumber = d.SINumber,
                                                  SIDate = d.SIDate.ToShortDateString(),
                                                  CustomerId = d.CustomerId,
                                                  Customer = d.MstArticle.Article,
                                                  TermId = d.TermId,
                                                  Term = d.MstTerm.Term,
                                                  DocumentReference = d.DocumentReference,
                                                  ManualSINumber = d.ManualSINumber,
                                                  Remarks = d.Remarks,
                                                  Amount = d.Amount,
                                                  PaidAmount = d.PaidAmount,
                                                  AdjustmentAmount = d.AdjustmentAmount,
                                                  BalanceAmount = d.BalanceAmount,
                                                  SoldById = d.SoldById,
                                                  SoldBy = d.MstUser4.FullName,
                                                  PreparedById = d.PreparedById,
                                                  PreparedBy = d.MstUser3.FullName,
                                                  CheckedById = d.CheckedById,
                                                  CheckedBy = d.MstUser1.FullName,
                                                  ApprovedById = d.ApprovedById,
                                                  ApprovedBy = d.MstUser.FullName,
                                                  IsLocked = d.IsLocked,
                                                  CreatedById = d.CreatedById,
                                                  CreatedBy = d.MstUser2.FullName,
                                                  CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                                  UpdatedById = d.UpdatedById,
                                                  UpdatedBy = d.MstUser5.FullName,
                                                  UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                              };

                    String Customer = "", Term = "", Remarks = "", Sales = "";
                    String SINumber = "", SIDate = "", DocumentReference = "";
                    String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                    foreach (var salesInvoiceHeader in salesInvoiceHeaders)
                    {
                        Customer = salesInvoiceHeader.Customer;
                        Term = salesInvoiceHeader.Term;
                        Remarks = salesInvoiceHeader.Remarks;
                        SINumber = salesInvoiceHeader.SINumber;
                        SIDate = salesInvoiceHeader.SIDate;
                        PreparedBy = salesInvoiceHeader.PreparedBy;
                        CheckedBy = salesInvoiceHeader.CheckedBy;
                        ApprovedBy = salesInvoiceHeader.ApprovedBy;
                        Sales = salesInvoiceHeader.SoldBy;
                        DocumentReference = salesInvoiceHeader.DocumentReference;
                    }

                    PdfPTable tableSubHeader = new PdfPTable(4);
                    float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                    tableSubHeader.SetWidths(widthscellsSubheader);
                    tableSubHeader.WidthPercentage = 100;
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Customer: ", columnFont)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Customer, cellFont2)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("SI Number: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(SINumber, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Term: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Term, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("SI date: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(SIDate, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Sales: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Sales, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Document Ref: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(DocumentReference, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Remarks: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Remarks, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    document.Add(tableSubHeader);
                    document.Add(Chunk.NEWLINE);

                    var salesInvoiceItems = from d in db.TrnSalesInvoiceItems
                                            where d.SIId == SalesId
                                            select new Models.TrnSalesInvoiceItem
                                            {
                                                Id = d.Id,
                                                SIId = d.SIId,
                                                SI = d.TrnSalesInvoice.SINumber,
                                                ItemId = d.ItemId,
                                                ItemCode = d.MstArticle.ManualArticleCode,
                                                Item = d.MstArticle.Article,
                                                ItemInventoryId = d.ItemInventoryId,
                                                ItemInventory = d.MstArticleInventory.InventoryCode,
                                                Particulars = d.Particulars,
                                                UnitId = d.UnitId,
                                                Unit = d.MstUnit.Unit,
                                                Quantity = d.Quantity,
                                                Price = d.Price,
                                                DiscountId = d.DiscountId,
                                                Discount = d.MstDiscount.Discount,
                                                DiscountRate = d.DiscountRate,
                                                DiscountAmount = d.DiscountAmount,
                                                NetPrice = d.NetPrice,
                                                Amount = d.Amount,
                                                VATId = d.VATId,
                                                VAT = d.MstTaxType.TaxType,
                                                VATPercentage = d.VATPercentage,
                                                VATAmount = d.VATAmount,
                                                BaseUnitId = d.BaseUnitId,
                                                BaseUnit = d.MstUnit1.Unit,
                                                BaseQuantity = d.BaseQuantity,
                                                BasePrice = d.BasePrice
                                            };

                    PdfPTable tableSILines = new PdfPTable(7);
                    float[] widthscellsSILines = new float[] { 60f, 70f, 130f, 140f, 100f, 100f, 100f };
                    tableSILines.SetWidths(widthscellsSILines);
                    tableSILines.WidthPercentage = 100;

                    tableSILines.AddCell(new PdfPCell(new Phrase("Quantity", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSILines.AddCell(new PdfPCell(new Phrase("Unit", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSILines.AddCell(new PdfPCell(new Phrase("Item", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSILines.AddCell(new PdfPCell(new Phrase("Particulars", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSILines.AddCell(new PdfPCell(new Phrase("Price", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSILines.AddCell(new PdfPCell(new Phrase("Discount", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSILines.AddCell(new PdfPCell(new Phrase("Amount", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    Decimal TotalAmount = 0;
                    foreach (var salesInvoiceItem in salesInvoiceItems)
                    {
                        //AmountMinusVAT = salesInvoiceItem.Amount - salesInvoiceItem.VATAmount;

                        tableSILines.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Quantity.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSILines.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Unit, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSILines.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Item, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSILines.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Particulars, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSILines.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Price.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSILines.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.DiscountAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSILines.AddCell(new PdfPCell(new Phrase(salesInvoiceItem.Amount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        TotalAmount = TotalAmount + salesInvoiceItem.Amount;
                    }

                    document.Add(tableSILines);

                    document.Add(Chunk.NEWLINE);

                    PdfPTable tableSILineTotalAmount = new PdfPTable(7);
                    float[] widthscellsSILinesTotalAmount = new float[] { 60f, 70f, 130f, 140f, 100f, 100f, 100f };
                    tableSILineTotalAmount.SetWidths(widthscellsSILinesTotalAmount);
                    tableSILineTotalAmount.WidthPercentage = 100;

                    tableSILineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSILineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSILineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSILineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSILineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSILineTotalAmount.AddCell(new PdfPCell(new Phrase("Total: ", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSILineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    document.Add(tableSILineTotalAmount);

                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);

                    // Table for Footer
                    PdfPTable tableFooter = new PdfPTable(7);
                    tableFooter.WidthPercentage = 100;
                    float[] widthsCells2 = new float[] { 100f, 20f, 100f, 20f, 100f, 20f, 100f };
                    tableFooter.SetWidths(widthsCells2);

                    tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Received by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });

                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                    tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Date Received: ", columnFont)) { Border = 1, HorizontalAlignment = 0, PaddingBottom = 5f });

                    document.Add(tableFooter);

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
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
        public ActionResult CollectionPDF(Int32 CollectonId)
        {
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 9);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Collection", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(branch, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    document.Add(line);

                    var collectionHeaders = from d in db.TrnCollections
                                            where d.Id == CollectonId
                                            select new Models.TrnCollection
                                            {
                                                Id = d.Id,
                                                BranchId = d.BranchId,
                                                Branch = d.MstBranch.Branch,
                                                ORNumber = d.ORNumber,
                                                ORDate = d.ORDate.ToShortDateString(),
                                                CustomerId = d.CustomerId,
                                                Customer = d.MstArticle.Article,
                                                Particulars = d.Particulars,
                                                ManualORNumber = d.ManualORNumber,
                                                PreparedById = d.PreparedById,
                                                PreparedBy = d.MstUser3.FullName,
                                                CheckedById = d.CheckedById,
                                                CheckedBy = d.MstUser.FullName,
                                                ApprovedById = d.ApprovedById,
                                                ApprovedBy = d.MstUser1.FullName,
                                                IsLocked = d.IsLocked,
                                                CreatedById = d.CreatedById,
                                                CreatedBy = d.MstUser2.FullName,
                                                CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                                UpdatedById = d.UpdatedById,
                                                UpdatedBy = d.MstUser4.FullName,
                                                UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                            };

                    String Customer = "", ManualORNumber = "", Particulars = "";
                    String ORNumber = "", ORDate = "";
                    String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                    foreach (var collectionHeader in collectionHeaders)
                    {
                        Customer = collectionHeader.Customer;
                        ManualORNumber = collectionHeader.ManualORNumber;
                        Particulars = collectionHeader.Particulars;
                        ORNumber = collectionHeader.ORNumber;
                        ORDate = collectionHeader.ORDate;
                        PreparedBy = collectionHeader.PreparedBy;
                        CheckedBy = collectionHeader.CheckedBy;
                        ApprovedBy = collectionHeader.ApprovedBy;
                    }

                    PdfPTable tableSubHeader = new PdfPTable(4);
                    float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                    tableSubHeader.SetWidths(widthscellsSubheader);
                    tableSubHeader.WidthPercentage = 100;
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Customer: ", columnFont)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Customer, cellFont2)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("OR Number: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(ORNumber, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Manual OR No.: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(ManualORNumber, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("OR Date: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(ORDate, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Particulars: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Particulars, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    document.Add(tableSubHeader);
                    document.Add(Chunk.NEWLINE);

                    var collectionLines = from d in db.TrnCollectionLines
                                          where d.ORId == CollectonId
                                          select new Models.TrnCollectionLine
                                          {
                                              Id = d.Id,
                                              ORId = d.ORId,
                                              OR = d.TrnCollection.ORNumber,
                                              ORDate = d.TrnCollection.ORDate.ToShortDateString(),
                                              Customer = d.TrnCollection.MstArticle.Article,
                                              BranchId = d.BranchId,
                                              Branch = d.MstBranch.Branch,
                                              AccountId = d.AccountId,
                                              Account = d.MstAccount.Account,
                                              ArticleId = d.ArticleId,
                                              Article = d.MstArticle.Article,
                                              SIId = d.SIId,
                                              SI = d.TrnSalesInvoice.SINumber,
                                              Particulars = d.Particulars,
                                              Amount = d.Amount,
                                              PayTypeId = d.PayTypeId,
                                              PayType = d.MstPayType.PayType,
                                              CheckNumber = d.CheckNumber,
                                              CheckDate = d.CheckDate.ToShortDateString(),
                                              CheckBank = d.CheckBank,
                                              DepositoryBankId = d.DepositoryBankId,
                                              DepositoryBank = d.MstArticle1.Article,
                                              IsClear = d.IsClear
                                          };

                    PdfPTable tableORLines = new PdfPTable(6);
                    float[] widthscellsORLines = new float[] { 70f, 120f, 100f, 80f, 140f, 100f };
                    tableORLines.SetWidths(widthscellsORLines);
                    tableORLines.WidthPercentage = 100;

                    tableORLines.AddCell(new PdfPCell(new Phrase("SI Number", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableORLines.AddCell(new PdfPCell(new Phrase("Pay Type", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableORLines.AddCell(new PdfPCell(new Phrase("Check No.", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableORLines.AddCell(new PdfPCell(new Phrase("Check Date", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableORLines.AddCell(new PdfPCell(new Phrase("Chgck Bank/Branch", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableORLines.AddCell(new PdfPCell(new Phrase("Amount", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    Decimal TotalAmount = 0;

                    foreach (var collectionLine in collectionLines)
                    {
                        tableORLines.AddCell(new PdfPCell(new Phrase(collectionLine.SI, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableORLines.AddCell(new PdfPCell(new Phrase(collectionLine.PayType, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableORLines.AddCell(new PdfPCell(new Phrase(collectionLine.CheckNumber, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableORLines.AddCell(new PdfPCell(new Phrase(collectionLine.CheckDate, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableORLines.AddCell(new PdfPCell(new Phrase(collectionLine.CheckBank, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableORLines.AddCell(new PdfPCell(new Phrase(collectionLine.Amount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        TotalAmount = TotalAmount + collectionLine.Amount;
                    }

                    document.Add(tableORLines);

                    document.Add(Chunk.NEWLINE);

                    PdfPTable tableORLineTotalAmount = new PdfPTable(6);
                    float[] widthscellsORLinesTotalAmount = new float[] { 70f, 120f, 100f, 80f, 140f, 100f };
                    tableORLineTotalAmount.SetWidths(widthscellsORLinesTotalAmount);
                    tableORLineTotalAmount.WidthPercentage = 100;

                    tableORLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableORLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableORLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableORLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableORLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total: ", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableORLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    document.Add(tableORLineTotalAmount);

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

                    tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });

                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                    tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });

                    document.Add(tableFooter);

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
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

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 11);

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 50f, 50f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase("Item Information Sheet", headerFont)) { Border = 0, HorizontalAlignment = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("General Admin", headerDetailFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed Tuesday, February 23, 2016, 1:25:25 PM", headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10 });
            document.Add(tableHeader);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line);

            document.Add(Chunk.NEWLINE);

            PdfPTable tableDetail = new PdfPTable(4);
            float[] widthscellheaderDetail = new float[] { 15f, 45f, 20f, 20f };
            tableDetail.SetWidths(widthscellheaderDetail);
            tableDetail.WidthPercentage = 100;
            tableDetail.AddCell(new PdfPCell(new Phrase("Item Code:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("0000000002", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Base Unit:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Gallon(s)", cellFont)) { Border = 0, HorizontalAlignment = 0 });

            tableDetail.AddCell(new PdfPCell(new Phrase("Item Manual Code:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("ACID01", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Base Price", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Gallon(s)", cellFont)) { Border = 0, HorizontalAlignment = 0 });

            tableDetail.AddCell(new PdfPCell(new Phrase("Item:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Apollo Muriatic Acid", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Is Inventory:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("[Checkbox ni dire master]", cellFont)) { Border = 0, HorizontalAlignment = 0 });

            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Particulars:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("NA", cellFont)) { Border = 0, HorizontalAlignment = 0 });

            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });

            tableDetail.AddCell(new PdfPCell(new Phrase("Category:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("supplies", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Group:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Office and Toolroom Supplies", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });

            tableDetail.AddCell(new PdfPCell(new Phrase("Account:", columnFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Office and Toolroom Supplies", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35, PaddingTop = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0, PaddingTop = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0, PaddingTop = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Sales Account:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Office and Toolroom Supplies", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Cost Account:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Office and Toolroom Supplies", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Asset Account:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Office and Toolroom Supplies", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Exense Account:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Office and Toolroom Supplies", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("")) { Border = 0 });

            tableDetail.AddCell(new PdfPCell(new Phrase("VAT Output:", columnFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("VAT OUTPUT", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35, PaddingTop = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Date Acquired:", columnFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("09/06/2011", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 35 });

            tableDetail.AddCell(new PdfPCell(new Phrase("VAT Input:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("VAT INPUT", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Useful Life(Years):", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("0.00", cellFont)) { Border = 0, HorizontalAlignment = 0 });

            tableDetail.AddCell(new PdfPCell(new Phrase("Withholding Tax:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("WTAX ZERO RATE", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingLeft = 35 });
            tableDetail.AddCell(new PdfPCell(new Phrase("Salvage Value:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
            tableDetail.AddCell(new PdfPCell(new Phrase("0.00", cellFont)) { Border = 0, HorizontalAlignment = 0 });
            document.Add(tableDetail);

            Paragraph line2 = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            document.Add(line2);

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
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 9);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Stock In", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(branch, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    document.Add(line);

                    var stockIns = from d in db.TrnStockIns
                                   where d.Id == StockInId
                                   select new Models.TrnStockIn
                                   {
                                       Id = d.Id,
                                       BranchId = d.BranchId,
                                       Branch = d.MstBranch.Branch,
                                       INNumber = d.INNumber,
                                       INDate = d.INDate.ToShortDateString(),
                                       AccountId = d.AccountId,
                                       Account = d.MstAccount.Account,
                                       ArticleId = d.ArticleId,
                                       Article = d.MstArticle.Article,
                                       Particulars = d.Particulars,
                                       ManualINNumber = d.ManualINNumber,
                                       IsProduced = d.IsProduced,
                                       PreparedById = d.PreparedById,
                                       PreparedBy = d.MstUser3.FullName,
                                       CheckedById = d.CheckedById,
                                       CheckedBy = d.MstUser1.FullName,
                                       ApprovedById = d.ApprovedById,
                                       ApprovedBy = d.MstUser.FullName,
                                       IsLocked = d.IsLocked,
                                       CreatedById = d.CreatedById,
                                       CreatedBy = d.MstUser2.FullName,
                                       CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                       UpdatedById = d.UpdatedById,
                                       UpdatedBy = d.MstUser4.FullName,
                                       UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                   };

                    String Account = "", Particulars = "";
                    String INNumber = "", INDate = "";
                    String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                    foreach (var stockIn in stockIns)
                    {
                        Account = stockIn.Account;
                        Particulars = stockIn.Particulars;
                        INNumber = stockIn.INNumber;
                        INDate = stockIn.INDate;
                        PreparedBy = stockIn.PreparedBy;
                        CheckedBy = stockIn.CheckedBy;
                        ApprovedBy = stockIn.ApprovedBy;
                    }

                    PdfPTable tableSubHeader = new PdfPTable(4);
                    float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                    tableSubHeader.SetWidths(widthscellsSubheader);
                    tableSubHeader.WidthPercentage = 100;
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Account: ", columnFont)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Account, cellFont2)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("IN Number: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(INNumber, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Particulars: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Particulars, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("IN Date: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(INDate, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    document.Add(tableSubHeader);
                    document.Add(Chunk.NEWLINE);

                    var stockInItems = from d in db.TrnStockInItems
                                       where d.INId == StockInId
                                       select new Models.TrnStockInItem
                                       {
                                           Id = d.Id,
                                           INId = d.INId,
                                           IN = d.TrnStockIn.INNumber,
                                           ItemId = d.ItemId,
                                           ItemCode = d.MstArticle.ManualArticleCode,
                                           Item = d.MstArticle.Article,
                                           Particulars = d.Particulars,
                                           UnitId = d.UnitId,
                                           Unit = d.MstUnit.Unit,
                                           Quantity = d.Quantity,
                                           Cost = d.Cost,
                                           Amount = d.Amount,
                                           BaseUnitId = d.BaseUnitId,
                                           BaseUnit = d.MstUnit1.Unit,
                                           BaseQuantity = d.BaseQuantity,
                                           BaseCost = d.BaseCost
                                       };

                    PdfPTable tableINLines = new PdfPTable(6);
                    float[] widthscellsINLines = new float[] { 50f, 60f, 100f, 150f, 100f, 100f };
                    tableINLines.SetWidths(widthscellsINLines);
                    tableINLines.WidthPercentage = 100;

                    tableINLines.AddCell(new PdfPCell(new Phrase("Quantity", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableINLines.AddCell(new PdfPCell(new Phrase("Unit", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableINLines.AddCell(new PdfPCell(new Phrase("Code", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableINLines.AddCell(new PdfPCell(new Phrase("Item", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableINLines.AddCell(new PdfPCell(new Phrase("Cost", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableINLines.AddCell(new PdfPCell(new Phrase("Amount", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    Decimal TotalAmount = 0;
                    foreach (var stockInItem in stockInItems)
                    {
                        tableINLines.AddCell(new PdfPCell(new Phrase(stockInItem.Quantity.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableINLines.AddCell(new PdfPCell(new Phrase(stockInItem.Unit, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableINLines.AddCell(new PdfPCell(new Phrase(stockInItem.ItemCode, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableINLines.AddCell(new PdfPCell(new Phrase(stockInItem.Item, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableINLines.AddCell(new PdfPCell(new Phrase(stockInItem.Cost.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableINLines.AddCell(new PdfPCell(new Phrase(stockInItem.Amount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        TotalAmount = TotalAmount + stockInItem.Amount;
                    }

                    document.Add(tableINLines);

                    document.Add(Chunk.NEWLINE);

                    PdfPTable tableINLineTotalAmount = new PdfPTable(6);
                    float[] widthscellsINLinesTotalAmount = new float[] { 50f, 60f, 100f, 150f, 100f, 100f };
                    tableINLineTotalAmount.SetWidths(widthscellsINLinesTotalAmount);
                    tableINLineTotalAmount.WidthPercentage = 100;

                    tableINLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableINLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableINLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableINLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableINLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableINLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    document.Add(tableINLineTotalAmount);

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

                    tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });

                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                    tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });

                    document.Add(tableFooter);

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
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
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 9);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Stock Out", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(branch, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    document.Add(line);

                    var stockOuts = from d in db.TrnStockOuts
                                    where d.Id == StockOutId
                                    select new Models.TrnStockOut
                                    {
                                        Id = d.Id,
                                        BranchId = d.BranchId,
                                        Branch = d.MstBranch.Branch,
                                        OTNumber = d.OTNumber,
                                        OTDate = d.OTDate.ToShortDateString(),
                                        AccountId = d.AccountId,
                                        Account = d.MstAccount.Account,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.Article,
                                        Particulars = d.Particulars,
                                        ManualOTNumber = d.ManualOTNumber,
                                        PreparedById = d.PreparedById,
                                        PreparedBy = d.MstUser3.FullName,
                                        CheckedById = d.CheckedById,
                                        CheckedBy = d.MstUser1.FullName,
                                        ApprovedById = d.ApprovedById,
                                        ApprovedBy = d.MstUser.FullName,
                                        IsLocked = d.IsLocked,
                                        CreatedById = d.CreatedById,
                                        CreatedBy = d.MstUser2.FullName,
                                        CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                        UpdatedById = d.UpdatedById,
                                        UpdatedBy = d.MstUser4.FullName,
                                        UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                    };

                    String Account = "", Particulars = "";
                    String OUTNumber = "", OUTDate = "";
                    String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                    foreach (var stockOut in stockOuts)
                    {
                        Account = stockOut.Account;
                        Particulars = stockOut.Particulars;
                        OUTNumber = stockOut.OTNumber;
                        OUTDate = stockOut.OTDate;
                        PreparedBy = stockOut.PreparedBy;
                        CheckedBy = stockOut.CheckedBy;
                        ApprovedBy = stockOut.ApprovedBy;
                    }

                    PdfPTable tableSubHeader = new PdfPTable(4);
                    float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                    tableSubHeader.SetWidths(widthscellsSubheader);
                    tableSubHeader.WidthPercentage = 100;
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Account: ", columnFont)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Account, cellFont2)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("OUT Number: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(OUTNumber, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Particulars: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Particulars, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("OUT Date: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(OUTDate, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    document.Add(tableSubHeader);
                    document.Add(Chunk.NEWLINE);

                    var stockOutItems = from d in db.TrnStockOutItems
                                        where d.OTId == StockOutId
                                        select new Models.TrnStockOutItem
                                        {
                                            Id = d.Id,
                                            OTId = d.OTId,
                                            OT = d.TrnStockOut.OTNumber,
                                            ExpenseAccountId = d.ExpenseAccountId,
                                            ExpenseAccount = d.MstAccount.Account,
                                            ItemId = d.ItemId,
                                            ItemCode = d.MstArticle.ManualArticleCode,
                                            Item = d.MstArticle.Article,
                                            ItemInventoryId = d.ItemInventoryId,
                                            ItemInventory = d.MstArticleInventory.InventoryCode,
                                            Particulars = d.Particulars,
                                            UnitId = d.UnitId,
                                            Unit = d.MstUnit.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            BaseUnitId = d.BaseUnitId,
                                            BaseUnit = d.MstUnit1.Unit,
                                            BaseQuantity = d.BaseQuantity,
                                            BaseCost = d.BaseCost
                                        };

                    PdfPTable tableOUTLines = new PdfPTable(6);
                    float[] widthscellsOUTLines = new float[] { 50f, 60f, 100f, 150f, 100f, 100f };
                    tableOUTLines.SetWidths(widthscellsOUTLines);
                    tableOUTLines.WidthPercentage = 100;

                    tableOUTLines.AddCell(new PdfPCell(new Phrase("Quantity", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableOUTLines.AddCell(new PdfPCell(new Phrase("Unit", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableOUTLines.AddCell(new PdfPCell(new Phrase("Code", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableOUTLines.AddCell(new PdfPCell(new Phrase("Item", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableOUTLines.AddCell(new PdfPCell(new Phrase("Cost", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableOUTLines.AddCell(new PdfPCell(new Phrase("Amount", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    Decimal TotalAmount = 0;
                    foreach (var stockOutItem in stockOutItems)
                    {
                        tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.Quantity.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.Unit, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.ItemCode, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.Item, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.Cost.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableOUTLines.AddCell(new PdfPCell(new Phrase(stockOutItem.Amount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        TotalAmount = TotalAmount + stockOutItem.Amount;
                    }

                    document.Add(tableOUTLines);

                    document.Add(Chunk.NEWLINE);

                    PdfPTable tableOUTLineTotalAmount = new PdfPTable(6);
                    float[] widthscellsOUTLinesTotalAmount = new float[] { 50f, 60f, 100f, 150f, 100f, 100f };
                    tableOUTLineTotalAmount.SetWidths(widthscellsOUTLinesTotalAmount);
                    tableOUTLineTotalAmount.WidthPercentage = 100;

                    tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableOUTLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    document.Add(tableOUTLineTotalAmount);

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

                    tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });

                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                    tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });

                    document.Add(tableFooter);

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
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
        public ActionResult StockTransferDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockTransferPDF(Int32 StockTransferId)
        {
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 9);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Stock Transfer", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(branch, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    document.Add(line);

                    var stockTransfers = from d in db.TrnStockTransfers
                                         where d.Id == StockTransferId
                                         select new Models.TrnStockTransfer
                                         {
                                             Id = d.Id,
                                             BranchId = d.BranchId,
                                             Branch = d.MstBranch.Branch,
                                             STNumber = d.STNumber,
                                             STDate = d.STDate.ToShortDateString(),
                                             ToBranchId = d.ToBranchId,
                                             ToBranch = d.MstBranch1.Branch,
                                             Particulars = d.Particulars,
                                             ManualSTNumber = d.ManualSTNumber,
                                             PreparedById = d.PreparedById,
                                             PreparedBy = d.MstUser3.FullName,
                                             CheckedById = d.CheckedById,
                                             CheckedBy = d.MstUser1.FullName,
                                             ApprovedById = d.ApprovedById,
                                             ApprovedBy = d.MstUser.FullName,
                                             IsLocked = d.IsLocked,
                                             CreatedById = d.CreatedById,
                                             CreatedBy = d.MstUser2.FullName,
                                             CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                             UpdatedById = d.UpdatedById,
                                             UpdatedBy = d.MstUser4.FullName,
                                             UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                         };

                    String FromBranch = "", ToBranch = "", Particulars = "";
                    String STNumber = "", STDate = "";
                    String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                    foreach (var stockTransfer in stockTransfers)
                    {
                        FromBranch = stockTransfer.Branch;
                        ToBranch = stockTransfer.ToBranch;
                        Particulars = stockTransfer.Particulars;
                        STNumber = stockTransfer.STNumber;
                        STDate = stockTransfer.STDate;
                        PreparedBy = stockTransfer.PreparedBy;
                        CheckedBy = stockTransfer.CheckedBy;
                        ApprovedBy = stockTransfer.ApprovedBy;
                    }

                    PdfPTable tableSubHeader = new PdfPTable(4);
                    float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                    tableSubHeader.SetWidths(widthscellsSubheader);
                    tableSubHeader.WidthPercentage = 100;
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("From Branch: ", columnFont)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(FromBranch, cellFont2)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("ST Number: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(STNumber, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("To Branch: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(ToBranch, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("ST Date: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(STDate, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Particulars: ", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Particulars, cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });

                    document.Add(tableSubHeader);
                    document.Add(Chunk.NEWLINE);

                    var stockTransferItems = from d in db.TrnStockTransferItems
                                             where d.STId == StockTransferId
                                             select new Models.TrnStockTransferItem
                                             {
                                                 Id = d.Id,
                                                 STId = d.STId,
                                                 ST = d.TrnStockTransfer.STNumber,
                                                 ItemId = d.ItemId,
                                                 ItemCode = d.MstArticle.ManualArticleCode,
                                                 Item = d.MstArticle.Article,
                                                 ItemInventoryId = d.ItemInventoryId,
                                                 ItemInventory = d.MstArticleInventory.InventoryCode,
                                                 Particulars = d.Particulars,
                                                 UnitId = d.UnitId,
                                                 Unit = d.MstUnit.Unit,
                                                 Quantity = d.Quantity,
                                                 Cost = d.Cost,
                                                 Amount = d.Amount,
                                                 BaseUnitId = d.BaseUnitId,
                                                 BaseUnit = d.MstUnit1.Unit,
                                                 BaseQuantity = d.BaseQuantity,
                                                 BaseCost = d.BaseCost
                                             };

                    PdfPTable tableSTLines = new PdfPTable(6);
                    float[] widthscellsSTLines = new float[] { 50f, 60f, 100f, 150f, 100f, 100f };
                    tableSTLines.SetWidths(widthscellsSTLines);
                    tableSTLines.WidthPercentage = 100;

                    tableSTLines.AddCell(new PdfPCell(new Phrase("Quantity", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTLines.AddCell(new PdfPCell(new Phrase("Unit", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTLines.AddCell(new PdfPCell(new Phrase("Code", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTLines.AddCell(new PdfPCell(new Phrase("Item", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTLines.AddCell(new PdfPCell(new Phrase("Cost", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTLines.AddCell(new PdfPCell(new Phrase("Amount", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    Decimal TotalAmount = 0;
                    foreach (var stockTransferItem in stockTransferItems)
                    {
                        tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.Quantity.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.Unit, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.ItemCode, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.Item, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.Cost.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTLines.AddCell(new PdfPCell(new Phrase(stockTransferItem.Amount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        TotalAmount = TotalAmount + stockTransferItem.Amount;
                    }

                    document.Add(tableSTLines);
                    document.Add(Chunk.NEWLINE);

                    PdfPTable tableSTLineTotalAmount = new PdfPTable(6);
                    float[] widthscellsSTLinesTotalAmount = new float[] { 50f, 60f, 100f, 150f, 100f, 100f };
                    tableSTLineTotalAmount.SetWidths(widthscellsSTLinesTotalAmount);
                    tableSTLineTotalAmount.WidthPercentage = 100;

                    tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase("Total:", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableSTLineTotalAmount.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    document.Add(tableSTLineTotalAmount);

                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);
                    document.Add(Chunk.NEWLINE);

                    // Table for Footer
                    PdfPTable tableFooter = new PdfPTable(7);
                    tableFooter.WidthPercentage = 100;
                    float[] widthsCells2 = new float[] { 100f, 20f, 100f, 20f, 100f, 20f, 100f };
                    tableFooter.SetWidths(widthsCells2);

                    tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Received by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });

                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                    tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });

                    document.Add(tableFooter);

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
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
            document.SetMargins(50f, 50f, 50f, 50f);
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
            document.SetMargins(50f, 50f, 50f, 50f);
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
            // retrieve account category journal
            var journals = from d in db.TrnJournals
                           where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                           && d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory != "Income"
                           && d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory != "Expenses"
                           && d.MstBranch.CompanyId == CompanyId
                           group d by new
                           {
                               AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
                               SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription,
                               AccountType = d.MstAccount.MstAccountType.AccountType,
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account
                           } into g
                           select new Models.TrnJournal
                           {
                               AccountCategory = g.Key.AccountCategory,
                               SubCategoryDescription = g.Key.SubCategoryDescription,
                               AccountType = g.Key.AccountType,
                               AccountCode = g.Key.AccountCode,
                               Account = g.Key.Account,
                               DebitAmount = g.Sum(d => d.DebitAmount),
                               CreditAmount = g.Sum(d => d.CreditAmount)
                           };

            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();


            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
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

            // retrieve account sub category journal for Asset
            var accountTypeSubCategory_Journal_Assets = from d in db.TrnJournals
                                                        where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                                                        && d.MstAccount.MstAccountType.MstAccountCategory.Id == 1
                                                        && d.MstBranch.CompanyId == CompanyId
                                                        group d by new
                                                        {
                                                            AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
                                                            SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription
                                                        } into g
                                                        select new Models.TrnJournal
                                                        {
                                                            AccountCategory = g.Key.AccountCategory,
                                                            SubCategoryDescription = g.Key.SubCategoryDescription,
                                                            DebitAmount = g.Sum(d => d.DebitAmount),
                                                            CreditAmount = g.Sum(d => d.CreditAmount)
                                                        };

            // retrieve account type journal for Asset
            var accountType_Journal_Assets = from d in db.TrnJournals
                                             where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                                             && d.MstAccount.MstAccountType.MstAccountCategory.Id == 1
                                             && d.MstBranch.CompanyId == CompanyId
                                             group d by new
                                             {
                                                 AccountType = d.MstAccount.MstAccountType.AccountType
                                             } into g
                                             select new Models.TrnJournal
                                             {
                                                 AccountType = g.Key.AccountType,
                                                 DebitAmount = g.Sum(d => d.DebitAmount),
                                                 CreditAmount = g.Sum(d => d.CreditAmount)
                                             };

            Decimal totalCurrentAsset = 0;
            Decimal totalCurrentLiabilities = 0;
            Decimal totalStockHoldersEquity = 0;

            if (accountTypeSubCategory_Journal_Assets.Any())
            {
                if (accountType_Journal_Assets.Any())
                {
                    foreach (var accountTypeSubCategory_Journal_Asset in accountTypeSubCategory_Journal_Assets)
                    {
                        // table Balance Sheet header
                        PdfPTable tableBalanceSheetHeader = new PdfPTable(3);
                        float[] widthCellsTableBalanceSheetHeader = new float[] { 50f, 100f, 50f };
                        tableBalanceSheetHeader.SetWidths(widthCellsTableBalanceSheetHeader);
                        tableBalanceSheetHeader.WidthPercentage = 100;

                        document.Add(line);

                        PdfPCell headerSubCategoryColspan = (new PdfPCell(new Phrase(accountTypeSubCategory_Journal_Asset.SubCategoryDescription, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        headerSubCategoryColspan.Colspan = 3;
                        tableBalanceSheetHeader.AddCell(headerSubCategoryColspan);

                        document.Add(tableBalanceSheetHeader);
                        //totalCurrentAsset = accountTypeSubCategory_Journal_Asset.DebitAmount;
                    }

                    foreach (var accountType_JournalAsset in accountType_Journal_Assets)
                    {
                        Decimal balanceAssetAmount = accountType_JournalAsset.DebitAmount - accountType_JournalAsset.CreditAmount;

                        // table Balance Sheet
                        PdfPTable tableBalanceSheetAccounts = new PdfPTable(3);
                        float[] widthCellsTableBalanceSheetAccounts = new float[] { 50f, 100f, 50f };
                        tableBalanceSheetAccounts.SetWidths(widthCellsTableBalanceSheetAccounts);
                        tableBalanceSheetAccounts.WidthPercentage = 100;

                        tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accountType_JournalAsset.AccountType, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                        tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceAssetAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

                        totalCurrentAsset = totalCurrentAsset + balanceAssetAmount;

                        // retrieve accounts journal Asset
                        var accounts_JournalAssets = from d in db.TrnJournals
                                                     where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                                                     && d.MstAccount.MstAccountType.AccountType == accountType_JournalAsset.AccountType
                                                     && d.MstAccount.MstAccountType.MstAccountCategory.Id == 1
                                                     && d.MstBranch.CompanyId == CompanyId
                                                     group d by new
                                                     {
                                                         AccountCode = d.MstAccount.AccountCode,
                                                         Account = d.MstAccount.Account
                                                     } into g
                                                     select new Models.TrnJournal
                                                     {
                                                         AccountCode = g.Key.AccountCode,
                                                         Account = g.Key.Account,
                                                         DebitAmount = g.Sum(d => d.DebitAmount),
                                                         CreditAmount = g.Sum(d => d.CreditAmount)
                                                     };

                        foreach (var accounts_JournalAsset in accounts_JournalAssets)
                        {
                            Decimal balanceAssetAmountForAccounts = accounts_JournalAsset.DebitAmount - accounts_JournalAsset.CreditAmount;

                            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalAsset.AccountCode, cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalAsset.Account, cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceAssetAmountForAccounts.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        }

                        document.Add(tableBalanceSheetAccounts);
                    }

                    document.Add(line);
                    foreach (var accountTypeSubCategory_Journal_Asset_Footer in accountTypeSubCategory_Journal_Assets)
                    {
                        // table Balance Sheet footer in Asset CAtegory
                        PdfPTable tableBalanceSheetFooterAsset = new PdfPTable(4);
                        float[] widthCellsTableBalanceSheetFooterAsset = new float[] { 20f, 20f, 150f, 30f };
                        tableBalanceSheetFooterAsset.SetWidths(widthCellsTableBalanceSheetFooterAsset);
                        tableBalanceSheetFooterAsset.WidthPercentage = 100;

                        tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("Total " + accountTypeSubCategory_Journal_Asset_Footer.SubCategoryDescription, cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase(totalCurrentAsset.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase("Total Asset", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableBalanceSheetFooterAsset.AddCell(new PdfPCell(new Phrase(totalCurrentAsset.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        document.Add(tableBalanceSheetFooterAsset);
                        document.Add(Chunk.NEWLINE);
                    }
                }
            }

            // retrieve account sub category journal Liabilities
            var accountTypeSubCategory_JournalLiabilities = from d in db.TrnJournals
                                                            where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                                                            && d.MstAccount.MstAccountType.MstAccountCategory.Id == 2
                                                            && d.MstBranch.CompanyId == CompanyId
                                                            group d by new
                                                            {
                                                                AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
                                                                SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription
                                                            } into g
                                                            select new Models.TrnJournal
                                                            {
                                                                AccountCategory = g.Key.AccountCategory,
                                                                SubCategoryDescription = g.Key.SubCategoryDescription,
                                                                DebitAmount = g.Sum(d => d.DebitAmount),
                                                                CreditAmount = g.Sum(d => d.CreditAmount)
                                                            };

            // retrieve account type journal Liabilities
            var accountTypeJournal_Liabilities = from d in db.TrnJournals
                                                 where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                                                 && d.MstAccount.MstAccountType.MstAccountCategory.Id == 2
                                                 && d.MstBranch.CompanyId == CompanyId
                                                 group d by new
                                                 {
                                                     AccountType = d.MstAccount.MstAccountType.AccountType
                                                 } into g
                                                 select new Models.TrnJournal
                                                 {
                                                     AccountType = g.Key.AccountType,
                                                     DebitAmount = g.Sum(d => d.DebitAmount),
                                                     CreditAmount = g.Sum(d => d.CreditAmount)
                                                 };

            if (accountTypeSubCategory_JournalLiabilities.Any())
            {
                if (accountTypeJournal_Liabilities.Any())
                {
                    foreach (var accountTypeSubCategory_JournalsLiability in accountTypeSubCategory_JournalLiabilities)
                    {
                        // table Balance Sheet account Type Sub Category liabilities
                        PdfPTable tableBalanceSheetHeader = new PdfPTable(3);
                        float[] widthCellsTableBalanceSheetHeader = new float[] { 50f, 100f, 50f };
                        tableBalanceSheetHeader.SetWidths(widthCellsTableBalanceSheetHeader);
                        tableBalanceSheetHeader.WidthPercentage = 100;

                        document.Add(line);

                        PdfPCell headerSubCategoryColspan = (new PdfPCell(new Phrase(accountTypeSubCategory_JournalsLiability.SubCategoryDescription, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        headerSubCategoryColspan.Colspan = 3;
                        tableBalanceSheetHeader.AddCell(headerSubCategoryColspan);

                        document.Add(tableBalanceSheetHeader);
                        //totalCurrentLiabilities = accountTypeSubCategory_JournalsLiability.CreditAmount;
                    }

                    foreach (var accountTypeJournal_Liability in accountTypeJournal_Liabilities)
                    {
                        Decimal balanceLiabilityAmount = accountTypeJournal_Liability.CreditAmount - accountTypeJournal_Liability.DebitAmount;

                        // table Balance Sheet liabilities
                        PdfPTable tableBalanceSheetAccounts = new PdfPTable(3);
                        float[] widthCellsTableBalanceSheetAccounts = new float[] { 50f, 100f, 50f };
                        tableBalanceSheetAccounts.SetWidths(widthCellsTableBalanceSheetAccounts);
                        tableBalanceSheetAccounts.WidthPercentage = 100;

                        tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accountTypeJournal_Liability.AccountType, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                        tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceLiabilityAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

                        totalCurrentLiabilities = totalCurrentLiabilities + balanceLiabilityAmount;

                        // retrieve accounts journal Liabilities
                        var accounts_JournalsLiabilities = from d in db.TrnJournals
                                                           where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                                                           && d.MstAccount.MstAccountType.AccountType == accountTypeJournal_Liability.AccountType
                                                           && d.MstAccount.MstAccountType.MstAccountCategory.Id == 2
                                                           && d.MstBranch.CompanyId == CompanyId
                                                           group d by new
                                                           {
                                                               AccountCode = d.MstAccount.AccountCode,
                                                               Account = d.MstAccount.Account
                                                           } into g
                                                           select new Models.TrnJournal
                                                           {
                                                               AccountCode = g.Key.AccountCode,
                                                               Account = g.Key.Account,
                                                               DebitAmount = g.Sum(d => d.DebitAmount),
                                                               CreditAmount = g.Sum(d => d.CreditAmount)
                                                           };

                        foreach (var accounts_JournalsLiability in accounts_JournalsLiabilities)
                        {
                            Decimal balanceLiabilityAmountForAccounts = accounts_JournalsLiability.CreditAmount - accounts_JournalsLiability.DebitAmount;

                            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalsLiability.AccountCode, cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalsLiability.Account, cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceLiabilityAmountForAccounts.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        }

                        document.Add(tableBalanceSheetAccounts);
                    }

                    document.Add(line);

                    foreach (var accountTypeSubCategory_JournalsLiability_Footer in accountTypeSubCategory_JournalLiabilities)
                    {
                        // table Balance Sheet
                        PdfPTable tableBalanceSheetFooterLiabilities = new PdfPTable(4);
                        float[] widthCellsTableBalanceSheetFooterLiablities = new float[] { 20f, 20f, 150f, 30f };
                        tableBalanceSheetFooterLiabilities.SetWidths(widthCellsTableBalanceSheetFooterLiablities);
                        tableBalanceSheetFooterLiabilities.WidthPercentage = 100;

                        tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("Total " + accountTypeSubCategory_JournalsLiability_Footer.SubCategoryDescription, cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase(totalCurrentLiabilities.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase("Total Liabilities", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableBalanceSheetFooterLiabilities.AddCell(new PdfPCell(new Phrase(totalCurrentLiabilities.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        document.Add(tableBalanceSheetFooterLiabilities);
                        document.Add(Chunk.NEWLINE);
                    }
                }
            }

            // retrieve account sub category journal Equity
            var accountTypeSubCategory_JournalEquities = from d in db.TrnJournals
                                                         where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                                                         && d.MstAccount.MstAccountType.MstAccountCategory.Id == 4
                                                         && d.MstBranch.CompanyId == CompanyId
                                                         group d by new
                                                         {
                                                             AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
                                                             SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription
                                                         } into g
                                                         select new Models.TrnJournal
                                                         {
                                                             AccountCategory = g.Key.AccountCategory,
                                                             SubCategoryDescription = g.Key.SubCategoryDescription,
                                                             DebitAmount = g.Sum(d => d.DebitAmount),
                                                             CreditAmount = g.Sum(d => d.CreditAmount)
                                                         };

            // retrieve account type journal Equity
            var accountTypeJournal_Equities = from d in db.TrnJournals
                                              where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                                              && d.MstAccount.MstAccountType.MstAccountCategory.Id == 4
                                              && d.MstBranch.CompanyId == CompanyId
                                              group d by new
                                              {
                                                  AccountType = d.MstAccount.MstAccountType.AccountType
                                              } into g
                                              select new Models.TrnJournal
                                              {
                                                  AccountType = g.Key.AccountType,
                                                  DebitAmount = g.Sum(d => d.DebitAmount),
                                                  CreditAmount = g.Sum(d => d.CreditAmount)
                                              };

            if (accountTypeSubCategory_JournalEquities.Any())
            {
                if (accountTypeJournal_Equities.Any())
                {
                    foreach (var accountTypeSubCategory_JournalEquity in accountTypeSubCategory_JournalEquities)
                    {
                        // table Balance Sheet Equity
                        PdfPTable tableBalanceSheetHeader = new PdfPTable(3);
                        float[] widthCellsTableBalanceSheetHeader = new float[] { 50f, 100f, 50f };
                        tableBalanceSheetHeader.SetWidths(widthCellsTableBalanceSheetHeader);
                        tableBalanceSheetHeader.WidthPercentage = 100;

                        document.Add(line);

                        PdfPCell headerSubCategoryColspan = (new PdfPCell(new Phrase(accountTypeSubCategory_JournalEquity.SubCategoryDescription, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        headerSubCategoryColspan.Colspan = 3;
                        tableBalanceSheetHeader.AddCell(headerSubCategoryColspan);

                        document.Add(tableBalanceSheetHeader);
                        //totalStockHoldersEquity = accountTypeSubCategory_JournalEquity.CreditAmount;
                    }

                    foreach (var accountTypeJournal_Equity in accountTypeJournal_Equities)
                    {
                        Decimal balanceEquityAmount = accountTypeJournal_Equity.CreditAmount - accountTypeJournal_Equity.DebitAmount;

                        // table Balance Sheet Equity
                        PdfPTable tableBalanceSheetAccounts = new PdfPTable(3);
                        float[] widthCellsTableBalanceSheetAccounts = new float[] { 50f, 100f, 50f };
                        tableBalanceSheetAccounts.SetWidths(widthCellsTableBalanceSheetAccounts);
                        tableBalanceSheetAccounts.WidthPercentage = 100;

                        tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accountTypeJournal_Equity.AccountType, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                        tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceEquityAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

                        totalStockHoldersEquity = totalStockHoldersEquity + balanceEquityAmount;

                        // retrieve accounts journal Equity
                        var accounts_JournalEquities = from d in db.TrnJournals
                                                       where d.JournalDate <= Convert.ToDateTime(DateAsOf)
                                                       && d.MstAccount.MstAccountType.AccountType == accountTypeJournal_Equity.AccountType
                                                       && d.MstAccount.MstAccountType.MstAccountCategory.Id == 4
                                                       && d.MstBranch.CompanyId == CompanyId
                                                       group d by new
                                                       {
                                                           AccountCode = d.MstAccount.AccountCode,
                                                           Account = d.MstAccount.Account
                                                       } into g
                                                       select new Models.TrnJournal
                                                       {
                                                           AccountCode = g.Key.AccountCode,
                                                           Account = g.Key.Account,
                                                           DebitAmount = g.Sum(d => d.DebitAmount),
                                                           CreditAmount = g.Sum(d => d.CreditAmount)
                                                       };

                        foreach (var accounts_JournalEquity in accounts_JournalEquities)
                        {
                            Decimal balanceEquityAmountForAccounts = accounts_JournalEquity.CreditAmount - accounts_JournalEquity.DebitAmount;

                            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalEquity.AccountCode, cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalEquity.Account, cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                            tableBalanceSheetAccounts.AddCell(new PdfPCell(new Phrase(balanceEquityAmountForAccounts.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        }

                        document.Add(tableBalanceSheetAccounts);
                    }

                    document.Add(line);

                    foreach (var accountTypeSubCategory_JournalEquity_Footer in accountTypeSubCategory_JournalEquities)
                    {
                        // table Balance Sheet
                        PdfPTable tableBalanceSheetFooterEquity = new PdfPTable(4);
                        float[] widthCellsTableBalanceSheetFooterEquity = new float[] { 20f, 20f, 150f, 30f };
                        tableBalanceSheetFooterEquity.SetWidths(widthCellsTableBalanceSheetFooterEquity);
                        tableBalanceSheetFooterEquity.WidthPercentage = 100;

                        tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("Total " + accountTypeSubCategory_JournalEquity_Footer.SubCategoryDescription, cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase(totalStockHoldersEquity.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase("Total Equity", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableBalanceSheetFooterEquity.AddCell(new PdfPCell(new Phrase(totalStockHoldersEquity.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        document.Add(tableBalanceSheetFooterEquity);
                        document.Add(Chunk.NEWLINE);
                    }
                }
            }

            document.Add(line);

            Decimal totalLiabilityAndEquity = totalCurrentLiabilities + totalStockHoldersEquity;
            Decimal totalBalance = totalCurrentAsset - totalCurrentLiabilities - totalStockHoldersEquity;

            // table Balance Sheet
            PdfPTable tableBalanceSheetFooterTotalLiabilityAndEquity = new PdfPTable(4);
            float[] widthCellsTableBalanceSheetFooterTotalLiabilityAndEquity = new float[] { 20f, 20f, 150f, 30f };
            tableBalanceSheetFooterTotalLiabilityAndEquity.SetWidths(widthCellsTableBalanceSheetFooterTotalLiabilityAndEquity);
            tableBalanceSheetFooterTotalLiabilityAndEquity.WidthPercentage = 100;

            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("Total Liability and Equity", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase(totalLiabilityAndEquity.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("Balance", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase(totalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

            document.Add(tableBalanceSheetFooterTotalLiabilityAndEquity);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementIncomeStatementPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // retrieve account category journal
            var journals = from d in db.TrnJournals
                           where d.JournalDate >= Convert.ToDateTime(StartDate)
                           && d.JournalDate <= Convert.ToDateTime(EndDate)
                           && d.MstBranch.CompanyId == CompanyId
                           group d by new
                           {
                               AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
                               SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription,
                               AccountType = d.MstAccount.MstAccountType.AccountType,
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account
                           } into g
                           select new Models.TrnJournal
                           {
                               AccountCategory = g.Key.AccountCategory,
                               SubCategoryDescription = g.Key.SubCategoryDescription,
                               AccountType = g.Key.AccountType,
                               AccountCode = g.Key.AccountCode,
                               Account = g.Key.Account,
                               DebitAmount = g.Sum(d => d.DebitAmount),
                               CreditAmount = g.Sum(d => d.CreditAmount)
                           };

            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
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
            tableHeader.AddCell(new PdfPCell(new Phrase("Income Statement", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // retrieve account sub category journal for income
            var accountTypeSubCategory_Journal_Incomes = from d in db.TrnJournals
                                                         where d.JournalDate >= Convert.ToDateTime(StartDate)
                                                         && d.JournalDate <= Convert.ToDateTime(EndDate)
                                                         && d.MstAccount.MstAccountType.MstAccountCategory.Id == 5
                                                         && d.MstBranch.CompanyId == CompanyId
                                                         group d by new
                                                         {
                                                             AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
                                                             SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription
                                                         } into g
                                                         select new Models.TrnJournal
                                                         {
                                                             AccountCategory = g.Key.AccountCategory,
                                                             SubCategoryDescription = g.Key.SubCategoryDescription,
                                                             DebitAmount = g.Sum(d => d.DebitAmount),
                                                             CreditAmount = g.Sum(d => d.CreditAmount)
                                                         };

            // retrieve account type journal for income
            var accountType_Journal_Incomes = from d in db.TrnJournals
                                              where d.JournalDate >= Convert.ToDateTime(StartDate)
                                              && d.JournalDate <= Convert.ToDateTime(EndDate)
                                              && d.MstAccount.MstAccountType.MstAccountCategory.Id == 5
                                              && d.MstBranch.CompanyId == CompanyId
                                              group d by new
                                              {
                                                  AccountType = d.MstAccount.MstAccountType.AccountType
                                              } into g
                                              select new Models.TrnJournal
                                              {
                                                  AccountType = g.Key.AccountType,
                                                  DebitAmount = g.Sum(d => d.DebitAmount),
                                                  CreditAmount = g.Sum(d => d.CreditAmount)
                                              };

            Decimal totalRevenue = 0;
            //Decimal totalCurrentLiabilities = 0;
            //Decimal totalStockHoldersEquity = 0;

            if (accountTypeSubCategory_Journal_Incomes.Any())
            {
                if (accountType_Journal_Incomes.Any())
                {
                    foreach (var accountType_Journal_Income in accountTypeSubCategory_Journal_Incomes)
                    {
                        // table income statement
                        PdfPTable tableIncomeStatementHeader = new PdfPTable(3);
                        float[] widthCellsTableIncomeStatementHeader = new float[] { 50f, 100f, 50f };
                        tableIncomeStatementHeader.SetWidths(widthCellsTableIncomeStatementHeader);
                        tableIncomeStatementHeader.WidthPercentage = 100;

                        document.Add(line);

                        PdfPCell headerSubCategoryColspan = (new PdfPCell(new Phrase(accountType_Journal_Income.SubCategoryDescription, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        headerSubCategoryColspan.Colspan = 3;
                        tableIncomeStatementHeader.AddCell(headerSubCategoryColspan);

                        document.Add(tableIncomeStatementHeader);
                        //totalCurrentAsset = accountTypeSubCategory_Journal_Asset.DebitAmount;
                    }

                    foreach (var accountType_Journal_Income in accountType_Journal_Incomes)
                    {
                        Decimal balanceIncomeAmount = accountType_Journal_Income.CreditAmount - accountType_Journal_Income.DebitAmount;

                        // table Balance Sheet
                        PdfPTable tableIncomeStatementAccounts = new PdfPTable(3);
                        float[] widthCellsTableIncomeStatementAccounts = new float[] { 50f, 100f, 50f };
                        tableIncomeStatementAccounts.SetWidths(widthCellsTableIncomeStatementAccounts);
                        tableIncomeStatementAccounts.WidthPercentage = 100;

                        tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase(accountType_Journal_Income.AccountType, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                        tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase(balanceIncomeAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

                        totalRevenue = totalRevenue + balanceIncomeAmount;

                        // table income statement
                        var accounts_JournalIncomes = from d in db.TrnJournals
                                                      where d.JournalDate >= Convert.ToDateTime(StartDate)
                                                      && d.JournalDate <= Convert.ToDateTime(EndDate)
                                                      && d.MstAccount.MstAccountType.AccountType == accountType_Journal_Income.AccountType
                                                      && d.MstAccount.MstAccountType.MstAccountCategory.Id == 5
                                                      && d.MstBranch.CompanyId == CompanyId
                                                      group d by new
                                                      {
                                                          AccountCode = d.MstAccount.AccountCode,
                                                          Account = d.MstAccount.Account
                                                      } into g
                                                      select new Models.TrnJournal
                                                      {
                                                          AccountCode = g.Key.AccountCode,
                                                          Account = g.Key.Account,
                                                          DebitAmount = g.Sum(d => d.DebitAmount),
                                                          CreditAmount = g.Sum(d => d.CreditAmount)
                                                      };

                        foreach (var accounts_JournalIncome in accounts_JournalIncomes)
                        {
                            Decimal balanceIncomeAmountForAccounts = accounts_JournalIncome.DebitAmount - accounts_JournalIncome.CreditAmount;

                            tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalIncome.AccountCode, cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                            tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalIncome.Account, cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                            tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase(balanceIncomeAmountForAccounts.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        }

                        document.Add(tableIncomeStatementAccounts);
                    }

                    document.Add(line);
                    foreach (var accountType_Journal_Income_Footer in accountTypeSubCategory_Journal_Incomes)
                    {
                        // table Balance Sheet footer in income CAtegory
                        PdfPTable tableIncomeStatementooterIncome = new PdfPTable(4);
                        float[] widthCellsTableIncomeStatementFooterIncome = new float[] { 20f, 20f, 150f, 30f };
                        tableIncomeStatementooterIncome.SetWidths(widthCellsTableIncomeStatementFooterIncome);
                        tableIncomeStatementooterIncome.WidthPercentage = 100;

                        tableIncomeStatementooterIncome.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableIncomeStatementooterIncome.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableIncomeStatementooterIncome.AddCell(new PdfPCell(new Phrase("Total " + accountType_Journal_Income_Footer.SubCategoryDescription, cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableIncomeStatementooterIncome.AddCell(new PdfPCell(new Phrase(totalRevenue.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        document.Add(tableIncomeStatementooterIncome);
                        document.Add(Chunk.NEWLINE);
                    }
                }
            }

            // retrieve account sub category journal for Expenses
            var accountTypeSubCategory_Journal_Expenses = from d in db.TrnJournals
                                                          where d.JournalDate >= Convert.ToDateTime(StartDate)
                                                          && d.JournalDate <= Convert.ToDateTime(EndDate)
                                                          && d.MstAccount.MstAccountType.MstAccountCategory.Id == 6
                                                          && d.MstBranch.CompanyId == CompanyId
                                                          group d by new
                                                          {
                                                              AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
                                                              SubCategoryDescription = d.MstAccount.MstAccountType.SubCategoryDescription
                                                          } into g
                                                          select new Models.TrnJournal
                                                          {
                                                              AccountCategory = g.Key.AccountCategory,
                                                              SubCategoryDescription = g.Key.SubCategoryDescription,
                                                              DebitAmount = g.Sum(d => d.DebitAmount),
                                                              CreditAmount = g.Sum(d => d.CreditAmount)
                                                          };

            // retrieve account type journal for Expenses
            var accountType_Journal_Expenses = from d in db.TrnJournals
                                               where d.JournalDate >= Convert.ToDateTime(StartDate)
                                               && d.JournalDate <= Convert.ToDateTime(EndDate)
                                               && d.MstAccount.MstAccountType.MstAccountCategory.Id == 6
                                               && d.MstBranch.CompanyId == CompanyId
                                               group d by new
                                               {
                                                   AccountType = d.MstAccount.MstAccountType.AccountType
                                               } into g
                                               select new Models.TrnJournal
                                               {
                                                   AccountType = g.Key.AccountType,
                                                   DebitAmount = g.Sum(d => d.DebitAmount),
                                                   CreditAmount = g.Sum(d => d.CreditAmount)
                                               };

            Decimal totalCostOfSales = 0;
            //Decimal totalCurrentLiabilities = 0;
            //Decimal totalStockHoldersEquity = 0;

            if (accountTypeSubCategory_Journal_Expenses.Any())
            {
                if (accountType_Journal_Expenses.Any())
                {
                    foreach (var accountTypeSubCategory_Journal_Expense in accountTypeSubCategory_Journal_Expenses)
                    {
                        // table income statement
                        PdfPTable tableIncomeStatementHeader = new PdfPTable(3);
                        float[] widthCellsTableIncomeStatementHeader = new float[] { 50f, 100f, 50f };
                        tableIncomeStatementHeader.SetWidths(widthCellsTableIncomeStatementHeader);
                        tableIncomeStatementHeader.WidthPercentage = 100;

                        document.Add(line);

                        PdfPCell headerSubCategoryColspan = (new PdfPCell(new Phrase(accountTypeSubCategory_Journal_Expense.SubCategoryDescription, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        headerSubCategoryColspan.Colspan = 3;
                        tableIncomeStatementHeader.AddCell(headerSubCategoryColspan);

                        document.Add(tableIncomeStatementHeader);
                        //totalCurrentAsset = accountTypeSubCategory_Journal_Asset.DebitAmount;
                    }

                    foreach (var accountType_Journal_Expense in accountType_Journal_Expenses)
                    {
                        Decimal balanceIncomeAmount = accountType_Journal_Expense.CreditAmount - accountType_Journal_Expense.DebitAmount;

                        // table income statement
                        PdfPTable tableIncomeStatementAccounts = new PdfPTable(3);
                        float[] widthCellsTableIncomeStatementAccounts = new float[] { 50f, 100f, 50f };
                        tableIncomeStatementAccounts.SetWidths(widthCellsTableIncomeStatementAccounts);
                        tableIncomeStatementAccounts.WidthPercentage = 100;

                        tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase(accountType_Journal_Expense.AccountType, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });
                        tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase(balanceIncomeAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

                        totalCostOfSales = totalCostOfSales + balanceIncomeAmount;

                        // retrieve accounts journal income
                        var accounts_JournalExpenses = from d in db.TrnJournals
                                                       where d.JournalDate >= Convert.ToDateTime(StartDate)
                                                       && d.JournalDate <= Convert.ToDateTime(EndDate)
                                                       && d.MstAccount.MstAccountType.AccountType == accountType_Journal_Expense.AccountType
                                                       && d.MstAccount.MstAccountType.MstAccountCategory.Id == 6
                                                       && d.MstBranch.CompanyId == CompanyId
                                                       group d by new
                                                       {
                                                           AccountCode = d.MstAccount.AccountCode,
                                                           Account = d.MstAccount.Account
                                                       } into g
                                                       select new Models.TrnJournal
                                                       {
                                                           AccountCode = g.Key.AccountCode,
                                                           Account = g.Key.Account,
                                                           DebitAmount = g.Sum(d => d.DebitAmount),
                                                           CreditAmount = g.Sum(d => d.CreditAmount)
                                                       };

                        foreach (var accounts_JournalExpense in accounts_JournalExpenses)
                        {
                            Decimal balanceExpenseAmountForAccounts = accounts_JournalExpense.DebitAmount - accounts_JournalExpense.CreditAmount;

                            tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalExpense.AccountCode, cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                            tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase(accounts_JournalExpense.Account, cellFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                            tableIncomeStatementAccounts.AddCell(new PdfPCell(new Phrase(balanceExpenseAmountForAccounts.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        }

                        document.Add(tableIncomeStatementAccounts);
                    }

                    document.Add(line);
                    foreach (var accountTypeSubCategory_Journal_Expense_Footer in accountTypeSubCategory_Journal_Expenses)
                    {
                        // table Balance Sheet footer in income CAtegory
                        PdfPTable tableIncomeStatementooterExpense = new PdfPTable(4);
                        float[] widthCellsTableIncomeStatementFooterExpense = new float[] { 20f, 20f, 150f, 30f };
                        tableIncomeStatementooterExpense.SetWidths(widthCellsTableIncomeStatementFooterExpense);
                        tableIncomeStatementooterExpense.WidthPercentage = 100;

                        tableIncomeStatementooterExpense.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
                        tableIncomeStatementooterExpense.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableIncomeStatementooterExpense.AddCell(new PdfPCell(new Phrase("Total " + accountTypeSubCategory_Journal_Expense_Footer.SubCategoryDescription, cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableIncomeStatementooterExpense.AddCell(new PdfPCell(new Phrase(totalCostOfSales.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        document.Add(tableIncomeStatementooterExpense);
                        document.Add(Chunk.NEWLINE);
                    }
                }
            }


            document.Add(line);

            Decimal totalNetIncome = totalRevenue + totalCostOfSales;

            // table Balance Sheet
            PdfPTable tableBalanceSheetFooterTotalLiabilityAndEquity = new PdfPTable(4);
            float[] widthCellsTableBalanceSheetFooterTotalLiabilityAndEquity = new float[] { 20f, 20f, 150f, 30f };
            tableBalanceSheetFooterTotalLiabilityAndEquity.SetWidths(widthCellsTableBalanceSheetFooterTotalLiabilityAndEquity);
            tableBalanceSheetFooterTotalLiabilityAndEquity.WidthPercentage = 100;

            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase("Net Income", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableBalanceSheetFooterTotalLiabilityAndEquity.AddCell(new PdfPCell(new Phrase(totalNetIncome.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

            document.Add(tableBalanceSheetFooterTotalLiabilityAndEquity);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementCashFlowIndirectPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 10);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Cash Flow (Indirect)", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

                    var identityUserId = User.Identity.GetUserId();
                    var mstUserId = from d in db.MstUsers where d.UserId == identityUserId select d;
                    var incomeAccount = from d in db.MstAccounts where d.Id == mstUserId.FirstOrDefault().IncomeAccountId select d;

                    var cashFlowIncome = from d in db.TrnJournals
                                         where d.MstBranch.CompanyId == CompanyId
                                         && d.JournalDate >= Convert.ToDateTime(StartDate)
                                         && d.JournalDate <= Convert.ToDateTime(EndDate)
                                         && (d.MstAccount.MstAccountType.AccountCategoryId == 5 || d.MstAccount.MstAccountType.AccountCategoryId == 6)
                                         select new Models.TrnJournal
                                         {
                                             AccountCashFlowCode = d.MstAccount.MstAccountCashFlow.AccountCashFlowCode,
                                             AccountCashFlow = d.MstAccount.MstAccountCashFlow.AccountCashFlow,
                                             AccountTypeCode = d.MstAccount.MstAccountType.AccountTypeCode,
                                             AccountType = d.MstAccount.MstAccountType.AccountType,
                                             AccountCode = d.MstAccount.AccountCode,
                                             Account = d.MstAccount.Account,
                                             DebitAmount = d.DebitAmount,
                                             CreditAmount = d.CreditAmount
                                         };

                    var cashFlowBalanceSheet = from d in db.TrnJournals
                                               where d.MstBranch.CompanyId == CompanyId
                                               && d.JournalDate >= Convert.ToDateTime(StartDate)
                                               && d.JournalDate <= Convert.ToDateTime(EndDate)
                                               && d.MstAccount.MstAccountType.AccountCategoryId < 5
                                               && (d.MstAccount.AccountCashFlowId == null ? 4 : d.MstAccount.AccountCashFlowId) <= 3
                                               select new Models.TrnJournal
                                               {
                                                   AccountCashFlowCode = d.MstAccount.MstAccountCashFlow.AccountCashFlowCode,
                                                   AccountCashFlow = d.MstAccount.MstAccountCashFlow.AccountCashFlow,
                                                   AccountTypeCode = d.MstAccount.MstAccountType.AccountTypeCode,
                                                   AccountType = d.MstAccount.MstAccountType.AccountType,
                                                   AccountCode = d.MstAccount.AccountCode,
                                                   Account = d.MstAccount.Account,
                                                   DebitAmount = d.DebitAmount,
                                                   CreditAmount = d.CreditAmount
                                               };


                    var accountCashFlowGroupFromCashFlowIncome = from d in cashFlowIncome
                                                                 group d by new
                                                                 {
                                                                     AccountCashFlowCode = d.AccountCashFlowCode,
                                                                     AccountCashFlow = d.AccountCashFlow
                                                                 } into g
                                                                 select new Models.TrnJournal
                                                                 {
                                                                     AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                     AccountCashFlow = g.Key.AccountCashFlow
                                                                 };

                    var accountCashFlowGroupFromCashFlowBalanceSheet = from d in cashFlowBalanceSheet
                                                                       group d by new
                                                                       {
                                                                           AccountCashFlowCode = d.AccountCashFlowCode,
                                                                           AccountCashFlow = d.AccountCashFlow
                                                                       } into g
                                                                       select new Models.TrnJournal
                                                                       {
                                                                           AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                           AccountCashFlow = g.Key.AccountCashFlow
                                                                       };

                    var unionAccountCashFlowGroups = accountCashFlowGroupFromCashFlowIncome.Union(accountCashFlowGroupFromCashFlowBalanceSheet).OrderBy(d => d.AccountCashFlowCode);

                    Decimal totalBalanceAmountOfAllBranches = 0;
                    foreach (var unionAccountCashFlowGroup in unionAccountCashFlowGroups)
                    {
                        PdfPTable tableCashFlow = new PdfPTable(3);
                        float[] widthCellstableTableCashFlow = new float[] { 50f, 150f, 30f };
                        tableCashFlow.SetWidths(widthCellstableTableCashFlow);
                        tableCashFlow.WidthPercentage = 100;

                        document.Add(Chunk.NEWLINE);
                        document.Add(line);

                        PdfPCell cashFlowColspan = (new PdfPCell(new Phrase(unionAccountCashFlowGroup.AccountCashFlow, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 6f, PaddingLeft = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        cashFlowColspan.Colspan = 3;
                        tableCashFlow.AddCell(cashFlowColspan);

                        var accountTypeGroupFromCashFlowIncome = from d in cashFlowIncome
                                                                 where d.AccountCashFlow == unionAccountCashFlowGroup.AccountCashFlow
                                                                 group d by new
                                                                 {
                                                                     AccountCashFlowCode = d.AccountCashFlowCode,
                                                                     AccountCashFlow = d.AccountCashFlow,
                                                                     AccountTypeCode = incomeAccount.FirstOrDefault().MstAccountType.AccountTypeCode,
                                                                     AccountType = incomeAccount.FirstOrDefault().MstAccountType.AccountType,
                                                                     AccountCode = "0000",
                                                                     Account = incomeAccount.FirstOrDefault().Account,
                                                                 } into g
                                                                 select new Models.TrnJournal
                                                                 {
                                                                     AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                     AccountCashFlow = g.Key.AccountCashFlow,
                                                                     AccountTypeCode = g.Key.AccountTypeCode,
                                                                     AccountType = g.Key.AccountType,
                                                                     AccountCode = g.Key.AccountCode,
                                                                     Account = g.Key.Account,
                                                                     DebitAmount = g.Sum(d => d.DebitAmount),
                                                                     CreditAmount = g.Sum(d => d.CreditAmount),
                                                                     Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                 };

                        var accountTypeGroupFromCashFlowBalanceSheet = from d in cashFlowBalanceSheet
                                                                       where d.AccountCashFlow == unionAccountCashFlowGroup.AccountCashFlow
                                                                       group d by new
                                                                       {
                                                                           AccountCashFlowCode = d.AccountCashFlowCode,
                                                                           AccountCashFlow = d.AccountCashFlow,
                                                                           AccountTypeCode = d.AccountTypeCode,
                                                                           AccountType = d.AccountType,
                                                                           AccountCode = d.AccountCode,
                                                                           Account = d.Account
                                                                       } into g
                                                                       select new Models.TrnJournal
                                                                       {
                                                                           AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                           AccountCashFlow = g.Key.AccountCashFlow,
                                                                           AccountTypeCode = g.Key.AccountTypeCode,
                                                                           AccountType = g.Key.AccountType,
                                                                           AccountCode = g.Key.AccountCode,
                                                                           Account = g.Key.Account,
                                                                           DebitAmount = g.Sum(d => d.DebitAmount),
                                                                           CreditAmount = g.Sum(d => d.CreditAmount),
                                                                           Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                       };

                        var unionAccountTypeGroups = accountTypeGroupFromCashFlowIncome.Union(accountTypeGroupFromCashFlowBalanceSheet).OrderBy(d => d.AccountCode);

                        Decimal totalBalanceAmount = 0;
                        foreach (var unionAccountTypeGroup in unionAccountTypeGroups)
                        {

                            var accountGroupFromCashFlowIncome = from d in accountTypeGroupFromCashFlowIncome
                                                                 where d.AccountCode == unionAccountTypeGroup.AccountCode
                                                                 group d by new
                                                                 {
                                                                     AccountCashFlowCode = d.AccountCashFlowCode,
                                                                     AccountCashFlow = d.AccountCashFlow,
                                                                     AccountTypeCode = incomeAccount.FirstOrDefault().MstAccountType.AccountTypeCode,
                                                                     AccountType = incomeAccount.FirstOrDefault().MstAccountType.AccountType,
                                                                     AccountCode = "0000",
                                                                     Account = incomeAccount.FirstOrDefault().Account,
                                                                 } into g
                                                                 select new Models.TrnJournal
                                                                 {
                                                                     AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                     AccountCashFlow = g.Key.AccountCashFlow,
                                                                     AccountTypeCode = g.Key.AccountTypeCode,
                                                                     AccountType = g.Key.AccountType,
                                                                     AccountCode = g.Key.AccountCode,
                                                                     Account = g.Key.Account,
                                                                     DebitAmount = g.Sum(d => d.DebitAmount),
                                                                     CreditAmount = g.Sum(d => d.CreditAmount),
                                                                     Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                 };

                            var accountGroupFromCashFlowBalanceSheet = from d in accountTypeGroupFromCashFlowBalanceSheet
                                                                       where d.AccountCode == unionAccountTypeGroup.AccountCode
                                                                       group d by new
                                                                       {
                                                                           AccountCashFlowCode = d.AccountCashFlowCode,
                                                                           AccountCashFlow = d.AccountCashFlow,
                                                                           AccountTypeCode = d.AccountTypeCode,
                                                                           AccountType = d.AccountType,
                                                                           AccountCode = d.AccountCode,
                                                                           Account = d.Account
                                                                       } into g
                                                                       select new Models.TrnJournal
                                                                       {
                                                                           AccountCashFlowCode = g.Key.AccountCashFlowCode,
                                                                           AccountCashFlow = g.Key.AccountCashFlow,
                                                                           AccountTypeCode = g.Key.AccountTypeCode,
                                                                           AccountType = g.Key.AccountType,
                                                                           AccountCode = g.Key.AccountCode,
                                                                           Account = g.Key.Account,
                                                                           DebitAmount = g.Sum(d => d.DebitAmount),
                                                                           CreditAmount = g.Sum(d => d.CreditAmount),
                                                                           Balance = g.Sum(d => d.CreditAmount - d.DebitAmount)
                                                                       };

                            PdfPCell accountColspan = (new PdfPCell(new Phrase(unionAccountTypeGroup.AccountType, cellBoldFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                            accountColspan.Colspan = 3;
                            tableCashFlow.AddCell(accountColspan);

                            var unionAccountGroups = accountGroupFromCashFlowIncome.Union(accountGroupFromCashFlowBalanceSheet).OrderBy(d => d.AccountCode);
                            foreach (var unionAccountGroup in unionAccountGroups)
                            {
                                tableCashFlow.AddCell(new PdfPCell(new Phrase(unionAccountGroup.AccountCode, cellFont)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                                tableCashFlow.AddCell(new PdfPCell(new Phrase(unionAccountGroup.Account, cellFont)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                                tableCashFlow.AddCell(new PdfPCell(new Phrase(unionAccountGroup.Balance.ToString("#,##0.00"), cellFont)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });

                                totalBalanceAmount = totalBalanceAmount + unionAccountGroup.Balance;
                            }
                        }

                        document.Add(tableCashFlow);

                        document.Add(line);
                        PdfPTable tableCashFlowTotal = new PdfPTable(3);
                        float[] widthCellstableTableCashFlowTotal = new float[] { 50f, 150f, 30f };
                        tableCashFlowTotal.SetWidths(widthCellstableTableCashFlowTotal);
                        tableCashFlowTotal.WidthPercentage = 100;
                        tableCashFlowTotal.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
                        tableCashFlowTotal.AddCell(new PdfPCell(new Phrase("Total " + unionAccountCashFlowGroup.AccountCashFlow, cellBoldFont)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f });
                        tableCashFlowTotal.AddCell(new PdfPCell(new Phrase(totalBalanceAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });

                        document.Add(tableCashFlowTotal);
                        document.Add(Chunk.NEWLINE);

                        totalBalanceAmountOfAllBranches = totalBalanceAmountOfAllBranches + totalBalanceAmount;
                    }

                    document.Add(line);
                    PdfPTable tableCashFlowTotalAllBranches = new PdfPTable(3);
                    float[] widthCellstableTableCashFlowTotalAllBranches = new float[] { 50f, 150f, 30f };
                    tableCashFlowTotalAllBranches.SetWidths(widthCellstableTableCashFlowTotalAllBranches);
                    tableCashFlowTotalAllBranches.WidthPercentage = 100;
                    tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase("", cellFont)) { Border = 0, HorizontalAlignment = 0, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 50f });
                    tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase("All Branches Cash Balance ", cellBoldFont)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f, PaddingLeft = 20f });
                    tableCashFlowTotalAllBranches.AddCell(new PdfPCell(new Phrase(totalBalanceAmountOfAllBranches.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, Rowspan = 2, PaddingTop = 5f, PaddingBottom = 5f });

                    document.Add(tableCashFlowTotalAllBranches);

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
        }

        [Authorize]
        public ActionResult FinancialStatementTrialBalancePDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // retrieve account category journal
            var journals = from d in db.TrnJournals
                           where d.JournalDate >= Convert.ToDateTime(StartDate)
                           && d.JournalDate <= Convert.ToDateTime(EndDate)
                           && d.MstBranch.CompanyId == CompanyId
                           group d by new
                           {
                               AccountCode = d.MstAccount.AccountCode,
                               Account = d.MstAccount.Account
                           } into g
                           select new Models.TrnJournal
                           {
                               AccountCode = g.Key.AccountCode,
                               Account = g.Key.Account,
                               DebitAmount = g.Sum(d => d.DebitAmount),
                               CreditAmount = g.Sum(d => d.CreditAmount),
                           };

            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Trial Balance", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            document.Add(line);
            PdfPTable tableHeaderDetail = new PdfPTable(4);
            PdfPCell Cell = new PdfPCell();
            float[] widthscellsheader2 = new float[] { 15f, 30f, 15f, 15f };
            tableHeaderDetail.SetWidths(widthscellsheader2);
            tableHeaderDetail.WidthPercentage = 100;
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account Code", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

            Decimal totalDebitAmount = 0;
            Decimal totalCreditAmount = 0;
            if (journals.Any())
            {
                foreach (var journal in journals)
                {
                    // retrieve account category journal
                    var journalsForBalances = from d in db.TrnJournals
                                              where d.JournalDate >= Convert.ToDateTime(StartDate)
                                              && d.JournalDate <= Convert.ToDateTime(EndDate)
                                              && d.MstBranch.CompanyId == CompanyId
                                              group d by new
                                              {
                                                  AccountCategoryId = d.MstAccount.MstAccountType.MstAccountCategory.Id,
                                                  AccountCategory = d.MstAccount.MstAccountType.MstAccountCategory.AccountCategory,
                                              } into g
                                              select new Models.TrnJournal
                                              {
                                                  AccountCategoryId = g.Key.AccountCategoryId,
                                                  AccountCategory = g.Key.AccountCategory,
                                                  DebitAmount = g.Sum(d => d.DebitAmount),
                                                  CreditAmount = g.Sum(d => d.CreditAmount)
                                              };

                    totalDebitAmount = journalsForBalances.Sum(d => d.DebitAmount);
                    totalCreditAmount = journalsForBalances.Sum(d => d.CreditAmount);

                    Decimal balance = 0;
                    foreach (var journalsForBalance in journalsForBalances)
                    {
                        if (journalsForBalance.AccountCategoryId == 1)
                        {
                            balance = journal.DebitAmount - journal.CreditAmount;
                        }
                        else
                        {
                            balance = journal.CreditAmount - journal.DebitAmount;
                        }
                    }

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, cellFont)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, cellFont)) { PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                }
            }
            document.Add(tableHeaderDetail);

            document.Add(Chunk.NEWLINE);
            document.Add(line);

            // table Balance Sheet footer in Asset CAtegory
            PdfPTable tableTrialBalanceFooter = new PdfPTable(4);
            float[] widthCellstableTrialBalanceFooter = new float[] { 15f, 30f, 15f, 15f };
            tableTrialBalanceFooter.SetWidths(widthCellstableTrialBalanceFooter);
            tableTrialBalanceFooter.WidthPercentage = 100;

            tableTrialBalanceFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTrialBalanceFooter.AddCell(new PdfPCell(new Phrase("Totals", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 3f, PaddingBottom = 5f });
            tableTrialBalanceFooter.AddCell(new PdfPCell(new Phrase(totalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableTrialBalanceFooter.AddCell(new PdfPCell(new Phrase(totalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

            document.Add(tableTrialBalanceFooter);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementAccountLedgerPDF(String StartDate, String EndDate, Int32 CompanyId, Int32 AccountId)
        {
            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Account Ledger", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            document.Add(line);

            var journalAccounts = from d in db.MstAccounts
                                  where d.Id == AccountId
                                  select new Models.TrnJournal
                                  {
                                      AccountCode = d.AccountCode,
                                      Account = d.Account
                                  };

            // table branch account
            PdfPTable tableAccountHeader = new PdfPTable(1);
            float[] widthCellsTableBAccountHeader = new float[] { 100f };
            tableAccountHeader.SetWidths(widthCellsTableBAccountHeader);
            tableAccountHeader.WidthPercentage = 100;

            foreach (var journalAccount in journalAccounts)
            {
                tableAccountHeader.AddCell(new PdfPCell(new Phrase(journalAccount.AccountCode + " - " + journalAccount.Account, columnFont)) { HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f, Border = 0 });
                document.Add(tableAccountHeader);
            }

            PdfPTable tableHeaderDetail = new PdfPTable(7);
            PdfPCell Cell = new PdfPCell();
            float[] widthscellsheader2 = new float[] { 60f, 125f, 130f, 150f, 100f, 100f, 100f };
            tableHeaderDetail.SetWidths(widthscellsheader2);
            tableHeaderDetail.WidthPercentage = 100;
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Date", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Document Reference", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Particulars", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
            tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.JournalDate, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DocumentReference, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Particulars, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

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

            tableFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableFooter.AddCell(new PdfPCell(new Phrase("Totals", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableFooter.AddCell(new PdfPCell(new Phrase(totalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableFooter.AddCell(new PdfPCell(new Phrase(totalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableFooter.AddCell(new PdfPCell(new Phrase(totalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            document.Add(tableFooter);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementReceivingReceiptBookPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Receiving Receipt Book", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            document.Add(line);

            var journalsDocumentReferences = from d in db.TrnJournals
                                             where d.JournalDate >= Convert.ToDateTime(StartDate)
                                             && d.JournalDate <= Convert.ToDateTime(EndDate)
                                             && d.MstBranch.CompanyId == CompanyId
                                             && d.RRId != null
                                             group d by new
                                             {
                                                 DocumentReference = d.DocumentReference
                                             } into g
                                             select new Models.TrnJournal
                                             {
                                                 DocumentReference = g.Key.DocumentReference
                                             };

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

                tableDocRefHeader.AddCell(new PdfPCell(new Phrase(journalsDocumentReference.DocumentReference, columnFont)) { HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f, Border = 0 });
                document.Add(tableDocRefHeader);

                PdfPTable tableHeaderDetail = new PdfPTable(6);
                PdfPCell Cell = new PdfPCell();
                float[] widthscellsheader2 = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                tableHeaderDetail.SetWidths(widthscellsheader2);
                tableHeaderDetail.WidthPercentage = 100;
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

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

                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

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

            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            document.Add(tableTotalFooter);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementDisbursementBookPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Disbursement Book", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

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

                tableDocRefHeader.AddCell(new PdfPCell(new Phrase(journalsDocumentReference.DocumentReference, columnFont)) { HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f, Border = 0 });
                document.Add(tableDocRefHeader);

                PdfPTable tableHeaderDetail = new PdfPTable(6);
                PdfPCell Cell = new PdfPCell();
                float[] widthscellsheader2 = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                tableHeaderDetail.SetWidths(widthscellsheader2);
                tableHeaderDetail.WidthPercentage = 100;
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

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

                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

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

            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            document.Add(tableTotalFooter);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementSalesBookPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Sales Book", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            document.Add(line);

            var journalsDocumentReferences = from d in db.TrnJournals
                                             where d.JournalDate >= Convert.ToDateTime(StartDate)
                                             && d.JournalDate <= Convert.ToDateTime(EndDate)
                                             && d.MstBranch.CompanyId == CompanyId
                                             && d.SIId != null
                                             group d by new
                                             {
                                                 DocumentReference = d.DocumentReference
                                             } into g
                                             select new Models.TrnJournal
                                             {
                                                 DocumentReference = g.Key.DocumentReference
                                             };

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

                tableDocRefHeader.AddCell(new PdfPCell(new Phrase(journalsDocumentReference.DocumentReference, columnFont)) { HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f, Border = 0 });
                document.Add(tableDocRefHeader);

                PdfPTable tableHeaderDetail = new PdfPTable(6);
                PdfPCell Cell = new PdfPCell();
                float[] widthscellsheader2 = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                tableHeaderDetail.SetWidths(widthscellsheader2);
                tableHeaderDetail.WidthPercentage = 100;
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

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

                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

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

            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            document.Add(tableTotalFooter);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementCollectionBookPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Collection Book", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            document.Add(line);

            var journalsDocumentReferences = from d in db.TrnJournals
                                             where d.JournalDate >= Convert.ToDateTime(StartDate)
                                             && d.JournalDate <= Convert.ToDateTime(EndDate)
                                             && d.MstBranch.CompanyId == CompanyId
                                             && d.ORId != null
                                             group d by new
                                             {
                                                 DocumentReference = d.DocumentReference
                                             } into g
                                             select new Models.TrnJournal
                                             {
                                                 DocumentReference = g.Key.DocumentReference
                                             };

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

                tableDocRefHeader.AddCell(new PdfPCell(new Phrase(journalsDocumentReference.DocumentReference, columnFont)) { HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f, Border = 0 });
                document.Add(tableDocRefHeader);

                PdfPTable tableHeaderDetail = new PdfPTable(6);
                PdfPCell Cell = new PdfPCell();
                float[] widthscellsheader2 = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                tableHeaderDetail.SetWidths(widthscellsheader2);
                tableHeaderDetail.WidthPercentage = 100;
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

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

                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

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

            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            document.Add(tableTotalFooter);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementStockInBookPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Stock In Book", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            document.Add(line);

            var journalsDocumentReferences = from d in db.TrnJournals
                                             where d.JournalDate >= Convert.ToDateTime(StartDate)
                                             && d.JournalDate <= Convert.ToDateTime(EndDate)
                                             && d.MstBranch.CompanyId == CompanyId
                                             && d.INId != null
                                             group d by new
                                             {
                                                 DocumentReference = d.DocumentReference
                                             } into g
                                             select new Models.TrnJournal
                                             {
                                                 DocumentReference = g.Key.DocumentReference
                                             };

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

                tableDocRefHeader.AddCell(new PdfPCell(new Phrase(journalsDocumentReference.DocumentReference, columnFont)) { HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f, Border = 0 });
                document.Add(tableDocRefHeader);

                PdfPTable tableHeaderDetail = new PdfPTable(6);
                PdfPCell Cell = new PdfPCell();
                float[] widthscellsheader2 = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                tableHeaderDetail.SetWidths(widthscellsheader2);
                tableHeaderDetail.WidthPercentage = 100;
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

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

                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

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

            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            document.Add(tableTotalFooter);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementStockOutBookPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Stock Out Book", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            document.Add(line);

            var journalsDocumentReferences = from d in db.TrnJournals
                                             where d.JournalDate >= Convert.ToDateTime(StartDate)
                                             && d.JournalDate <= Convert.ToDateTime(EndDate)
                                             && d.MstBranch.CompanyId == CompanyId
                                             && d.OTId != null
                                             group d by new
                                             {
                                                 DocumentReference = d.DocumentReference
                                             } into g
                                             select new Models.TrnJournal
                                             {
                                                 DocumentReference = g.Key.DocumentReference
                                             };

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

                tableDocRefHeader.AddCell(new PdfPCell(new Phrase(journalsDocumentReference.DocumentReference, columnFont)) { HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f, Border = 0 });
                document.Add(tableDocRefHeader);

                PdfPTable tableHeaderDetail = new PdfPTable(6);
                PdfPCell Cell = new PdfPCell();
                float[] widthscellsheader2 = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                tableHeaderDetail.SetWidths(widthscellsheader2);
                tableHeaderDetail.WidthPercentage = 100;
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

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

                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

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

            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            document.Add(tableTotalFooter);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementStockTransferBookPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Stock Transfer Book", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            document.Add(line);

            var journalsDocumentReferences = from d in db.TrnJournals
                                             where d.JournalDate >= Convert.ToDateTime(StartDate)
                                             && d.JournalDate <= Convert.ToDateTime(EndDate)
                                             && d.MstBranch.CompanyId == CompanyId
                                             && d.STId != null
                                             group d by new
                                             {
                                                 DocumentReference = d.DocumentReference
                                             } into g
                                             select new Models.TrnJournal
                                             {
                                                 DocumentReference = g.Key.DocumentReference
                                             };

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

                tableDocRefHeader.AddCell(new PdfPCell(new Phrase(journalsDocumentReference.DocumentReference, columnFont)) { HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f, Border = 0 });
                document.Add(tableDocRefHeader);

                PdfPTable tableHeaderDetail = new PdfPTable(6);
                PdfPCell Cell = new PdfPCell();
                float[] widthscellsheader2 = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                tableHeaderDetail.SetWidths(widthscellsheader2);
                tableHeaderDetail.WidthPercentage = 100;
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

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

                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

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

            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            document.Add(tableTotalFooter);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementJournalVoucherBookPDF(String StartDate, String EndDate, Int32 CompanyId)
        {
            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // Start of the PDF
            MemoryStream workStream = new MemoryStream();
            Rectangle rec = new Rectangle(PageSize.A3);
            Document document = new Document(rec, 72, 72, 72, 72);
            document.SetMargins(50f, 50f, 50f, 50f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            // Document Starts
            document.Open();

            // Fonts Customization
            Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font headerDetailFont = FontFactory.GetFont("Arial", 11);
            Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font cellFont = FontFactory.GetFont("Arial", 10);
            Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // table main header
            PdfPTable tableHeader = new PdfPTable(2);
            float[] widthscellsheader = new float[] { 100f, 75f };
            tableHeader.SetWidths(widthscellsheader);
            tableHeader.WidthPercentage = 100;
            tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
            tableHeader.AddCell(new PdfPCell(new Phrase("Journal Voucher Book", headerFont)) { Border = 0, HorizontalAlignment = 2 });
            tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Date from " + StartDate + " to " + EndDate, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
            tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
            document.Add(tableHeader);

            document.Add(line);

            var journalsDocumentReferences = from d in db.TrnJournals
                                             where d.JournalDate >= Convert.ToDateTime(StartDate)
                                             && d.JournalDate <= Convert.ToDateTime(EndDate)
                                             && d.MstBranch.CompanyId == CompanyId
                                             && d.JVId != null
                                             group d by new
                                             {
                                                 DocumentReference = d.DocumentReference
                                             } into g
                                             select new Models.TrnJournal
                                             {
                                                 DocumentReference = g.Key.DocumentReference
                                             };

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

                tableDocRefHeader.AddCell(new PdfPCell(new Phrase(journalsDocumentReference.DocumentReference, columnFont)) { HorizontalAlignment = 0, PaddingTop = 6f, PaddingBottom = 9f, Border = 0 });
                document.Add(tableDocRefHeader);

                PdfPTable tableHeaderDetail = new PdfPTable(6);
                PdfPCell Cell = new PdfPCell();
                float[] widthscellsheader2 = new float[] { 80f, 130f, 150f, 100f, 100f, 100f };
                tableHeaderDetail.SetWidths(widthscellsheader2);
                tableHeaderDetail.WidthPercentage = 100;
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Code", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Account", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Article", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Debit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Credit", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableHeaderDetail.AddCell(new PdfPCell(new Phrase("Balance", columnFont)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.AccountCode, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Account, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.Article, cellFont)) { HorizontalAlignment = 1, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.DebitAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(journal.CreditAmount.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableHeaderDetail.AddCell(new PdfPCell(new Phrase(balance.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

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

                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
                tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(subTotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

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

            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("", cellBoldFont)) { Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase("Total", cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 25f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalDebitAmount.ToString("#,##0.00"), cellBoldFont)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f, PaddingBottom = 5f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalCreditAmount.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, PaddingLeft = 50f });
            tableTotalFooter.AddCell(new PdfPCell(new Phrase(TotalBalance.ToString("#,##0.00"), cellBoldFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

            document.Add(tableTotalFooter);
            document.Add(Chunk.NEWLINE);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }

        [Authorize]
        public ActionResult FinancialStatementChartOfAccountsBookPDF()
        {
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 10);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Chart of Accounts", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("", headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f, PaddingBottom = 18f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    document.Add(line);

                    var accountCategories = from d in db.MstAccountCategories
                                            group d by new
                                            {
                                                AccountCategoryCode = d.AccountCategoryCode,
                                                AccountCategory = d.AccountCategory
                                            } into g
                                            select new Models.MstAccountCategory
                                            {
                                                AccountCategoryCode = g.Key.AccountCategoryCode,
                                                AccountCategory = g.Key.AccountCategory
                                            };

                    foreach (var accountCategory in accountCategories)
                    {

                        PdfPTable tableAccountCategory = new PdfPTable(1);
                        float[] widthscellsTableAccountCategory = new float[] { 100f };
                        tableAccountCategory.SetWidths(widthscellsTableAccountCategory);
                        tableAccountCategory.WidthPercentage = 100;

                        tableAccountCategory.AddCell(new PdfPCell(new Phrase(accountCategory.AccountCategory, cellFont2)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, Border = 0, BackgroundColor = BaseColor.LIGHT_GRAY });
                        document.Add(tableAccountCategory);

                        var accountSubCategories = from d in db.MstAccountTypes
                                                   where d.MstAccountCategory.AccountCategory == accountCategory.AccountCategory
                                                   group d by new
                                                   {
                                                       SubCategoryDescription = d.SubCategoryDescription
                                                   } into g
                                                   select new Models.MstAccountType
                                                   {
                                                       SubCategoryDescription = g.Key.SubCategoryDescription
                                                   };

                        foreach (var accountSubCategory in accountSubCategories)
                        {
                            PdfPTable tableSubAccountCategory = new PdfPTable(1);
                            float[] widthscellsTableSubAccountCategory = new float[] { 100f };
                            tableSubAccountCategory.SetWidths(widthscellsTableSubAccountCategory);
                            tableSubAccountCategory.WidthPercentage = 100;

                            tableSubAccountCategory.AddCell(new PdfPCell(new Phrase(accountSubCategory.SubCategoryDescription, cellFont2)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 20f, Border = 0 });
                            document.Add(tableSubAccountCategory);

                            var accountTypes = from d in db.MstAccountTypes
                                               where d.SubCategoryDescription == accountSubCategory.SubCategoryDescription
                                               group d by new
                                               {
                                                   AccountType = d.AccountType
                                               } into g
                                               select new Models.MstAccountType
                                               {
                                                   AccountType = g.Key.AccountType
                                               };

                            foreach (var accountType in accountTypes)
                            {
                                PdfPTable tableAccountType = new PdfPTable(1);
                                float[] widthscellsTableAccountType = new float[] { 100f };
                                tableAccountType.SetWidths(widthscellsTableAccountType);
                                tableAccountType.WidthPercentage = 100;

                                tableAccountType.AddCell(new PdfPCell(new Phrase(accountType.AccountType, cellFont2)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 40f, Border = 0 });
                                document.Add(tableAccountType);

                                var accounts = from d in db.MstAccounts
                                               where d.MstAccountType.AccountType == accountType.AccountType
                                               select new Models.MstAccount
                                               {
                                                   AccountCode = d.AccountCode,
                                                   Account = d.Account
                                               };

                                foreach (var account in accounts)
                                {
                                    PdfPTable tableAccount = new PdfPTable(2);
                                    float[] widthscellsTableAccount = new float[] { 20f, 100f };
                                    tableAccount.SetWidths(widthscellsTableAccount);
                                    tableAccount.WidthPercentage = 100;

                                    tableAccount.AddCell(new PdfPCell(new Phrase(account.AccountCode, cellFont2)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 60f, Border = 0 });
                                    tableAccount.AddCell(new PdfPCell(new Phrase(account.Account, cellFont2)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 10f, Border = 0 });
                                    document.Add(tableAccount);

                                }
                            }

                        }
                    }

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
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
        public ActionResult StockCount()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockCountDetail()
        {
            return View();
        }

        [Authorize]
        public ActionResult StockCountPDF(Int32 Id)
        {
            HttpCookieCollection cookieCollection = Request.Cookies;
            HttpCookie branchIdCookie = cookieCollection.Get("branchId");

            if (branchIdCookie != null)
            {
                var branchId = Convert.ToInt32(Request.Cookies["branchId"].Value);
                var branches = from d in db.MstBranches where d.Id == branchId select d;

                if (branches.Any())
                {
                    // Company Detail
                    var companyName = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Company).SingleOrDefault();
                    var address = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.Address).SingleOrDefault();
                    var contactNo = (from d in db.MstBranches where d.Id == branchId select d.MstCompany.ContactNumber).SingleOrDefault();
                    var branch = (from d in db.MstBranches where d.Id == branchId select d.Branch).SingleOrDefault();

                    // Start of the PDF
                    MemoryStream workStream = new MemoryStream();
                    Rectangle rec = new Rectangle(PageSize.A3);
                    Document document = new Document(rec, 72, 72, 72, 72);
                    document.SetMargins(50f, 50f, 50f, 50f);
                    PdfWriter.GetInstance(document, workStream).CloseStream = false;

                    // Document Starts
                    document.Open();

                    // Fonts Customization
                    Font headerFont = FontFactory.GetFont("Arial", 17, Font.BOLD);
                    Font headerDetailFont = FontFactory.GetFont("Arial", 11);
                    Font columnFont = FontFactory.GetFont("Arial", 11, Font.BOLD);
                    Font cellFont = FontFactory.GetFont("Arial", 9);
                    Font cellFont2 = FontFactory.GetFont("Arial", 11);
                    Font cellBoldFont = FontFactory.GetFont("Arial", 10, Font.BOLD);

                    PdfPTable tableHeader = new PdfPTable(2);
                    float[] widthscellsheader = new float[] { 100f, 75f };
                    tableHeader.SetWidths(widthscellsheader);
                    tableHeader.WidthPercentage = 100;
                    tableHeader.AddCell(new PdfPCell(new Phrase(companyName, headerFont)) { Border = 0 });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Stock Count", headerFont)) { Border = 0, HorizontalAlignment = 2 });
                    tableHeader.AddCell(new PdfPCell(new Phrase(address, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(branch, headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase(contactNo, headerDetailFont)) { Border = 0, PaddingTop = 5f });
                    tableHeader.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), headerDetailFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 5f });

                    document.Add(tableHeader);

                    Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
                    document.Add(line);

                    var stockCounts = from d in db.TrnStockCounts
                                      where d.Id == Id
                                      select new Models.TrnStockCount
                                      {
                                          Id = d.Id,
                                          BranchId = d.BranchId,
                                          Branch = d.MstBranch.Branch,
                                          SCNumber = d.SCNumber,
                                          SCDate = d.SCDate.ToShortDateString(),
                                          Particulars = d.Particulars,
                                          PreparedBy = d.MstUser3.FullName,
                                          PreparedById = d.PreparedById,
                                          CheckedBy = d.MstUser1.FullName,
                                          CheckedById = d.CheckedById,
                                          ApprovedBy = d.MstUser.FullName,
                                          ApprovedById = d.ApprovedById,
                                          IsLocked = d.IsLocked,
                                          CreatedById = d.CreatedById,
                                          CreatedBy = d.MstUser2.FullName,
                                          CreatedDateTime = d.CreatedDateTime.ToShortDateString(),
                                          UpdatedById = d.UpdatedById,
                                          UpdatedBy = d.MstUser4.FullName,
                                          UpdatedDateTime = d.UpdatedDateTime.ToShortDateString()
                                      };

                    String Particulars = "";
                    String SCNumber = "", SCDate = "";
                    String PreparedBy = "", CheckedBy = "", ApprovedBy = "";

                    foreach (var stockCount in stockCounts)
                    {
                        Particulars = stockCount.Particulars;
                        SCNumber = stockCount.SCNumber;
                        SCDate = stockCount.SCDate;
                        PreparedBy = stockCount.PreparedBy;
                        CheckedBy = stockCount.CheckedBy;
                        ApprovedBy = stockCount.ApprovedBy;
                    }

                    PdfPTable tableSubHeader = new PdfPTable(4);
                    float[] widthscellsSubheader = new float[] { 40f, 100f, 100f, 40f };
                    tableSubHeader.SetWidths(widthscellsSubheader);
                    tableSubHeader.WidthPercentage = 100;
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("Particulars: ", columnFont)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(Particulars, cellFont2)) { Border = 0, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("SC Number: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(SCNumber, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", columnFont)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("", cellFont2)) { Border = 0, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase("SC Date: ", columnFont)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    tableSubHeader.AddCell(new PdfPCell(new Phrase(SCDate, cellFont2)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f });
                    document.Add(tableSubHeader);

                    document.Add(Chunk.NEWLINE);

                    var stockCountItems = from d in db.TrnStockCountItems
                                          where d.SCId == Id
                                          select new Models.TrnStockCountItem
                                          {
                                              Id = d.Id,
                                              SCId = d.SCId,
                                              ItemId = d.ItemId,
                                              Item = d.MstArticle.Article,
                                              ItemCode = d.MstArticle.ManualArticleCode,
                                              Particulars = d.Particulars,
                                              Quantity = d.Quantity,
                                              Unit = d.MstArticle.MstUnit.Unit
                                          };

                    PdfPTable tableSCLines = new PdfPTable(4);
                    float[] widthscellsSCLines = new float[] { 50f, 60f, 100f, 350f };
                    tableSCLines.SetWidths(widthscellsSCLines);
                    tableSCLines.WidthPercentage = 100;
                    tableSCLines.AddCell(new PdfPCell(new Phrase("Quantity", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSCLines.AddCell(new PdfPCell(new Phrase("Unit", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSCLines.AddCell(new PdfPCell(new Phrase("Code", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSCLines.AddCell(new PdfPCell(new Phrase("Item", cellBoldFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    foreach (var stockCountItem in stockCountItems)
                    {
                        tableSCLines.AddCell(new PdfPCell(new Phrase(stockCountItem.Quantity.ToString("#,##0.00"), cellFont)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSCLines.AddCell(new PdfPCell(new Phrase(stockCountItem.Unit, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSCLines.AddCell(new PdfPCell(new Phrase(stockCountItem.ItemCode, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSCLines.AddCell(new PdfPCell(new Phrase(stockCountItem.Item, cellFont)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                    }

                    document.Add(tableSCLines);

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

                    tableFooter.AddCell(new PdfPCell(new Phrase("Prepared by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Checked by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Approved by:", columnFont)) { Border = 0, HorizontalAlignment = 0 });

                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingTop = 10f, PaddingBottom = 10f });

                    tableFooter.AddCell(new PdfPCell(new Phrase(PreparedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(CheckedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0, PaddingBottom = 5f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(ApprovedBy)) { Border = 1, HorizontalAlignment = 1, PaddingBottom = 5f });

                    document.Add(tableFooter);

                    // Document End
                    document.Close();

                    // Document End
                    document.Close();

                    byte[] byteInfo = workStream.ToArray();
                    workStream.Write(byteInfo, 0, byteInfo.Length);
                    workStream.Position = 0;

                    return new FileStreamResult(workStream, "application/pdf");
                }
                else
                {
                    return RedirectToAction("Index", "Manage");
                }
            }
            else
            {
                return RedirectToAction("Index", "Manage");
            }
        }
    }
}