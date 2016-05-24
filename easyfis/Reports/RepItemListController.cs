using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNet.Identity;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace easyfis.Reports
{
    public class RepItemListController : Controller
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
        public ActionResult ItemList(Int32 ItemGroupId)
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
            tableHeaderPage.AddCell(new PdfPCell(new Phrase("Item List", fontArial17Bold)) { Border = 0, HorizontalAlignment = 2 });
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
                // Article Items
                var items = from d in db.MstArticles
                            where d.ArticleGroupId == itemGroup.ArticleGroupId
                            && d.ArticleTypeId == 1
                            select new Models.MstArticle
                            {
                                Id = d.Id,
                                ArticleCode = d.ArticleCode,
                                ManualArticleCode = d.ManualArticleCode,
                                Article = d.Article,
                                Category = d.Category,
                                ArticleTypeId = d.ArticleTypeId,
                                ArticleType = d.MstArticleType.ArticleType,
                                ArticleGroupId = d.ArticleGroupId,
                                ArticleGroup = d.MstArticleGroup.ArticleGroup,
                                AccountId = d.AccountId,
                                AccountCode = d.MstAccount.AccountCode,
                                Account = d.MstAccount.Account,
                                SalesAccountId = d.SalesAccountId,
                                SalesAccount = d.MstAccount1.Account,
                                CostAccountId = d.CostAccountId,
                                CostAccount = d.MstAccount2.Account,
                                AssetAccountId = d.AssetAccountId,
                                AssetAccount = d.MstAccount3.Account,
                                ExpenseAccountId = d.ExpenseAccountId,
                                ExpenseAccount = d.MstAccount4.Account,
                                UnitId = d.UnitId,
                                Unit = d.MstUnit.Unit,
                                InputTaxId = d.InputTaxId,
                                InputTax = d.MstTaxType.TaxType,
                                OutputTaxId = d.OutputTaxId,
                                OutputTax = d.MstTaxType1.TaxType,
                                WTaxTypeId = d.WTaxTypeId,
                                WTaxType = d.MstTaxType2.TaxType,
                                Price = d.Price,
                                Cost = d.Cost,
                                IsInventory = d.IsInventory,
                                Particulars = d.Particulars,
                                Address = d.Address,
                                TermId = d.TermId,
                                Term = d.MstTerm.Term,
                                ContactNumber = d.ContactNumber,
                                ContactPerson = d.ContactPerson,
                                TaxNumber = d.TaxNumber,
                                CreditLimit = d.CreditLimit,
                                DateAcquired = d.DateAcquired.ToShortDateString(),
                                UsefulLife = d.UsefulLife,
                                SalvageValue = d.SalvageValue,
                                ManualArticleOldCode = d.ManualArticleOldCode
                            };

                if (items.Any())
                {
                    PdfPTable tableItemGroup = new PdfPTable(1);
                    float[] widthscellsTableItemGroup = new float[] { 100f };
                    tableItemGroup.SetWidths(widthscellsTableItemGroup);
                    tableItemGroup.WidthPercentage = 100;
                    tableItemGroup.AddCell(new PdfPCell(new Phrase(itemGroup.ArticleGroup, fontArial10Bold)) { HorizontalAlignment = 0, PaddingTop = 10f, PaddingBottom = 9f, Border = 0 });
                    document.Add(tableItemGroup);

                    PdfPTable tableItems = new PdfPTable(6);
                    float[] widthscellsTableTtems = new float[] { 15f, 20f, 35f, 25f, 20f, 20f };
                    tableItems.SetWidths(widthscellsTableTtems);
                    tableItems.WidthPercentage = 100;
                    tableItems.AddCell(new PdfPCell(new Phrase("Code", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableItems.AddCell(new PdfPCell(new Phrase("Manual Code", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableItems.AddCell(new PdfPCell(new Phrase("Item", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableItems.AddCell(new PdfPCell(new Phrase("Unit", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableItems.AddCell(new PdfPCell(new Phrase("Cost", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableItems.AddCell(new PdfPCell(new Phrase("Price", fontArial11Bold)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f, BackgroundColor = BaseColor.LIGHT_GRAY });

                    foreach (var item in items)
                    {
                        tableItems.AddCell(new PdfPCell(new Phrase(item.ArticleCode, fontArial10)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableItems.AddCell(new PdfPCell(new Phrase(item.ManualArticleCode, fontArial10)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableItems.AddCell(new PdfPCell(new Phrase(item.Article, fontArial10)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableItems.AddCell(new PdfPCell(new Phrase(item.Unit, fontArial10)) { HorizontalAlignment = 1, PaddingTop = 3f, PaddingBottom = 5f });
                        tableItems.AddCell(new PdfPCell(new Phrase(Convert.ToDecimal(item.Cost).ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                        tableItems.AddCell(new PdfPCell(new Phrase(item.Price.ToString("#,##0.00"), fontArial10)) { HorizontalAlignment = 2, PaddingTop = 3f, PaddingBottom = 5f });
                    }

                    document.Add(tableItems);
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