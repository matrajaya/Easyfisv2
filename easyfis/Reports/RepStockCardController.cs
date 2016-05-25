using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepStockCardController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Stock Card
        [Authorize]
        public ActionResult StockCard(String StartDate, String EndDate, Int32 BranchId, Int32 ItemId)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Stock Card", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Date From " + StartDate + " to " + EndDate, fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var inventories = from d in db.TrnInventories.OrderBy(d => d.InventoryDate)
                              where d.InventoryDate >= Convert.ToDateTime(StartDate)
                              && d.InventoryDate <= Convert.ToDateTime(EndDate)
                              && d.BranchId == BranchId
                              && d.ArticleId == ItemId
                              select new Models.TrnInventory
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  BranchCode = d.MstBranch.BranchCode,
                                  InventoryDate = d.InventoryDate.ToShortDateString(),
                                  ArticleId = d.ArticleId,
                                  Article = d.MstArticle.Article,
                                  ArticleInventoryId = d.ArticleInventoryId,
                                  ArticleInventoryCode = d.MstArticleInventory.InventoryCode,
                                  RRId = d.RRId,
                                  RRNumber = d.TrnReceivingReceipt.RRNumber,
                                  SIId = d.SIId,
                                  SINumber = d.TrnSalesInvoice.SINumber,
                                  INId = d.INId,
                                  INNumber = d.TrnStockIn.INNumber,
                                  OTId = d.OTId,
                                  OTNumber = d.TrnStockOut.OTNumber,
                                  STId = d.STId,
                                  STNumber = d.TrnStockTransfer.STNumber,
                                  QuantityIn = d.QuantityIn,
                                  Quantity = d.Quantity,
                                  QuantityOut = d.QuantityOut,
                                  Amount = d.Amount,
                                  Particulars = d.Particulars,
                                  Cost = d.MstArticle.Cost,
                                  Unit = d.MstArticle.MstUnit.Unit
                              };

            if (inventories.Any())
            {
                var detailBranch = (from d in db.MstBranches where d.Id == BranchId select d.Branch).SingleOrDefault();

                String ItemName = "";
                foreach (var inventory in inventories)
                {
                    ItemName = inventory.Article;
                }

                // table branch header
                PdfPTable tableBranchHeader = new PdfPTable(1);
                float[] widthCellsTableBranchHeader = new float[] { 100f };
                tableBranchHeader.SetWidths(widthCellsTableBranchHeader);
                tableBranchHeader.WidthPercentage = 100;
                tableBranchHeader.AddCell(new PdfPCell(new Phrase(detailBranch, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                tableBranchHeader.AddCell(new PdfPCell(new Phrase(ItemName, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 9f, PaddingBottom = 10f });
                document.Add(tableBranchHeader);

                PdfPTable tableData = new PdfPTable(9);
                PdfPCell Cell = new PdfPCell();
                float[] widthsCellsData = new float[] { 20f, 30f, 15f, 16f, 16f, 16f, 16f, 16f, 16f };
                tableData.SetWidths(widthsCellsData);
                tableData.WidthPercentage = 100;
                tableData.AddCell(new PdfPCell(new Phrase("Date", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Document", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                PdfPCell header = (new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                header.Colspan = 4;
                tableData.AddCell(header);
                tableData.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Cost", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Beg", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("In", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Out", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("End", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal TotalBeg = 0;
                Decimal TotalIn = 0;
                Decimal TotalOut = 0;
                Decimal TotalEnd = 0;
                Decimal TotalAmount = 0;
                Decimal TotalCost = 0;

                String InventoryCodeDocument = "";
                foreach (var inventory in inventories)
                {
                    if (inventory.RRId != null)
                    {
                        InventoryCodeDocument = "RR-" + inventory.BranchCode + "-" + inventory.RRNumber;
                    }
                    else if (inventory.SIId != null)
                    {
                        InventoryCodeDocument = "SI-" + inventory.BranchCode + "-" + inventory.SINumber;
                    }
                    else if (inventory.INId != null)
                    {
                        InventoryCodeDocument = "IN-" + inventory.BranchCode + "-" + inventory.INNumber;
                    }
                    else if (inventory.OTId != null)
                    {
                        InventoryCodeDocument = "OT-" + inventory.BranchCode + "-" + inventory.OTNumber;
                    }
                    else if (inventory.STId != null)
                    {
                        InventoryCodeDocument = "ST-" + inventory.BranchCode + "-" + inventory.STNumber;
                    }
                    else
                    {
                        InventoryCodeDocument = "";
                    }

                    tableData.AddCell(new PdfPCell(new Phrase(inventory.InventoryDate, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(InventoryCodeDocument, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase("0.00", fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(inventory.QuantityIn.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(inventory.QuantityOut.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(inventory.Quantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(inventory.Unit, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(Convert.ToInt32(inventory.Cost).ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(inventory.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    TotalBeg = TotalBeg + 0;
                    TotalIn = TotalIn + inventory.QuantityIn;
                    TotalOut = TotalOut + inventory.QuantityOut;
                    TotalEnd = TotalEnd + inventory.Quantity;
                    TotalAmount = TotalAmount + inventory.Amount;
                    TotalCost = TotalCost + Convert.ToInt32(inventory.Cost);
                }

                document.Add(tableData);
                document.Add(Chunk.NEWLINE);

                PdfPTable tableFooter = new PdfPTable(9);
                float[] widthsCellsFooter = new float[] { 20f, 30f, 15f, 16f, 16f, 16f, 16f, 16f, 16f };
                tableFooter.SetWidths(widthsCellsFooter);
                tableFooter.WidthPercentage = 100;
                tableFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase("TOTAL", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(TotalBeg.ToString("#,##0.00"), fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(TotalIn.ToString("#,##0.00"), fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(TotalOut.ToString("#,##0.00"), fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(TotalEnd.ToString("#,##0.00"), fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase("", fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                tableFooter.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), fontArial11Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                document.Add(tableFooter);
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