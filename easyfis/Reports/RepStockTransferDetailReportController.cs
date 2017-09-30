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
    public class RepStockTransferDetailReportController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // =================================
        // Stock Transfer Detail Report - PDF
        // ==================================
        [Authorize]
        public ActionResult StockTransferDetailReport(String StartDate, String EndDate, Int32 CompanyId, Int32 BranchId)
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
            headerPage.AddCell(new PdfPCell(new Phrase("Stock Transfer Detail Report", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Date From " + Convert.ToDateTime(StartDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + Convert.ToDateTime(EndDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
            document.Add(line);

            // ====================
            // Stock Transfer Items
            // ====================
            var stockTransferItems = from d in db.TrnStockTransferItems
                                     where d.TrnStockTransfer.MstBranch.CompanyId == Convert.ToInt32(CompanyId)
                                     && d.TrnStockTransfer.BranchId == Convert.ToInt32(BranchId)
                                     && d.TrnStockTransfer.STDate >= Convert.ToDateTime(StartDate)
                                     && d.TrnStockTransfer.STDate <= Convert.ToDateTime(EndDate)
                                     && d.TrnStockTransfer.IsLocked == true
                                     select new
                                     {
                                         Id = d.Id,
                                         STId = d.STId,
                                         ST = d.TrnStockTransfer.STNumber,
                                         STDate = d.TrnStockTransfer.STDate.ToString("MM-dd-yyyy", CultureInfo.InvariantCulture),
                                         ToBranchId = d.TrnStockTransfer.ToBranchId,
                                         ToBranch = d.TrnStockTransfer.MstBranch1.Branch,
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
                                     };

            if (stockTransferItems.Any())
            {
                Decimal totalAllBranches = 0;

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

                // =========================
                // To Branch Title - Grouped
                // =========================
                var toBranches = from d in stockTransferItems
                                 group d by new
                                 {
                                     ToBranchId = d.ToBranchId,
                                     ToBranch = d.ToBranch
                                 } into g
                                 select new
                                 {
                                     ToBranchId = g.Key.ToBranchId,
                                     ToBranch = g.Key.ToBranch
                                 };

                if (toBranches.Any())
                {
                    foreach (var toBranch in toBranches)
                    {
                        // ===============
                        // To Branch Title
                        // ===============
                        PdfPTable toBranchTitle = new PdfPTable(1);
                        float[] widthCellsToBranchTitle = new float[] { 100f };
                        toBranchTitle.SetWidths(widthCellsToBranchTitle);
                        toBranchTitle.WidthPercentage = 100;
                        PdfPCell toBranchHeaderColspan = (new PdfPCell(new Phrase("To Branch: " + toBranch.ToBranch, fontArial11Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f });
                        toBranchTitle.AddCell(toBranchHeaderColspan);
                        document.Add(toBranchTitle);

                        // =========
                        // Get Items
                        // =========
                        var items = from d in stockTransferItems
                                    where d.ToBranchId == toBranch.ToBranchId
                                    select new
                                    {
                                        Id = d.Id,
                                        STId = d.STId,
                                        ToBranchId = d.ToBranchId,
                                        ToBranch = d.ToBranch,
                                        ST = d.ST,
                                        STDate = d.STDate,
                                        ItemId = d.ItemId,
                                        ItemCode = d.ItemCode,
                                        Item = d.Item,
                                        ItemInventoryId = d.ItemInventoryId,
                                        ItemInventory = d.ItemInventory,
                                        Particulars = d.Particulars,
                                        UnitId = d.UnitId,
                                        Unit = d.Unit,
                                        Quantity = d.Quantity,
                                        Cost = d.Cost,
                                        Amount = d.Amount,
                                        BaseUnitId = d.BaseUnitId,
                                        BaseUnit = d.BaseUnit,
                                        BaseQuantity = d.BaseQuantity,
                                        BaseCost = d.BaseCost,
                                    };

                        if (items.Any())
                        {
                            PdfPTable tableData = new PdfPTable(7);
                            float[] widthscellsTableINtems = new float[] { 20f, 15f, 35f, 20f, 20f, 20f, 20f };
                            tableData.SetWidths(widthscellsTableINtems);
                            tableData.WidthPercentage = 100;
                            tableData.AddCell(new PdfPCell(new Phrase("OT Number", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("OT Date", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Cost", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                            Decimal total = 0;

                            foreach (var item in items)
                            {
                                tableData.AddCell(new PdfPCell(new Phrase(item.ST, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableData.AddCell(new PdfPCell(new Phrase(item.STDate, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableData.AddCell(new PdfPCell(new Phrase(item.Item, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableData.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableData.AddCell(new PdfPCell(new Phrase(item.Unit, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableData.AddCell(new PdfPCell(new Phrase(item.Cost.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableData.AddCell(new PdfPCell(new Phrase(item.Amount.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                                total = total + item.Amount;
                            }

                            tableData.AddCell(new PdfPCell(new Phrase("Sub Total", fontArial10Bold)) { Colspan = 6, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                            tableData.AddCell(new PdfPCell(new Phrase(total.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                            document.Add(tableData);

                            totalAllBranches += total;
                        }
                    }
                }

                document.Add(line);

                PdfPTable tableTotalAllBranches = new PdfPTable(7);
                float[] widthsCellsTableTotalAllBranches = new float[] { 20f, 15f, 35f, 20f, 20f, 20f, 20f };
                tableTotalAllBranches.SetWidths(widthsCellsTableTotalAllBranches);
                tableTotalAllBranches.WidthPercentage = 100;
                tableTotalAllBranches.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAllBranches.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAllBranches.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAllBranches.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAllBranches.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                tableTotalAllBranches.AddCell(new PdfPCell(new Phrase("Total", fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Border = 0 });
                tableTotalAllBranches.AddCell(new PdfPCell(new Phrase(totalAllBranches.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f, Border = 0 });
                document.Add(tableTotalAllBranches);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}