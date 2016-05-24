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
        // Highest Cost
        public Decimal getHighestCost(Int32 ArticleId)
        {
            var articleInventoryCost = (from d in db.MstArticleInventories.OrderByDescending(d => d.Cost)
                                        where d.ArticleId == ArticleId
                                        select d.Cost).FirstOrDefault();

            return articleInventoryCost;
        }

         // Easyfis data context
        private Data.easyfisdbDataContext db = new Data.easyfisdbDataContext();

        // current branch Id
        public Int32 currentBranchId()
        {
            var identityUserId = User.Identity.GetUserId();
            return (from d in db.MstUsers where d.UserId == identityUserId select d.BranchId).SingleOrDefault();
        }

        // PDF Item Component List
        [Authorize]
        public ActionResult ItemComponentList(Int32 ItemGroupId)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Item Component List", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(address, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("", fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2, });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase(contactNo, fontArial11)) { Border = 0, PaddingTop = 5f });
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Printed " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToString("hh:mm:ss tt"), fontArial11)) { Border = 0, PaddingTop = 5f, HorizontalAlignment = 2 });
            document.Add(tableHeaderPage);
            document.Add(line);

            // Item Groups
            var itemGroups = from d in db.MstArticles
                             where d.ArticleTypeId == 1
                             && d.ArticleGroupId == ItemGroupId
                             group d by new
                             {
                                 ArticleGroupId = d.ArticleGroupId,
                                 ArticleGroup = d.MstArticleGroup.ArticleGroup
                             } into g
                             select new Models.MstArticle
                             {
                                 ArticleGroupId = g.Key.ArticleGroupId,
                                 ArticleGroup = g.Key.ArticleGroup
                             };

            foreach (var itemGroup in itemGroups)
            {
                var articleComponentGroups = from d in db.MstArticleComponents
                                             where d.MstArticle.ArticleGroupId == itemGroup.ArticleGroupId
                                             group d by new
                                             {
                                                 ArticleId = d.ArticleId,
                                                 Article = d.MstArticle.Article,
                                             } into g
                                             select new Models.MstArticleComponent
                                             {
                                                 ArticleId = g.Key.ArticleId,
                                                 Article = g.Key.Article
                                             };

                if (articleComponentGroups.Any())
                {
                    PdfPTable tableItemGroup = new PdfPTable(1);
                    float[] widthscellsTableItemGroup = new float[] { 100f };
                    tableItemGroup.SetWidths(widthscellsTableItemGroup);
                    tableItemGroup.WidthPercentage = 100;
                    tableItemGroup.AddCell(new PdfPCell(new Phrase(itemGroup.ArticleGroup, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, Border = 0 });

                    document.Add(tableItemGroup);

                    Decimal Total = 0;
                    foreach (var articleComponentGroup in articleComponentGroups)
                    {
                        var articleComponents = from d in db.MstArticleComponents
                                                where d.ArticleId == articleComponentGroup.ArticleId
                                                select new Models.MstArticleComponent
                                                {
                                                    Id = d.Id,
                                                    ArticleId = d.ArticleId,
                                                    Article = d.MstArticle.Article,
                                                    ManualArticleCode = d.MstArticle.ManualArticleCode,
                                                    ComponentArticleId = d.ComponentArticleId,
                                                    ComponentArticle = d.MstArticle1.Article,
                                                    Quantity = d.Quantity,
                                                    Unit = d.MstArticle.MstUnit.Unit,
                                                    Cost = getHighestCost(d.ArticleId),
                                                    Particulars = d.MstArticle.Particulars,
                                                };

                        PdfPTable tableItem = new PdfPTable(1);
                        float[] widthscellsTableItem = new float[] { 100f };
                        tableItem.SetWidths(widthscellsTableItem);
                        tableItem.WidthPercentage = 100;
                        tableItem.AddCell(new PdfPCell(new Phrase(articleComponentGroup.Article, fontArial10Bold)) { Border = 0, HorizontalAlignment = 0, PaddingTop = 9f, PaddingBottom = 10f });

                        document.Add(tableItem);

                        PdfPTable tableItems = new PdfPTable(6);
                        float[] widthscellsTableTtems = new float[] { 15f, 35f, 20f, 25f, 20f, 20f };
                        tableItems.SetWidths(widthscellsTableTtems);
                        tableItems.WidthPercentage = 100;
                        tableItems.AddCell(new PdfPCell(new Phrase("Code", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableItems.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableItems.AddCell(new PdfPCell(new Phrase("Quantity", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableItems.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableItems.AddCell(new PdfPCell(new Phrase("Cost", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                        tableItems.AddCell(new PdfPCell(new Phrase("Amount", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                        Decimal SubTotal = 0;
                        foreach (var articleComponent in articleComponents)
                        {
                            tableItems.AddCell(new PdfPCell(new Phrase(articleComponent.ManualArticleCode, fontArial10)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                            tableItems.AddCell(new PdfPCell(new Phrase(articleComponent.Article, fontArial10)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                            tableItems.AddCell(new PdfPCell(new Phrase(articleComponent.Quantity.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableItems.AddCell(new PdfPCell(new Phrase(articleComponent.Unit, fontArial10)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                            tableItems.AddCell(new PdfPCell(new Phrase(articleComponent.Cost.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            tableItems.AddCell(new PdfPCell(new Phrase((articleComponent.Cost * articleComponent.Quantity).ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                            SubTotal = SubTotal + (articleComponent.Cost * articleComponent.Quantity);
                        }

                        document.Add(tableItems);

                        PdfPTable tableSubTotal = new PdfPTable(6);
                        float[] widthscellsTableSubTotal = new float[] { 15f, 20f, 35f, 25f, 20f, 20f };
                        tableSubTotal.SetWidths(widthscellsTableSubTotal);
                        tableSubTotal.WidthPercentage = 100;
                        tableSubTotal.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                        tableSubTotal.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                        tableSubTotal.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                        tableSubTotal.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { Border = 0, HorizontalAlignment = 1, PaddingTop = 10f, PaddingBottom = 5f });
                        tableSubTotal.AddCell(new PdfPCell(new Phrase("Sub Total: ", fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });
                        tableSubTotal.AddCell(new PdfPCell(new Phrase(SubTotal.ToString("#,##0.00"), fontArial10Bold)) { Border = 0, HorizontalAlignment = 2, PaddingTop = 10f, PaddingBottom = 5f });

                        document.Add(tableSubTotal);
                        Total = Total + SubTotal;
                    }

                    document.Add(Chunk.NEWLINE);
                    PdfPTable tableItemComponentItemFooter = new PdfPTable(6);
                    float[] widthscellsTableItemComponentItemFooter = new float[] { 15f, 35f, 20f, 25f, 20f, 20f };
                    tableItemComponentItemFooter.SetWidths(widthscellsTableItemComponentItemFooter);
                    tableItemComponentItemFooter.WidthPercentage = 100;
                    tableItemComponentItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                    tableItemComponentItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                    tableItemComponentItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                    tableItemComponentItemFooter.AddCell(new PdfPCell(new Phrase("", fontArial10Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                    tableItemComponentItemFooter.AddCell(new PdfPCell(new Phrase("Total:", fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });
                    tableItemComponentItemFooter.AddCell(new PdfPCell(new Phrase(Total.ToString("#,##0.00"), fontArial10Bold)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f, Border = 0 });

                    document.Add(line);
                    document.Add(tableItemComponentItemFooter);
                }
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