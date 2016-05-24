using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepInventoryReportController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // PDF Inventory Report
        [Authorize]
        public ActionResult InventoryReport(String StartDate, String EndDate, Int32 CompanyId)
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

            // line
            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 1)));

            // Company Detail
            var companyName = (from d in db.MstCompanies where d.Id == CompanyId select d.Company).SingleOrDefault();
            var address = (from d in db.MstCompanies where d.Id == CompanyId select d.Address).SingleOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == CompanyId select d.ContactNumber).SingleOrDefault();

            // table main header
            PdfPTable tableHeaderPage = new PdfPTable(2);
            float[] widthsCellsheaderPage = new float[] { 100f, 75f };
            tableHeaderPage.SetWidths(widthsCellsheaderPage);
            tableHeaderPage.WidthPercentage = 100;
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Inventory Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var articleInventoriesByBranch = from d in db.MstBranches select d;

            Decimal totalAmountForFooter = 0;
            Decimal totalVarianceForFooter = 0;

            foreach (var articleInventoryByBranch in articleInventoriesByBranch)
            {

                Decimal totalAmount = 0;
                Decimal count = 0;
                Decimal quantityVariance = 0;
                Decimal varianceAmount = 0;
                Decimal totalTotalAmount = 0;
                Decimal totalVarianceAmount = 0;

                // union inventories
                var unionInventories = (from d in db.TrnInventories
                                        where d.InventoryDate < Convert.ToDateTime(StartDate)
                                        && d.MstArticleInventory.MstBranch.Id == articleInventoryByBranch.Id
                                        && d.MstArticleInventory.MstBranch.CompanyId == CompanyId
                                        && d.MstArticleInventory.MstArticle.IsInventory == true
                                        select new Models.MstArticleInventory
                                        {
                                            Id = d.Id,
                                            Document = "Beginning Balance",
                                            ArticleId = d.MstArticleInventory.ArticleId,
                                            Article = d.MstArticleInventory.MstArticle.Article,
                                            InventoryCode = d.MstArticleInventory.InventoryCode,
                                            Quantity = d.MstArticleInventory.Quantity,
                                            Cost = d.MstArticleInventory.Cost,
                                            Amount = d.MstArticleInventory.Amount,
                                            UnitId = d.MstArticleInventory.MstArticle.MstUnit.Id,
                                            Unit = d.MstArticleInventory.MstArticle.MstUnit.Unit,
                                            BegQuantity = d.Quantity,
                                            InQuantity = d.QuantityIn,
                                            OutQuantity = d.QuantityOut,
                                            EndQuantity = d.Quantity
                                        }).Union(from d in db.TrnInventories
                                                 where d.InventoryDate >= Convert.ToDateTime(StartDate)
                                                 && d.InventoryDate <= Convert.ToDateTime(EndDate)
                                                 && d.MstArticleInventory.MstBranch.Id == articleInventoryByBranch.Id
                                                 && d.MstArticleInventory.MstBranch.CompanyId == CompanyId
                                                 && d.MstArticleInventory.MstArticle.IsInventory == true
                                                 select new Models.MstArticleInventory
                                                 {
                                                     Id = d.Id,
                                                     Document = "Current",
                                                     ArticleId = d.MstArticleInventory.ArticleId,
                                                     Article = d.MstArticleInventory.MstArticle.Article,
                                                     InventoryCode = d.MstArticleInventory.InventoryCode,
                                                     Quantity = d.MstArticleInventory.Quantity,
                                                     Cost = d.MstArticleInventory.Cost,
                                                     Amount = d.MstArticleInventory.Amount,
                                                     UnitId = d.MstArticleInventory.MstArticle.MstUnit.Id,
                                                     Unit = d.MstArticleInventory.MstArticle.MstUnit.Unit,
                                                     BegQuantity = d.Quantity,
                                                     InQuantity = d.QuantityIn,
                                                     OutQuantity = d.QuantityOut,
                                                     EndQuantity = d.Quantity
                                                 });

                // inventories
                var inventories = from d in unionInventories
                                  group d by new
                                  {
                                      ArticleId = d.ArticleId,
                                      Article = d.Article,
                                      InventoryCode = d.InventoryCode,
                                      Cost = d.Cost,
                                      UnitId = d.UnitId,
                                      Unit = d.Unit
                                  } into g
                                  select new Models.MstArticleInventory
                                  {
                                      ArticleId = g.Key.ArticleId,
                                      Article = g.Key.Article,
                                      InventoryCode = g.Key.InventoryCode,
                                      Cost = g.Key.Cost,
                                      UnitId = g.Key.UnitId,
                                      Unit = g.Key.Unit,

                                      BegQuantity = g.Sum(d => d.Document == "Current" ? 0 : d.BegQuantity),
                                      InQuantity = g.Sum(d => d.Document == "Beginning Balance" ? 0 : d.InQuantity),
                                      OutQuantity = g.Sum(d => d.Document == "Beginning Balance" ? 0 : d.OutQuantity),
                                      EndQuantity = g.Sum(d => d.EndQuantity),

                                      Amount = g.Sum(d => d.Quantity * d.Cost)
                                  };

                if (inventories.Any())
                {
                    // table branch header
                    PdfPTable tableBranchHeader = new PdfPTable(1);
                    float[] widthCellsTableBranchHeader = new float[] { 100f };
                    tableBranchHeader.SetWidths(widthCellsTableBranchHeader);
                    tableBranchHeader.WidthPercentage = 100;

                    PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(articleInventoryByBranch.Branch, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                    tableBranchHeader.AddCell(branchHeaderColspan);
                    document.Add(tableBranchHeader);

                    PdfPTable tableData = new PdfPTable(12);
                    PdfPCell Cell = new PdfPCell();
                    float[] widthsCellsData = new float[] { 20f, 30f, 15f, 16f, 16f, 16f, 16f, 16f, 16f, 16f, 16f, 16f };
                    tableData.SetWidths(widthsCellsData);
                    tableData.WidthPercentage = 100;
                    tableData.AddCell(new PdfPCell(new Phrase("Code", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableData.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableData.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableData.AddCell(new PdfPCell(new Phrase("Cost", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PdfPCell header = (new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    header.Colspan = 4;
                    tableData.AddCell(header);
                    tableData.AddCell(new PdfPCell(new Phrase("Total Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PdfPCell headerColspan = (new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    headerColspan.Colspan = 2;
                    tableData.AddCell(headerColspan);
                    tableData.AddCell(new PdfPCell(new Phrase("Variance Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableData.AddCell(new PdfPCell(new Phrase("Beg", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableData.AddCell(new PdfPCell(new Phrase("In", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableData.AddCell(new PdfPCell(new Phrase("Out", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableData.AddCell(new PdfPCell(new Phrase("End", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableData.AddCell(new PdfPCell(new Phrase("Count", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableData.AddCell(new PdfPCell(new Phrase("Variance", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    foreach (var inventory in inventories)
                    {
                        totalAmount = inventory.Cost * inventory.EndQuantity;
                        count = 0;
                        quantityVariance = inventory.EndQuantity - count;
                        varianceAmount = inventory.Cost * quantityVariance;

                        tableData.AddCell(new PdfPCell(new Phrase(inventory.InventoryCode, fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase(inventory.Article, fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase(inventory.Unit, fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase(inventory.Cost.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase(inventory.BegQuantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase(inventory.InQuantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase(inventory.OutQuantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase(inventory.EndQuantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase("0.00", fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase(quantityVariance.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableData.AddCell(new PdfPCell(new Phrase(varianceAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    }

                    document.Add(tableData);

                    foreach (var articleInventory in inventories)
                    {
                        totalTotalAmount = totalTotalAmount + (articleInventory.Cost * articleInventory.EndQuantity);
                        quantityVariance = articleInventory.EndQuantity - count;
                        totalVarianceAmount = totalVarianceAmount + (articleInventory.Cost * quantityVariance);
                    }

                    totalAmountForFooter = totalAmountForFooter + totalTotalAmount;
                    totalVarianceForFooter = totalVarianceForFooter + totalVarianceAmount;

                    PdfPTable tableFooter = new PdfPTable(12);
                    float[] widthscellsfooter = new float[] { 20f, 30f, 15f, 16f, 16f, 16f, 12f, 20f, 16f, 16f, 16f, 16f };
                    tableFooter.SetWidths(widthscellsfooter);
                    tableFooter.WidthPercentage = 100;
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase("Sub Total", fontArial11Bold)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(totalTotalAmount.ToString("#,##0.00"), fontArial11Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 11f });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
                    tableFooter.AddCell(new PdfPCell(new Phrase(totalVarianceAmount.ToString("#,##0.00"), fontArial11Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 11f });
                    document.Add(tableFooter);

                    document.Add(Chunk.NEWLINE);
                }
            }

            PdfPTable tableFooterForTotal = new PdfPTable(12);
            float[] widthscellsfooterForTotal = new float[] { 20f, 30f, 15f, 16f, 16f, 16f, 16f, 16f, 16f, 16f, 16f, 16f };
            tableFooterForTotal.SetWidths(widthscellsfooterForTotal);
            tableFooterForTotal.WidthPercentage = 100;
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase("Total", fontArial11Bold)) { HorizontalAlignment = 2, Border = 0, PaddingTop = 10f });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(totalAmountForFooter.ToString("#,##0.00"), fontArial11Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 11f });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(" ")) { Border = 0 });
            tableFooterForTotal.AddCell(new PdfPCell(new Phrase(totalVarianceForFooter.ToString("#,##0.00"), fontArial11Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 11f });
            document.Add(tableFooterForTotal);

            // Document End
            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}