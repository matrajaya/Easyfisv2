using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepFixedAssetsController : Controller
    {
        // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Item List
        [Authorize]
        public ActionResult FixedAssets(String StartDate, String EndDate, Int32 CompanyId)
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
            Font fontArial9Bold = FontFactory.GetFont("Arial", 9, Font.BOLD);
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Fixed Assets", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("", fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            var articleInventories = from d in db.MstArticleInventories
                                     where d.MstArticle.DateAcquired >= Convert.ToDateTime(StartDate)
                                     && d.MstArticle.DateAcquired <= Convert.ToDateTime(EndDate)
                                     && d.MstBranch.CompanyId == CompanyId
                                     && d.MstArticle.UsefulLife > 0
                                     select new Models.MstArticleInventory
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
                //PdfPTable tableItemGroup = new PdfPTable(1);
                //float[] widthscellsTableItemGroup = new float[] { 100f };
                //tableItemGroup.SetWidths(widthscellsTableItemGroup);
                //tableItemGroup.WidthPercentage = 100;
                //tableItemGroup.AddCell(new PdfPCell(new Phrase(itemGroup.ArticleGroup, columnFont)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 5f });

                //document.Add(tableItemGroup);

                document.Add(Chunk.NEWLINE);

                PdfPTable tableItems = new PdfPTable(11);
                float[] widthscellsTableTtems = new float[] { 100f, 150f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                tableItems.SetWidths(widthscellsTableTtems);
                tableItems.WidthPercentage = 100;
                tableItems.AddCell(new PdfPCell(new Phrase("Code", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableItems.AddCell(new PdfPCell(new Phrase("Item", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableItems.AddCell(new PdfPCell(new Phrase("Unit", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableItems.AddCell(new PdfPCell(new Phrase("Cost", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableItems.AddCell(new PdfPCell(new Phrase("Quantity", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableItems.AddCell(new PdfPCell(new Phrase("Total Amount", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableItems.AddCell(new PdfPCell(new Phrase("Date Aquired", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableItems.AddCell(new PdfPCell(new Phrase("No. of Years", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableItems.AddCell(new PdfPCell(new Phrase("Salvage Value", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableItems.AddCell(new PdfPCell(new Phrase("Accumulated Depreciation", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                tableItems.AddCell(new PdfPCell(new Phrase("Adjusted Total Amount", fontArial9Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                Decimal totalAmount = 0;
                Decimal totalAccumulatedDepreciation = 0;
                Decimal totalAdjustedTotalAmount = 0;
                foreach (var item in articleInventories)
                {
                    tableItems.AddCell(new PdfPCell(new Phrase(item.ManualArticleCode, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableItems.AddCell(new PdfPCell(new Phrase(item.Article, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableItems.AddCell(new PdfPCell(new Phrase(item.Unit, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableItems.AddCell(new PdfPCell(new Phrase(item.Cost.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableItems.AddCell(new PdfPCell(new Phrase(item.Quantity.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableItems.AddCell(new PdfPCell(new Phrase(item.Amount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableItems.AddCell(new PdfPCell(new Phrase(item.DateAquired, fontArial9)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f });
                    tableItems.AddCell(new PdfPCell(new Phrase(item.NoOfYears.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableItems.AddCell(new PdfPCell(new Phrase(item.SalvageValue.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableItems.AddCell(new PdfPCell(new Phrase(item.AccumulatedDepreciation.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    tableItems.AddCell(new PdfPCell(new Phrase(item.AdjustedTotalAmount.ToString("#,##0.00"), fontArial9)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });

                    totalAmount = totalAmount + item.Amount;
                    totalAccumulatedDepreciation = totalAccumulatedDepreciation + item.AccumulatedDepreciation;
                    totalAdjustedTotalAmount = totalAdjustedTotalAmount + item.AdjustedTotalAmount;
                }

                document.Add(tableItems);

                PdfPTable tableTotal = new PdfPTable(11);
                float[] widthscellsTableTotal = new float[] { 100f, 150f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f, 100f };
                tableTotal.SetWidths(widthscellsTableTotal);
                tableTotal.WidthPercentage = 100;
                tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotal.AddCell(new PdfPCell(new Phrase("TOTAL ", fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotal.AddCell(new PdfPCell(new Phrase(totalAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial9Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotal.AddCell(new PdfPCell(new Phrase(totalAccumulatedDepreciation.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                tableTotal.AddCell(new PdfPCell(new Phrase(totalAdjustedTotalAmount.ToString("#,##0.00"), fontArial9Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

                document.Add(tableTotal);
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