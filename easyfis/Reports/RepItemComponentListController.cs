using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepItemComponentListController : Controller
    {
        // ============
        // Data Context
        // ============
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // ================
        // Get Highest Cost
        // ================
        public Decimal GetHighestCost(Int32 articleId)
        {
            var articleInventories = from d in db.MstArticleInventories.OrderByDescending(d => d.Cost)
                                     where d.ArticleId == articleId
                                     select d;

            if (articleInventories.Any())
            {
                return articleInventories.FirstOrDefault().Cost;
            }
            else
            {
                return 0;
            }
        }

        // ================================
        // Item Component List Report - PDF
        // ================================
        [Authorize]
        public ActionResult ItemComponentList(Int32 ItemGroupId)
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

            var identityUserId = User.Identity.GetUserId();
            var currentUser = from d in db.MstUsers where d.UserId == identityUserId select d;
            var currentCompanyId = currentUser.FirstOrDefault().CompanyId;

            // ==============
            // Company Detail
            // ==============
            var companyName = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.Company).FirstOrDefault();
            var address = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.Address).FirstOrDefault();
            var contactNo = (from d in db.MstCompanies where d.Id == Convert.ToInt32(currentCompanyId) select d.ContactNumber).FirstOrDefault();

            // ===========
            // Header Page
            // ===========
            PdfPTable headerPage = new PdfPTable(2);
            float[] widthsCellsHeaderPage = new float[] { 100f, 75f };
            headerPage.SetWidths(widthsCellsHeaderPage);
            headerPage.WidthPercentage = 100;
            headerPage.AddCell(new PdfPCell(new Phrase(companyName, fontArial17Bold)) { Border = 0 });
            headerPage.AddCell(new PdfPCell(new Phrase("Item Component List", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase(" ", fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            headerPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            headerPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(headerPage);
            document.Add(line);

            // ========
            // Get Data
            // ========
            var articleComponents = from d in db.MstArticleComponents
                                    where d.MstArticle.ArticleGroupId == ItemGroupId
                                    select new
                                    {
                                        Id = d.Id,
                                        ArticleId = d.ArticleId,
                                        Article = d.MstArticle.Article,
                                        ComponentManualArticleCode = d.MstArticle1.ManualArticleCode,
                                        ComponentArticleId = d.ComponentArticleId,
                                        ComponentArticle = d.MstArticle1.Article,
                                        Quantity = d.Quantity,
                                        Unit = d.MstArticle1.MstUnit.Unit,
                                        Cost = GetHighestCost(d.ComponentArticleId),
                                        Particulars = d.MstArticle.Particulars,
                                    };

            if (articleComponents.Any())
            {
                // =========
                // Get Items
                // =========
                var groupedItems = from d in articleComponents
                                   group d by new
                                   {
                                       ArticleId = d.ArticleId,
                                       Article = d.Article
                                   } into g
                                   select new
                                   {
                                       ArticleId = g.Key.ArticleId,
                                       Article = g.Key.Article
                                   };

                if (groupedItems.Any())
                {
                    var articleGroups = from d in db.MstArticleGroups
                                        where d.Id == Convert.ToInt32(ItemGroupId)
                                        select d;

                    if (articleGroups.Any())
                    {
                        // ===================
                        // Article Group Title
                        // ===================
                        PdfPTable articleGroupTitle = new PdfPTable(1);
                        float[] widthCellsArticleGroupTitle = new float[] { 100f };
                        articleGroupTitle.SetWidths(widthCellsArticleGroupTitle);
                        articleGroupTitle.WidthPercentage = 100;
                        PdfPCell articleGroupTitleCell = (new PdfPCell(new Phrase(articleGroups.FirstOrDefault().ArticleGroup, fontArial12Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f });
                        articleGroupTitle.AddCell(articleGroupTitleCell);
                        document.Add(articleGroupTitle);

                        Decimal totalAllAmount = 0;

                        foreach (var groupedItem in groupedItems)
                        {
                            // ===================
                            // Article Group Title
                            // ===================
                            PdfPTable articleTitle = new PdfPTable(1);
                            float[] widthCellsArticleTitle = new float[] { 100f };
                            articleTitle.SetWidths(widthCellsArticleTitle);
                            articleTitle.WidthPercentage = 100;
                            PdfPCell articleTitleCell = (new PdfPCell(new Phrase(groupedItem.Article, fontArial11Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f });
                            articleTitle.AddCell(articleTitleCell);
                            document.Add(articleTitle);

                            // ==============
                            // Get Components
                            // ==============
                            var components = from d in articleComponents
                                             where d.ArticleId == groupedItem.ArticleId
                                             select new
                                             {
                                                 Id = d.Id,
                                                 ArticleId = d.ArticleId,
                                                 Article = d.Article,
                                                 ComponentManualArticleCode = d.ComponentManualArticleCode,
                                                 ComponentArticleId = d.ComponentArticleId,
                                                 ComponentArticle = d.ComponentArticle,
                                                 Quantity = d.Quantity,
                                                 Unit = d.Unit,
                                                 Cost = d.Cost,
                                                 Particulars = d.Particulars,
                                             };

                            if (components.Any())
                            {
                                PdfPTable tableData = new PdfPTable(6);
                                float[] widthscellsTableTtems = new float[] { 25f, 35f, 20f, 20f, 20f, 20f };
                                tableData.SetWidths(widthscellsTableTtems);
                                tableData.WidthPercentage = 100;
                                tableData.AddCell(new PdfPCell(new Phrase("Code", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                                tableData.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                                tableData.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                                tableData.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                                tableData.AddCell(new PdfPCell(new Phrase("Cost", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                                tableData.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                                Decimal SubTotal = 0;

                                foreach (var component in components)
                                {
                                    tableData.AddCell(new PdfPCell(new Phrase(component.ComponentManualArticleCode, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableData.AddCell(new PdfPCell(new Phrase(component.ComponentArticle, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableData.AddCell(new PdfPCell(new Phrase(component.Quantity.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableData.AddCell(new PdfPCell(new Phrase(component.Unit, fontArial10)) { HorizontalAlignment = 0, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableData.AddCell(new PdfPCell(new Phrase(component.Cost.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                    tableData.AddCell(new PdfPCell(new Phrase((component.Cost * component.Quantity).ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });

                                    SubTotal += component.Cost * component.Quantity;
                                }

                                tableData.AddCell(new PdfPCell(new Phrase("Sub Total ", fontArial10Bold)) { Colspan = 5, HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                tableData.AddCell(new PdfPCell(new Phrase(SubTotal.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, PaddingLeft = 5f, PaddingRight = 5f });
                                document.Add(tableData);

                                totalAllAmount += SubTotal;
                            }
                        }

                        document.Add(line);

                        PdfPTable tableTotal = new PdfPTable(6);
                        float[] widthCellsTableTotal = new float[] { 25f, 35f, 20f, 20f, 20f, 20f };
                        tableTotal.SetWidths(widthCellsTableTotal);
                        tableTotal.WidthPercentage = 100;
                        tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                        tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                        tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                        tableTotal.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                        tableTotal.AddCell(new PdfPCell(new Phrase("Total ", fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                        tableTotal.AddCell(new PdfPCell(new Phrase(totalAllAmount.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0, PaddingLeft = 5f, PaddingRight = 5f });
                        document.Add(tableTotal);
                    }
                }
            }

            document.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return new FileStreamResult(workStream, "application/pdf");
        }
    }
}