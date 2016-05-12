using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace easyfis.Controllers
{
    public class RepPurchaseDetailReportController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Purchase Detail Report
        [Authorize]
        public ActionResult PurchaseDetailReport(String StartDate, String EndDate)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Purchase Detail Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // PO Items
            var PurchaseOrderItems = from d in db.TrnPurchaseOrderItems
                                     where d.TrnPurchaseOrder.BranchId == currentBranchId()
                                     && d.TrnPurchaseOrder.PODate >= Convert.ToDateTime(StartDate)
                                     && d.TrnPurchaseOrder.PODate <= Convert.ToDateTime(EndDate)
                                     && d.TrnPurchaseOrder.IsLocked == true
                                     select new Models.TrnPurchaseOrderItem
                                     {
                                         Id = d.Id,
                                         PODate = d.TrnPurchaseOrder.PODate.ToShortDateString(),
                                         PO = d.TrnPurchaseOrder.PONumber,
                                         Item = d.MstArticle.Article,
                                         Price = d.MstArticle.Price,
                                         Unit = d.MstUnit.Unit,
                                         Quantity = d.Quantity,
                                         Amount = d.Amount
                                     };

            Decimal total = 0;

            if (PurchaseOrderItems.Any())
            {
                // table branch header
                PdfPTable tableBranchHeader = new PdfPTable(1);
                float[] widthCellsTableBranchHeader = new float[] { 100f };
                tableBranchHeader.SetWidths(widthCellsTableBranchHeader);
                tableBranchHeader.WidthPercentage = 100;
                PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                tableBranchHeader.AddCell(branchHeaderColspan);
                document.Add(tableBranchHeader);

                // TTable PO Item Data
                PdfPTable tablePOItemData = new PdfPTable(7);
                float[] widthsCellsTablePOItemData = new float[] { 15f, 10f, 35f, 20f, 25f, 20f, 20f };
                tablePOItemData.SetWidths(widthsCellsTablePOItemData);
                tablePOItemData.WidthPercentage = 100;
                tablePOItemData.AddCell(new PdfPCell(new Phrase("PO Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOItemData.AddCell(new PdfPCell(new Phrase("PO Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOItemData.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOItemData.AddCell(new PdfPCell(new Phrase("Price", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOItemData.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOItemData.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOItemData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                foreach (var PurchaseOrderItem in PurchaseOrderItems)
                {
                    tablePOItemData.AddCell(new PdfPCell(new Phrase(PurchaseOrderItem.PO, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOItemData.AddCell(new PdfPCell(new Phrase(PurchaseOrderItem.PODate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOItemData.AddCell(new PdfPCell(new Phrase(PurchaseOrderItem.Item, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOItemData.AddCell(new PdfPCell(new Phrase(PurchaseOrderItem.Price.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOItemData.AddCell(new PdfPCell(new Phrase(PurchaseOrderItem.Unit, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOItemData.AddCell(new PdfPCell(new Phrase(PurchaseOrderItem.Quantity.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOItemData.AddCell(new PdfPCell(new Phrase(PurchaseOrderItem.Amount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    total = total + PurchaseOrderItem.Amount;
                }

                document.Add(tablePOItemData);
                document.Add(Chunk.NEWLINE);

                // Table PO Item Footer Total
                PdfPTable tablePOItemFooter = new PdfPTable(7);
                float[] widthscellsTablePOItemFooter = new float[] { 15f, 10f, 35f, 20f, 25f, 20f, 20f };
                tablePOItemFooter.SetWidths(widthscellsTablePOItemFooter);
                tablePOItemFooter.WidthPercentage = 100;
                tablePOItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOItemFooter.AddCell(new PdfPCell(new Phrase("Total:", fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOItemFooter.AddCell(new PdfPCell(new Phrase(total.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                document.Add(tablePOItemFooter);
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