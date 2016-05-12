using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Controllers
{
    public class RepReceivingReceiptDetailReportController : Controller
    {
         // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Receiving Receipt Detail Report
        [Authorize]
        public ActionResult ReceivingReceiptDetailReport(String StartDate, String EndDate)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Receiving Receipt Detail Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);
            
            // receiving receipt Items
            var receivingReceiptItems = from d in db.TrnReceivingReceiptItems
                                        where d.TrnReceivingReceipt.BranchId == currentBranchId()
                                        && d.TrnReceivingReceipt.RRDate >= Convert.ToDateTime(StartDate)
                                        && d.TrnReceivingReceipt.RRDate <= Convert.ToDateTime(EndDate)
                                        && d.TrnReceivingReceipt.IsLocked == true
                                        select new Models.TrnReceivingReceiptItem
                                        {
                                            Id = d.Id,
                                            RRDate = d.TrnReceivingReceipt.RRDate.ToShortDateString(),
                                            RR = d.TrnReceivingReceipt.RRNumber,
                                            PO = d.TrnPurchaseOrder.PONumber,
                                            Item = d.MstArticle.Article,
                                            Price = d.MstArticle.Price,
                                            Unit = d.MstUnit.Unit,
                                            Quantity = d.Quantity,
                                            Cost = d.Cost,
                                            Amount = d.Amount,
                                            Branch = d.MstBranch.Branch,
                                        };

            Decimal total = 0;

            if (receivingReceiptItems.Any())
            {
                // table branch header
                PdfPTable tableBranchHeader = new PdfPTable(1);
                float[] widthCellsTableBranchHeader = new float[] { 100f };
                tableBranchHeader.SetWidths(widthCellsTableBranchHeader);
                tableBranchHeader.WidthPercentage = 100;
                PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                tableBranchHeader.AddCell(branchHeaderColspan);
                document.Add(tableBranchHeader);

                // Table RR Item Data
                PdfPTable tableRRItemData = new PdfPTable(9);
                float[] widthsCellsTableRRItemData = new float[] { 19f, 15f, 19f, 30f, 20f, 20f, 20f, 20f, 20f };
                tableRRItemData.SetWidths(widthsCellsTableRRItemData);
                tableRRItemData.WidthPercentage = 100;
                tableRRItemData.AddCell(new PdfPCell(new Phrase("RR Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRItemData.AddCell(new PdfPCell(new Phrase("RR Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRItemData.AddCell(new PdfPCell(new Phrase("PO Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRItemData.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRItemData.AddCell(new PdfPCell(new Phrase("Price", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRItemData.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRItemData.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRItemData.AddCell(new PdfPCell(new Phrase("Branch", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableRRItemData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                foreach (var receivingReceiptItem in receivingReceiptItems)
                {
                    tableRRItemData.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.RR, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRItemData.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.RRDate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRItemData.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.PO, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRItemData.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Item, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRItemData.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Price.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRItemData.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Unit, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRItemData.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Quantity.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRItemData.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Branch, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableRRItemData.AddCell(new PdfPCell(new Phrase(receivingReceiptItem.Amount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    
                    total = total + receivingReceiptItem.Amount;
                }

                document.Add(tableRRItemData);
                document.Add(Chunk.NEWLINE);

                // Table RR Item Total Footer
                PdfPTable tableRRItemFooter = new PdfPTable(9);
                float[] widthscellsTableRRItemFooter = new float[] { 19f, 15f, 19f, 30f, 20f, 20f, 20f, 20f, 20f };
                tableRRItemFooter.SetWidths(widthscellsTableRRItemFooter);
                tableRRItemFooter.WidthPercentage = 100;
                tableRRItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRItemFooter.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableRRItemFooter.AddCell(new PdfPCell(new Phrase(total.ToString("#,##0.00"), fontArial11Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                document.Add(tableRRItemFooter);
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