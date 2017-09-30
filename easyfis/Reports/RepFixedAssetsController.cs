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
    public class RepFixedAssetsController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ========================
        // Fixed Asset Report - PDF
        // ========================
        [Authorize]
        public ActionResult FixedAssets(String StartDate, String EndDate, Int32 CompanyId, Int32 BranchId)
        {
            // ============
            // PDF Settings
            // ============
            MemoryStream workStream = new MemoryStream();
            Rectangle rectangle = new Rectangle(PageSize.A3);
            Document document = new Document(rectangle, 72, 72, 72, 72);
            document.SetMargins(30f, 30f, 30f, 30f);
            PdfWriter.GetInstance(document, workStream).CloseStream = false;

            document.Open();

            // ===================
            // Fonts Customization
            // ===================
            Font fontArial17Bold = FontFactory.GetFont("Arial", 17, Font.BOLD);
            Font fontArial11 = FontFactory.GetFont("Arial", 11);
            Font fontArial9 = FontFactory.GetFont("Arial", 9);
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
            Font fontArial10Bold = FontFactory.GetFont("Arial", 10, Font.BOLD);
            Font fontArial10 = FontFactory.GetFont("Arial", 10);
            Font fontArial11Bold = FontFactory.GetFont("Arial", 11, Font.BOLD);
            Font fontArial12Bold = FontFactory.GetFont("Arial", 12, Font.BOLD);

            Paragraph line = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 4.5F)));

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.MstCompany.Company).FirstOrDefault();
            var address = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.MstCompany.Address).FirstOrDefault();
            var contactNo = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.MstCompany.ContactNumber).FirstOrDefault();
            var branch = (from d in db.MstBranches where d.Id == Convert.ToInt32(BranchId) select d.Branch).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Fixed Assets", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Date From " + Convert.ToDateTime(StartDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture) + " to " + Convert.ToDateTime(EndDate).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
            document.Add(line);

            // ========
            // Get Data
            // ========
            var articleInventories = from d in db.MstArticleInventories
                                     where d.MstArticle.DateAcquired >= Convert.ToDateTime(StartDate)
                                     && d.MstArticle.DateAcquired <= Convert.ToDateTime(EndDate)
                                     && d.MstBranch.CompanyId == CompanyId
                                     && d.BranchId == BranchId
                                     && d.MstArticle.UsefulLife > 0
                                     select new
                                     {
                                         Id = d.Id,
                                         BranchId = d.BranchId,
                                         ArticleId = d.ArticleId,
                                         ManualArticleCode = d.MstArticle.ManualArticleCode,
                                         Article = d.MstArticle.Article,
                                         Unit = d.MstArticle.MstUnit.Unit,
                                         Cost = d.Cost,
                                         Quantity = d.Quantity,
                                         Amount = d.Amount,
                                         DateAquired = d.MstArticle.DateAcquired.ToShortDateString(),
                                         NoOfYears = d.MstArticle.UsefulLife,
                                         SalvageValue = d.MstArticle.SalvageValue,
                                         AccumulatedDepreciation = 0,
                                         AdjustedTotalAmount = 0,
                                         InventoryCode = d.InventoryCode,
                                         Particulars = d.Particulars
                                     };

            if (articleInventories.Any())
            {
                // ============
                // Branch Title
                // ============
                PdfPTable branchTitle = new PdfPTable(1);
                float[] widthCellsBranchTitle = new float[] { 100f };
                branchTitle.SetWidths(widthCellsBranchTitle);
                branchTitle.WidthPercentage = 100;
                PdfPCell branchHeaderColspan = (new PdfPCell(new Phrase(branch, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 14f });
                branchTitle.AddCell(branchHeaderColspan);
                document.Add(branchTitle);

                PdfPTable tableData = new PdfPTable(11);
                float[] widthscellsTableTtems = new float[] { 100f, 150f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                tableData.SetWidths(widthscellsTableTtems);
                tableData.WidthPercentage = 100;
                tableData.AddCell(new PdfPCell(new Phrase("Code", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Item", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Unit", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Cost", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Quantity", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Total Amount", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Date Aquired", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("No. of Years", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Salvage Value", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Accumulated Depreciation", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableData.AddCell(new PdfPCell(new Phrase("Adjusted Total Amount", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal totalAmount = 0;
                Decimal totalAccumulatedDepreciation = 0;
                Decimal totalAdjustedTotalAmount = 0;

                foreach (var item in articleInventories)
                {
                    tableData.AddCell(new PdfPCell(new Phrase(item.ManualArticleCode, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(item.Article, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(item.Unit, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(item.Cost.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(item.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(item.DateAquired, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(item.NoOfYears.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(item.SalvageValue.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(item.AccumulatedDepreciation.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                    tableData.AddCell(new PdfPCell(new Phrase(item.AdjustedTotalAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                    totalAmount = totalAmount + item.Amount;
                    totalAccumulatedDepreciation = totalAccumulatedDepreciation + item.AccumulatedDepreciation;
                    totalAdjustedTotalAmount = totalAdjustedTotalAmount + item.AdjustedTotalAmount;
                }

                tableData.AddCell(new PdfPCell(new Phrase("Total ", fontArial9Bold)) { Colspan = 4, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableData.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableData.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Colspan = 3, HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableData.AddCell(new PdfPCell(new Phrase(totalAccumulatedDepreciation.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                tableData.AddCell(new PdfPCell(new Phrase(totalAdjustedTotalAmount.ToString("#,##0.00"), fontArial9Bold)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                document.Add(tableData);
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}