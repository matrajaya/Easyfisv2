using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepStockCardController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =====================
        // Stock Card Report PDF
        // =====================
        [Authorize]
        public ActionResult StockCard(String StartDate, String EndDate, Int32 CompanyId, Int32 BranchId, Int32 ItemId)
        {
            // ==============================
            // PDF Settings and Customization
            // ==============================
            MemoryStream workStream = new MemoryStream();
            Rectangle rectangle = new Rectangle(PageSize.A3);
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            // =====
            // Fonts
            // =====
            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstCompanies where d.Id == Convert.ToInt32(CompanyId) select d.Company).FirstOrDefault();
            var address = (from d in db.MstCompanies where d.Id == Convert.ToInt32(CompanyId) select d.Address).FirstOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == Convert.ToInt32(CompanyId) select d.ContactNumber).FirstOrDefault();
            var branch = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.Branch).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Stock Card", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Date From " + Convert.ToDateTime(StartDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + Convert.ToDateTime(EndDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
            document.Add(line);

            var inventories = from d in db.TrnInventories.OrderBy(d => d.InventoryDate)
                              where d.InventoryDate >= Convert.ToDateTime(StartDate)
                              && d.InventoryDate <= Convert.ToDateTime(EndDate)
                              && d.MstBranch.CompanyId == Convert.ToInt32(CompanyId)
                              && d.BranchId == Convert.ToInt32(BranchId)
                              && d.ArticleId == Convert.ToInt32(ItemId)
                              select new Models.TrnInventory
                              {
                                  Id = d.Id,
                                  BranchId = d.BranchId,
                                  Branch = d.MstBranch.Branch,
                                  BranchCode = d.MstBranch.BranchCode,
                                  InventoryDate = d.InventoryDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture),
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
                // ============
                // Branch Title
                // ============
                PdfPTable branchTitle = new PdfPTable(1);
                float[] widthCellsBranchTitle = new float[] { 100f };
                branchTitle.SetWidths(widthCellsBranchTitle);
                branchTitle.WidthPercentage = 100;
                PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                branchTitle.AddCell(branchHeaderColspan);
                document.Add(branchTitle);

                // ==========
                // Item Title 
                // ==========
                PdfPTable itemTitle = new PdfPTable(1);
                float[] widthCellsItemTitle = new float[] { 100f };
                itemTitle.SetWidths(widthCellsItemTitle);
                itemTitle.WidthPercentage = 100;
                itemTitle.AddCell(new PdfPCell(new Phrase(inventories.FirstOrDefault().Article, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingBottom = 14f });
                document.Add(itemTitle);

                // ====
                // Data
                // ====
                PdfPTable data = new PdfPTable(9);
                float[] widthsCellsData = new float[] { 20f, 30f, 15f, 16f, 16f, 16f, 16f, 16f, 16f };
                data.SetWidths(widthsCellsData);
                data.WidthPercentage = 100;
                data.AddCell(new PdfPCell(new Phrase("Date", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Document", fontArial11Bold)) { HorizontalAlignment = 1, Rowspan = 2, PaddingTop = 3f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { Colspan = 4, HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Cost", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Beg", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("In", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("Out", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                data.AddCell(new PdfPCell(new Phrase("End", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

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

                    data.AddCell(new PdfPCell(new Phrase(inventory.InventoryDate, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    data.AddCell(new PdfPCell(new Phrase(InventoryCodeDocument, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    data.AddCell(new PdfPCell(new Phrase("0.00", fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    data.AddCell(new PdfPCell(new Phrase(inventory.QuantityIn.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    data.AddCell(new PdfPCell(new Phrase(inventory.QuantityOut.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    data.AddCell(new PdfPCell(new Phrase(inventory.Quantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    data.AddCell(new PdfPCell(new Phrase(inventory.Unit, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    data.AddCell(new PdfPCell(new Phrase(Convert.ToInt32(inventory.Cost).ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    data.AddCell(new PdfPCell(new Phrase(inventory.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                    TotalBeg += 0;
                    TotalIn += inventory.QuantityIn;
                    TotalOut += inventory.QuantityOut;
                    TotalEnd += inventory.Quantity;
                    TotalAmount += inventory.Amount;
                    TotalCost += +Convert.ToInt32(inventory.Cost);
                }

                data.AddCell(new PdfPCell(new Phrase("TOTAL", fontArial9Bold)) { Colspan = 2, Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                data.AddCell(new PdfPCell(new Phrase(TotalBeg.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                data.AddCell(new PdfPCell(new Phrase(TotalIn.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                data.AddCell(new PdfPCell(new Phrase(TotalOut.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                data.AddCell(new PdfPCell(new Phrase(TotalEnd.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                data.AddCell(new PdfPCell(new Phrase(TotalAmount.ToString("#,##0.00"), fontArial9Bold)) { Colspan = 3, Border = 0, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(data);
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