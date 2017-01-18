using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class RepPurchaseSummaryReportController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Purchase Summary Report
        [Authorize]
        public ActionResult PurchaseSummaryReport(String StartDate, String EndDate)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Purchase Summary Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // purchase orders
            var purchaseOrders = from d in db.TrnPurchaseOrders
                                 where d.BranchId == currentBranchId()
                                 && d.PODate >= Convert.ToDateTime(StartDate)
                                 && d.PODate <= Convert.ToDateTime(EndDate)
                                 && d.IsLocked == true
                                 select new Models.TrnPurchaseOrder
                                 {
                                     Id = d.Id,
                                     Branch = d.MstBranch.Branch,
                                     PONumber = d.PONumber,
                                     PODate = d.PODate.ToShortDateString(),
                                     Supplier = d.MstArticle.Article,
                                     Remarks = d.Remarks,
                                     IsClose = d.IsClose,
                                 };

            Decimal total = 0;

            if (purchaseOrders.Any())
            {
                // table branch header
                PdfPTable tableBranchHeader = new PdfPTable(1);
                float[] widthCellsTableBranchHeader = new float[] { 100f };
                tableBranchHeader.SetWidths(widthCellsTableBranchHeader);
                tableBranchHeader.WidthPercentage = 100;
                PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                tableBranchHeader.AddCell(branchHeaderColspan);
                document.Add(tableBranchHeader);

                // table PO Data
                PdfPTable tablePOData = new PdfPTable(7);
                float[] widthsCellsTablePOData = new float[] { 25f, 10f, 15f, 25f, 35f, 10f, 20f };
                tablePOData.SetWidths(widthsCellsTablePOData);
                tablePOData.WidthPercentage = 100;
                tablePOData.AddCell(new PdfPCell(new Phrase("Branch", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOData.AddCell(new PdfPCell(new Phrase("PO Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOData.AddCell(new PdfPCell(new Phrase("PO Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOData.AddCell(new PdfPCell(new Phrase("Supplier", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOData.AddCell(new PdfPCell(new Phrase("Remarks", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOData.AddCell(new PdfPCell(new Phrase("Status", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tablePOData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                foreach (var purchaseOrder in purchaseOrders)
                {
                    tablePOData.AddCell(new PdfPCell(new Phrase(purchaseOrder.Branch, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOData.AddCell(new PdfPCell(new Phrase(purchaseOrder.PODate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOData.AddCell(new PdfPCell(new Phrase(purchaseOrder.PONumber, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOData.AddCell(new PdfPCell(new Phrase(purchaseOrder.Supplier, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tablePOData.AddCell(new PdfPCell(new Phrase(purchaseOrder.Remarks, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });

                    String POStatus = "";
                    if (purchaseOrder.IsClose == true)
                    {
                        POStatus = "Closed";
                    }
                    else
                    {
                        POStatus = " ";
                    }

                    tablePOData.AddCell(new PdfPCell(new Phrase(POStatus, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });

                    // Purchase order items total amount
                    var purchaseOrderItemTotalAmount = from d in db.TrnPurchaseOrderItems
                                                       where d.POId == purchaseOrder.Id
                                                       select new Models.TrnPurchaseOrderItem
                                                       {
                                                           Id = d.Id,
                                                           Amount = d.Amount
                                                       };
                    Decimal totalAmount = 0;
                    if (purchaseOrderItemTotalAmount.Any())
                    {
                        totalAmount = purchaseOrderItemTotalAmount.Sum(d => d.Amount);
                    }

                    tablePOData.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    total = total + totalAmount;
                }

                document.Add(tablePOData);
                document.Add(Chunk.NEWLINE);

                // table PO Footer Total
                PdfPTable tablePOFooter = new PdfPTable(7);
                float[] widthscellsTablePOFooter = new float[] { 25f, 10f, 15f, 25f, 35f, 10f, 20f };
                tablePOFooter.SetWidths(widthscellsTablePOFooter);
                tablePOFooter.WidthPercentage = 100;
                tablePOFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOFooter.AddCell(new PdfPCell(new Phrase("Total", fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tablePOFooter.AddCell(new PdfPCell(new Phrase(total.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                document.Add(tablePOFooter);
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