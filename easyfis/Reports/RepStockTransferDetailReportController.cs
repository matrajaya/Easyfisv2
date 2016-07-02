using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepStockTransferDetailReportController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Stock Transfer Detail Report
        [Authorize]
        public ActionResult StockTransferDetailReport(String StartDate, String EndDate, Int32 BranchId)
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
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);

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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Stock Transfer Detail Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // stockTransfers
            var stockTransfers = from d in db.TrnStockTransfers
                                 where d.BranchId == BranchId
                                 && d.STDate >= Convert.ToDateTime(StartDate)
                                 && d.STDate <= Convert.ToDateTime(EndDate)
                                 && d.IsLocked == true
                                 group d by new
                                 {
                                     ToBranchId = d.ToBranchId,
                                     ToBranch = d.MstBranch1.Branch,
                                 } into g
                                 select new Models.TrnStockTransfer
                                 {
                                     ToBranchId = g.Key.ToBranchId,
                                     ToBranch = g.Key.ToBranch,
                                 };

            Decimal Subtotal = 0;
            Decimal Total = 0;
            if (stockTransfers.Any())
            {
                // table branch header
                PdfPTable tableBranchHeader = new PdfPTable(1);
                float[] widthCellsTableBranchHeader = new float[] { 100f };
                tableBranchHeader.SetWidths(widthCellsTableBranchHeader);
                tableBranchHeader.WidthPercentage = 100;

                PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, Border = 0 });
                tableBranchHeader.AddCell(branchHeaderColspan);
                document.Add(tableBranchHeader);

                foreach (var stockTransfer in stockTransfers)
                {
                    PdfPTable tableSTItems = new PdfPTable(7);
                    float[] widthscellsTableSTtems = new float[] { 15f, 10f, 35f, 20f, 25f, 20f, 20f };
                    tableSTItems.SetWidths(widthscellsTableSTtems);
                    tableSTItems.WidthPercentage = 100;

                    PdfPCell stockTransferItemColSpan = (new PdfPCell(new Phrase("To Branch: " + stockTransfer.ToBranch, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 9f, PaddingBottom = 10f });
                    stockTransferItemColSpan.Colspan = 7;
                    tableSTItems.AddCell(stockTransferItemColSpan);

                    tableSTItems.AddCell(new PdfPCell(new Phrase("ST Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTItems.AddCell(new PdfPCell(new Phrase("ST Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTItems.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTItems.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTItems.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTItems.AddCell(new PdfPCell(new Phrase("Cost", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableSTItems.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    var stockTransferItems = from d in db.TrnStockTransferItems
                                             where d.TrnStockTransfer.MstBranch1.Branch == stockTransfer.ToBranch
                                             && d.TrnStockTransfer.IsLocked == true
                                             select new Models.TrnStockTransferItem
                                             {
                                                 Id = d.Id,
                                                 STId = d.STId,
                                                 ST = d.TrnStockTransfer.STNumber,
                                                 STDate = d.TrnStockTransfer.STDate.ToShortDateString(),
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
                                                 BaseCost = d.BaseCost,
                                                 ToBranch = d.TrnStockTransfer.MstBranch1.Branch
                                             };

                    foreach (var stockTransferItem in stockTransferItems)
                    {
                        tableSTItems.AddCell(new PdfPCell(new Phrase(stockTransferItem.ST, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTItems.AddCell(new PdfPCell(new Phrase(stockTransferItem.STDate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTItems.AddCell(new PdfPCell(new Phrase(stockTransferItem.Item, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTItems.AddCell(new PdfPCell(new Phrase(stockTransferItem.Quantity.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTItems.AddCell(new PdfPCell(new Phrase(stockTransferItem.Unit, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTItems.AddCell(new PdfPCell(new Phrase(stockTransferItem.Cost.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableSTItems.AddCell(new PdfPCell(new Phrase(stockTransferItem.Amount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        Subtotal = Subtotal + stockTransferItem.Amount;
                    }

                    PdfPTable tableSubTotalFooter = new PdfPTable(7);
                    float[] widthscellsTableSubItemFooter = new float[] { 15f, 10f, 35f, 20f, 25f, 20f, 20f };
                    tableSubTotalFooter.SetWidths(widthscellsTableSubItemFooter);
                    tableSubTotalFooter.WidthPercentage = 100;
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f, Border = 0 });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f, Border = 0 });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f, Border = 0 });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f, Border = 0 });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f, Border = 0 });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase("Sub Total", fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, Border = 0 });
                    tableSubTotalFooter.AddCell(new PdfPCell(new Phrase(Subtotal.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f, Border = 0 });

                    document.Add(tableSTItems);
                    document.Add(tableSubTotalFooter);

                    Total = Total + Subtotal;
                }

                document.Add(Chunk.NEWLINE);

                PdfPTable tableSTItemFooter = new PdfPTable(7);
                float[] widthscellsTableSTItemFooter = new float[] { 15f, 10f, 35f, 20f, 25f, 20f, 20f };
                tableSTItemFooter.SetWidths(widthscellsTableSTItemFooter);
                tableSTItemFooter.WidthPercentage = 100;
                tableSTItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableSTItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableSTItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableSTItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableSTItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableSTItemFooter.AddCell(new PdfPCell(new Phrase("Total", fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableSTItemFooter.AddCell(new PdfPCell(new Phrase(Total.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });

                document.Add(line);
                document.Add(tableSTItemFooter);
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